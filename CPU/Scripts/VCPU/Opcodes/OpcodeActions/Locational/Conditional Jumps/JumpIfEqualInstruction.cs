using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// The JumpIfEqualInstruction class represents the "Jump if Equal" opcode (JE) in the virtual CPU.
    /// The instruction format is as follows:
    /// JE Destination
    /// </summary>
    public class JumpIfEqualInstruction : OpcodeInstruction
    {
        public string Name => "JE";

        public bool Accept(byte opcode) => opcode == (byte)OpCodes.JE;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var isEqual = vCpu.Registers.FlagsRegister.HasFlag(Flags.Zero);
            if(!isEqual)
            {
                vCpu.Log("Not jumping because the values are not equal");
                vCpu.SetProgramCounter((byte)(vCpu.ProgramCounter + 2)); //Go to the next instruction
                return;
            }


            var localPC = vCpu.ProgramCounter;

            localPC++; //Go to the next adress which stores the location to jump to

            var destination = vCpu.Program[localPC]; ///Get the location to jump to represented in offset from 0
            vCpu.Log($"Values are equal, jumping to: {destination}");

            vCpu.SetProgramCounter(destination);
        }
    }
}
