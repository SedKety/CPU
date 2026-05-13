using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Instruction to print the value of a register or a block of memory to the console.
    /// The instruction format is as follows:
    /// PRT SourceType(Register = 0, Memory = 1, Immediate value = 2) Source
    /// </summary>
    public class PrintInstruction : OpcodeInstruction
    {
        public string Name => "PRT";
        public bool Accept(byte opcode) => opcode == (byte)OpCodes.PRT;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            // Start at ProgramCounter + 1 to jump past the opcode itself
            var internalPc = vCpu.ProgramCounter + 1; 
            var sourceType = vCpu.Program[internalPc++];
            var source = vCpu.Program[internalPc++];

            if (sourceType == 0) // Register
            {
                var value = vCpu.Registers.GetRegisterValue(source);
                vCpu.Print((char)value);
            }
            else if (sourceType == 1) // Memory
            {
                var curChar = (char)vCpu.Memory.GetFromMemory(source++);
                while(curChar != '\0')
                {
                    vCpu.Print((char)curChar);
                    curChar = (char)vCpu.Memory.GetFromMemory(source++);
                }
            }
            else if (sourceType == 2) // Immediate value
            {
                // The first character is already read into the 'source' variable
                var curChar = source;
                while (curChar != '\0')
                {
                    vCpu.Print((char)curChar);
                    // Read next character and increment PC
                    curChar = vCpu.Program[internalPc++];
                }
            }
            else
            {
                crashHandle($"Invalid source type for PRT instruction: {sourceType}");
                return;
            }

            // Update the program counter when the instruction is finished
            vCpu.SetProgramCounter((byte)internalPc);
        }
    }
}
