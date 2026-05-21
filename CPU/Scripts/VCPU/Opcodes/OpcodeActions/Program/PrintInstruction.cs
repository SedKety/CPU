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
    /// PRT OutputType SourceType
    /// </summary>
    public class PrintInstruction : OpcodeInstruction
    {
        public string Name => "PRT";
        public bool Accept(byte opcode) => opcode == (byte)OpCodes.PRT;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            // Start at ProgramCounter + 1 to jump past the opcode itself
            var internalPc = vCpu.ProgramCounter + 1; 
            var outputType = (OutputType)vCpu.Program[internalPc++];
            var sourceType = (SourceType)vCpu.Program[internalPc++];
            var source = vCpu.Program[internalPc++];

            //Defines a local function to print the value based on the output type
            Action<byte> printValue = (val) =>
            {
                if (outputType == OutputType.Hex)
                    vCpu.Print($"0x{val:X2} ");
                else if (outputType == OutputType.Decimal)
                    vCpu.Print(val.ToString() + " ");
                else if (outputType == OutputType.Character)
                    vCpu.Print((char)val);
                else
                    vCpu.Print((char)val);
            };

            if (sourceType == SourceType.Register) 
            {
                var value = vCpu.Registers.GetRegisterValue(source);
                printValue((byte)value);
            }
            else if (sourceType == SourceType.Memory) 
            {
                var curByte = vCpu.Memory.GetFromMemory(source++);
                if (outputType == OutputType.String)
                {
                    while(curByte != '\0')
                    {
                        printValue(curByte);
                        curByte = vCpu.Memory.GetFromMemory(source++);
                    }
                }
                else
                {
                    printValue(curByte);
                }
            }
            else if (sourceType == SourceType.ImmediateValue) 
            {
                //The first character is already read into the 'source' variable
                var curByte = source;
                if (outputType == OutputType.String)
                {
                    while (curByte != '\0')
                    {
                        printValue(curByte);
                        //Read next character and increment PC
                        curByte = vCpu.Program[internalPc++];
                    }
                }
                else
                {
                    printValue(curByte);
                }
            }
            else
            {
                crashHandle($"Invalid source type for PRT instruction: {sourceType}");
                return;
            }

            vCpu.Log($"\nExecuted PRT instruction with output type {outputType}, source type {sourceType} and source {source}", ConsoleColor.Cyan);

            // Update the program counter when the instruction is finished
            vCpu.SetProgramCounter((byte)internalPc);
        }

    }
}
