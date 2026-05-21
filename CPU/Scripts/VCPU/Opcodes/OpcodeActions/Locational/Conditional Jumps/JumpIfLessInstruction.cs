using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// The JumpIfLessInstruction class represents the "Jump if Less" (JL).
    /// </summary>
    public class JumpIfLessInstruction : OpcodeInstruction
    {
        public string Name => "JL";

        public bool Accept(byte opcode) => opcode == (byte)OpCodes.JL;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var isLess = vCpu.Registers.FlagsRegister.HasFlag(Flags.Signed);
            if(!isLess)
            {
                vCpu.Log("Not jumping because the value is not less");
                vCpu.SetProgramCounter((byte)(vCpu.ProgramCounter + 2)); //Go to the next instruction
                return;
            }

            var localPC = vCpu.ProgramCounter;
            localPC++; //Go to the next adress which stores the location to jump to

            var destination = vCpu.Program[localPC];
            vCpu.Log($"Value is less, jumping to: {destination}");

            vCpu.SetProgramCounter(destination);
        }
    }
}