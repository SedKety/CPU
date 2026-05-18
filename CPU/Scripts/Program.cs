using System.Reflection;

namespace VirtualCPU
{
    public class Program
    {
        //Entry point
        static void Main(string[] args)
        {
            var program = Programs.SumInputSample;

            OpcodeInstruction[] instructions = Assembly.GetAssembly(typeof(Program)).GetTypes()
                .Where(t => typeof(OpcodeInstruction).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => (OpcodeInstruction)Activator.CreateInstance(t))
                .ToArray();

            VCPU cpu = new VCPU(program, instructions, false);
        }
    }
}
