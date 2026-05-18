using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Stores a value from user input into a specified register.
    /// The instruction format is as follows:
    /// IPT InputMode(Byte = 0, Char = 1) RegisterIndex
    /// Then the user is prompted to enter a value, which is then stored in the specified register.
    /// </summary>
    public class InputInstruction : OpcodeInstruction
    {
        public string Name => "INPUT";
        public bool Accept(byte opcode) => opcode == (byte)OpCodes.IPT;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            // Start at ProgramCounter + 1 to jump past the opcode itself
            var internalPc = vCpu.ProgramCounter + 1;
            var inputMode = vCpu.Program[internalPc++];
            var registerIndex = vCpu.Program[internalPc++];

            vCpu.Print($"Enter a value for register R{registerIndex}: ");

            // Get user input and store it in the specified register
            var input = Console.ReadLine();
            
            byte value = 0;
            if (inputMode == 1 && !string.IsNullOrEmpty(input))
            {
                value = (byte)input[0];
            }
            else
            {
                byte.TryParse(input, out value);
            }

            vCpu.Registers.SetRegisterValue(registerIndex, value);


            // Update the program counter when the instruction is finished
            vCpu.SetProgramCounter((byte)internalPc);
        }
    }
}

