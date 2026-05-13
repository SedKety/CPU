using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Instruction to add the values of two registers and store the result in the destination register.
    /// Written in the format: ADD DestinationRegister, SourceRegister1
    /// </summary>
    public class AddInstruction : OpcodeInstruction
    {
        public string Name => "ADD";

        public bool Accept(byte opcode)
        {
            return opcode == (byte)OpCodes.ADD;
        }

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var lhs = vCpu.Registers.GetRegisterValue(vCpu.Program[vCpu.ProgramCounter + 1]);
            var rhs = vCpu.Registers.GetRegisterValue(vCpu.Program[vCpu.ProgramCounter + 2]);

            vCpu.Log($"Adding {lhs} and {rhs} from registers {vCpu.Program[vCpu.ProgramCounter + 1]} and {vCpu.Program[vCpu.ProgramCounter + 2]}");

            vCpu.Registers.UpdateFlags(lhs, rhs);

            var result = (byte)(lhs + rhs);
            vCpu.Registers.SetRegisterValue(vCpu.Program[vCpu.ProgramCounter + 1], result);

            vCpu.Log($"Result of addition: {result} stored in register {vCpu.Program[vCpu.ProgramCounter + 1]}");
            vCpu.SetProgramCounter((byte)(vCpu.ProgramCounter + 3));
        }
    }
}
