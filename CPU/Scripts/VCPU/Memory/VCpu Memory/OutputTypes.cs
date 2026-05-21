using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU
{
    /// <summary>
    /// Provides a non-magic way to specify the output type for instructions that print data.
    /// </summary>
    public enum OutputType : byte
    {
        String = 0,
        Hex = 1,
        Decimal = 2,
        Character = 3
    }
}
