using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU
{
    public enum Flags : Byte
    {
        zero_flag = 0x00,
        signed_flag = 0x01,
        overflow_flag = 0x02,

    }
    internal class Cpu
    {

        private byte[] registers = new byte[16]; // 16 registers, each 1 byte

        private byte[] memory = new byte[256]; // Memory the program has access to, 256 bytes

        private byte[] program = new byte[256]; // For now just 256 considering the program size might differ later on

        private int pc = 0; //Program counter, pretty cool

        private byte[] flags = new byte[Enum.GetValues(typeof(Flags)).Length];

        private bool dbgMode = true;
        private bool programRunning = true;
        //Executes the program
        public void Run(byte[] programArray)
        {
            Console.WriteLine("Executing the program");

            program = programArray;


            while (pc <= program.Length)
            {
                byte instruction = program[pc]; // Current instruction(the byte at the program counter)
                switch (instruction)
                {
                    //Program Opcodes
                    case (byte)OpCodes.END:
                        Console.WriteLine("Reached the end of the program.");
                        pc += program.Length;
                        programRunning = false;
                        break;

                    //Memory Opcodes
                    case (byte)OpCodes.LOAD:
                        Load(); break;

                    //Jump Opcodes
                    case (byte)OpCodes.JMP:
                        Jump(); break;

                    //Arithmetic Opcodes
                    case (byte)OpCodes.ADD:
                        Add(); break;

                    case (byte)OpCodes.CMP:
                        Compare(); break;

                    case (byte)OpCodes.SUB:
                        Subtract(); break;

                    case (byte)OpCodes.INC:
                        Increment(); break;

                    case (byte)OpCodes.DEC:
                        Decrement(); break;

                    default:
                        Console.WriteLine($"Uhh well something went wrong, opcode = {instruction} was not found");
                        break;


                }

            }

            for (int i = 0; i < registers.Length; i++)
            {
                DbgPrint($"Register {i} holds = {registers[i]}");
            }

            for(int i = 0; i < flags.Length; i++)
            {
                DbgPrint($"Flag {Enum.GetName(typeof(Flags), i)} holds = {flags[i]}");
            }
        }

        private void ProgramError(string dbgMess)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(dbgMess);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void DbgPrint(string dbgMess)
        {
            if (dbgMode) 
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                if (programRunning)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine(dbgMess);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        #region OpCodes 

        #region Program-Deciding

        private bool haveJumped = false; //Quick fix to prevent crashing or some undefined nasty shit
        private void Jump()
        {
            if (!haveJumped)
            {
                pc = program[pc + 1];
                haveJumped = true;
                return;
            }
            pc++;
            pc++;
        }

        #endregion

        #region Memory-Opcodes
        private void Load()
        {
            byte register = program[++pc];
            byte val = program[++pc];
            pc++;

            if (registers.Length >= register)
            {
                registers[register] = val;
                DbgPrint($"Loaded: {val},  into : R{register}");

            }
            else
            {
                ProgramError("Tried to load in an non existent register");
            }

        }
        #endregion

        #region Arithmetic Operations
        private void Add()
        {

        }

        private void Compare()
        {

        }

        private void Subtract()
        {
            byte inputReg1 = program[++pc];
            byte inputReg2 = program[++pc];
            pc++;

            if (registers.Length >= inputReg1 && registers.Length >= inputReg2)
            {
                DbgPrint($"Subtracting, {registers[inputReg2]} from {registers[inputReg1]}");
                registers[inputReg1] = (byte)(registers[inputReg1] - registers[inputReg2]);

                sbyte sum = (sbyte)(registers[inputReg1]); 

                if (sum == 0)
                {
                    flags[(byte)(Flags.zero_flag)] = 1;
                }
                if (sum < 0)
                {
                    flags[(byte)(Flags.signed_flag)] = 1;
                    flags[(byte)(Flags.overflow_flag)] = 1;
                }
            }
            else
            {
                ProgramError("Tried to subtract with an non existent register");
                return;
            }

        }

        private void Increment()
        {
            DbgPrint($"Incrementing R{program[pc + 1]} CurValue = {registers[program[pc + 1]]}");
            registers[program[++pc]]++; //Holy fuck this syntax is cursed
            pc++;
        }
        private void Decrement()
        {
            DbgPrint($"Decrementing R{program[pc + 1]} CurValue = {registers[program[pc + 1]]}");
            registers[program[++pc]]--; //Holy fuck this syntax is cursed
            pc++;
        }
        #endregion

        #endregion
    }
}
