using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// The jump instruction, this allows you to jump to a different adress.
    /// </summary>
    public class JumpInstruction : OpcodeInstruction
    {
        public string Name => "JMP";

        public bool Accept(byte opcode) => opcode == (byte)OpCode.JMP;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var localPC = vCpu.ProgramCounter;

            localPC++; //Go to the next adress which stores the location to jump to

            var destination = vCpu.Program[localPC]; ///Get the location to jump to represented in offset from 0

            vCpu.Log($"Jumping from {localPC} to: {destination}");

            vCpu.SetProgramCounter(destination);
        }
    }
}
