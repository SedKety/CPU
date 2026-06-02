using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU.Opcodes
{

    /// <summary>
    /// Move instruction, moves a value from a source to a destination, 
    /// the source and destination can be either a register or a memory address, but not both memory addresses.
    /// Written as: MOV source isSourceRegister destination isDestinationRegister
    /// </summary>
    public class MoveInstruction : OpcodeInstruction
    {
        public string Name => "MOV";
        public bool Accept(byte opcode) => opcode == (byte)OpCodes.MOV;
        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle)
        {
            var registers = vCpu.Registers;

            var sourceOperand = vCpu.Program[vCpu.ProgramCounter + 1];
            bool sourceIsRegister = vCpu.Program[vCpu.ProgramCounter + 2] != 0; // 0 for memory, 1 for register

            vCpu.Log($"Executing MOV instruction with source operand {sourceOperand} (IsRegister: {sourceIsRegister})");
            var sourceValue = sourceIsRegister ? registers.GetRegisterValue(sourceOperand) : vCpu.Memory.GetFromMemory(sourceOperand);

            var destination = vCpu.Program[vCpu.ProgramCounter + 3];
            bool destinationIsRegister = vCpu.Program[vCpu.ProgramCounter + 4] != 0; // 0 for memory, 1 for register

            vCpu.Log($"Destination operand {destination} (IsRegister: {destinationIsRegister})");


            if (sourceIsRegister && destinationIsRegister)
                MoveToRegister(vCpu, sourceValue, destination);
            else if (sourceIsRegister && !destinationIsRegister)
                MoveToMemory(vCpu, sourceValue, destination);
            else if (!sourceIsRegister && destinationIsRegister)
                MoveToRegister(vCpu, sourceValue, destination);
            else
                crashHandle("Invalid MOV instruction: both sourceValue and destination cannot be memory.");
        }

        private void MoveToRegister(VCPU vCpu, byte source, byte destination)
        {
            vCpu.Log($"Moving value {source} to register R{destination}");
            vCpu.Registers.SetRegisterValue(destination, source);
            vCpu.SetProgramCounter((byte)(vCpu.ProgramCounter + 5));
        }

        private void MoveToMemory(VCPU vCpu, byte source, byte destination)
        {
            vCpu.Log($"Moving value {source} to memory address {destination}");
            vCpu.Memory.WriteToMemory(destination, source);
            vCpu.SetProgramCounter((byte)(vCpu.ProgramCounter + 5));
        }
    }
}