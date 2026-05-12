
namespace VirtualCPU
{
    public interface OpcodeInstruction
    {
        public string Name { get; } 
        public bool Accept(byte opcode);

        public void Act(VCPU vCpu, byte opcode, Action<string> crashHandle);
    }
}