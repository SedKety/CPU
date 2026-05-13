using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Loads an immediate value into a register. The instruction format is as follows:
    /// LOAD Register Value
    /// </summary>
    public class LoadInstruction : OpcodeInstruction
    {
        public string Name => "LOAD";

        public bool Accept(byte opcode) => opcode == (byte)OpCodes.LOAD;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var register = vCpu.Program[vCpu.ProgramCounter + 1];
            var value = vCpu.Program[vCpu.ProgramCounter + 2];

            vCpu.Log($"Executing LOAD instruction: Loading value {value} into register {register}");

            vCpu.Registers.SetRegisterValue(register, value);

            vCpu.SetProgramCounter((byte)(vCpu.ProgramCounter + 3));
        }
    }
}
