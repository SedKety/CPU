using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{
    public class IncrementInstruction : OpcodeInstruction
    {
        public string Name => "INC";
        public bool Accept(byte opcode) => opcode == (byte)OpCode.INC;

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var pc = vCpu.ProgramCounter;
            var registers = vCpu.Registers;
            vCpu.Log($"Incrementing R{vCpu.Program[pc + 1]} CurValue = {registers.GetRegisterValue(vCpu.Program[pc + 1])}");

            registers.SetRegisterValue(vCpu.Program[pc + 1], (byte)(registers.GetRegisterValue(vCpu.Program[pc + 1]) + 1)); 

            vCpu.Log($"NewValue = {registers.GetRegisterValue(vCpu.Program[pc + 1])}");
            vCpu.SetProgramCounter((byte)(pc + 2));
        }
    }
}
