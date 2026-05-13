using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU
{
    public class VCPU
    {
        #region Variables
        /// <summary>
        /// Memory this virtual CPU has.
        /// </summary>
        public ref Memory Memory => ref _memory;
        private Memory _memory;

        /// <summary>
        /// Provides methods to get and set the values of the registers, and to set and get the flags register,
        /// </summary>
        public ref RegisterManager Registers => ref _registers;
        private RegisterManager _registers;

        /// <summary>
        /// The program in bytes that is being executed, 
        /// this is set when the Run method is called, and is used to fetch instructions and data from it.
        /// </summary>
        private byte[] _program = new byte[0];

        /// <summary>
        /// The program that is being executed, this is set when the Run method is called, and is used to fetch instructions and data from it.
        /// </summary>
        public ref byte[] Program => ref _program;

        private OpcodeInstruction[] _opcodeActions;

        /// <summary>
        /// The program counter, this is used to keep track of the current instruction being executed in the program,
        /// </summary>
        private int _pc = 0;

        /// <summary>
        /// The index of the current instruction being executed in the program, 
        /// this is used to fetch instructions and data from the program, and is incremented after each instruction is executed.
        /// </summary>
        public int ProgramCounter => _pc;

        private bool _loggingEnabled = true;

        private bool _forceQuit = false;

        /// <summary>
        /// Handle to crash the program in 
        /// </summary>
        private Action<string> _crashHandle;
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the virtual CPU with the specified memory and stack sizes.
        /// </summary>
        /// <param name="programArray">The program in bytes to be executed</param>
        /// <param name="actions">The array of opcode instructions</param>
        /// <param name="memorySize">The size of the memory</param>
        /// <param name="stackSize">The size of the stack</param>
        /// <param name="loggingEnabled">Whether logging is enabled</param>
        public VCPU(byte[] programArray, OpcodeInstruction[] actions, bool loggingEnabled = true, uint memorySize = 16, uint stackSize = 8)
        {
            _loggingEnabled = loggingEnabled;
            _crashHandle = Crash;

            Initialize(memorySize, stackSize);

            Run(programArray, actions);
        }

        private void Initialize(uint memorySize, uint stackSize)
        {
            _memory = new Memory(new byte[memorySize], stackSize, this, _crashHandle);
            _registers = new RegisterManager(this, _crashHandle);
        }

        /// <summary>
        /// Executes the program
        /// </summary>
        /// <param name="programArray">The program in bytes to be executed</param>
        private void Run(byte[] programArray, OpcodeInstruction[] actions)
        {
            Log("Executing the program", ConsoleColor.White);

            _program = programArray;
            this._opcodeActions = actions;

            while (_pc < _program.Length & !_forceQuit)
            {
                byte instruction = _program[_pc]; // Current instruction(the byte at the program counter)
                var opcode = _opcodeActions.Where(x => x.Accept(instruction)).FirstOrDefault();
                if (opcode == null)
                {
                    Crash($"No instruction found for {_pc}");
                    break; //Stop the loop if no instruction was found to "crash" the program.
                }

                Log($"<--(Executing {opcode.Name} instruction)-->", ConsoleColor.DarkGreen);
                opcode.Act(this, instruction, _crashHandle);
                Log($"<--(Finished {opcode.Name} instruction)--->", ConsoleColor.DarkGreen);
                Space();
            }

            Space();

            DumpRegisters();
            Space();
            DumpFlags();
            Space();    
            DumpMemory();
        }

        /// <summary>
        /// Simulates crashing the program
        /// </summary>
        private void Crash(string errorMessage)
        {
            LogError($"The program has crashed: {errorMessage}");
            _forceQuit = true;
        }

        private void DumpRegisters()
        {
            Log("Dumping registers:", ConsoleColor.White);
            for (byte i = 0; i < Enum.GetValues(typeof(Register)).Length; i++)
            {
                var value = _registers.GetRegisterValue(i);
                Log($"Register {Enum.GetName(typeof(Register), i)} holds = {value}", value == 0 ? ConsoleColor.DarkYellow : ConsoleColor.Yellow);
            }
        }
         
        private void DumpFlags()
        {
            Log("Dumping flags:", ConsoleColor.White);
            for (int i = 0; i < Enum.GetValues(typeof(Flags)).Length; i++)
            {
                var flag = (Flags)(1 << i);
                var hasFlag = Registers.FlagsRegister.HasFlag(flag);
                Log($"Flag {flag} is {(hasFlag ? "set" : "not set")}", hasFlag ? ConsoleColor.Yellow : ConsoleColor.DarkYellow);
            }
        }

        private void DumpMemory()
        {
            Log("Dumping memory:", ConsoleColor.White);
            for (uint i = 0; i < _memory.HeapMemorySize; i++)
            {
                var value = _memory.GetFromMemory(i);
                Log($"Memory address {i} holds = {value}", value == 0 ? ConsoleColor.DarkYellow : ConsoleColor.Yellow);
            }
        }
        #endregion

        #region API's 

        /// <summary>
        /// Sets the program counter to a specific value, if the value is out of bounds it will crash the program
        /// </summary>
        /// <param name="value">The value to set the program counter to.</param>
        public void SetProgramCounter(byte value)
        {
            _pc = value;
            if(_pc > _program.Length)
                Crash("Tried to set the program counter to a value that is out of bounds");

        }

        public void EndProgram()
        {
            Log("Ending the program", ConsoleColor.Blue);
            _forceQuit = true;
        }

        #region Printing/Logging

        /// <summary>
        /// Prints a message to the console without a new line at the end.
        /// </summary>
        /// <param name="message">The message to print.</param>
        /// <remarks>This works regardless if _loggingEnabled is enabled or not.</remarks>
        public void Print(string message) => Console.Write(message);

        /// <summary>
        /// Prints a character to the console without a new line at the end.
        /// </summary>
        /// <param name="message">The character to print.</param>
        /// <remarks>This works regardless if _loggingEnabled is enabled or not.</remarks>
        public void Print (char message) => Console.Write(message);


        /// <summary>
        /// Logs an error message to the console in red color, and resets the color back to white after logging.
        /// </summary>
        /// <param name="errorMessage">The error message to log.</param>
        /// <remarks>This does NOT work if _loggingEnabled is false.</remarks>
        public void LogError(string errorMessage)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(errorMessage);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Writes a message to the console in the specified color if debug mode is enabled.
        /// </summary>
        /// <param name="message">The message to display in the console.</param>
        /// <param name="color">The color to use for the console output. Defaults to green.</param>
        /// <remarks>This does NOT work if _loggingEnabled is false.</remarks>
        public void Log(string message, ConsoleColor color = ConsoleColor.Green)
        {
            if (_loggingEnabled) 
            {
                Console.ForegroundColor = color;

                Console.WriteLine(message);

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Logs an empty line to the console if debug mode is enabled, 
        /// this is used to separate different instructions in the console output for better readability.
        /// </summary>
        public void Space()
        {
            if (_loggingEnabled)
                Console.WriteLine();
        }

        #endregion

        #endregion
    }
}
