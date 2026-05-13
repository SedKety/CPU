using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    /// <summary>
    /// Represents an instruction that decrements the value of a specified register in a virtual CPU.
    /// The instruction format is as follows:
    /// DEC Register
    /// </summary>
    public class DecrementInstruction : OpcodeInstruction
    {
        public string Name => "DEC";
        public bool Accept(byte opcode) => opcode == (byte)OpCodes.DEC;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var pc = vCpu.ProgramCounter;
            var registers = vCpu.Registers;
            vCpu.Log($"Decrementing R{vCpu.Program[pc + 1]} CurValue = {registers.GetRegisterValue(vCpu.Program[pc + 1])}");

            registers.SetRegisterValue(vCpu.Program[pc + 1], (byte)(registers.GetRegisterValue(vCpu.Program[pc + 1]) - 1));

            vCpu.Log($"NewValue = {registers.GetRegisterValue(vCpu.Program[pc + 1])}");
            vCpu.SetProgramCounter((byte)(pc + 2));
        }
    }
}
