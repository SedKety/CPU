using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU
{
    /// <summary>
    /// Provides a non magic way to specify the source type for instructions
    /// that can take multiple source types. e.g., PRT instruction.
    /// </summary>
    public enum SourceType : byte
    {
        Register = 0,
        Memory = 1,
        ImmediateValue = 2
    }
}
