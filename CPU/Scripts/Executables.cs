using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualCPU
{
    public static class Executables
    {
        public static byte[] SampleProgram = new byte[]
        {
            (byte)OpCodes.INC, (byte)Register.R0, // Increment R0
            (byte)OpCodes.MOV, (byte)Register.R0, 1, (byte)Register.R1, 1, // Move R0 to R1
            (byte)OpCodes.ADD, (byte)Register.R0, (byte)Register.R1, // Add R0 and R1, store in R0
            (byte)OpCodes.CMP, (byte)Register.R1, (byte)Register.R0, // Compare R1 and R0
            (byte)OpCodes.END // End of program
        };

        //This will jump if equal, it will allways be equal.

        public static byte[] JumpIfEqualSample = new byte[]
        {
            (byte)OpCodes.LOAD, 5, (byte)Register.R0, // Load the value 5 into R0
            (byte)OpCodes.LOAD, 5, (byte)Register.R1, // Load the value 5 into R1
            (byte)OpCodes.CMP, (byte)Register.R0, (byte)Register.R1, // Compare R0 and R1
            (byte)OpCodes.JE, 15, // Jump to address 15 if R0 and R1 are equal
            (byte)OpCodes.LOAD, 0, (byte)Register.R2, // Load the value 0 into R2 (this will be skipped if the jump is taken)
            (byte)OpCodes.END, // End of program
            (byte)OpCodes.LOAD, 1, (byte)Register.R2, // Load the value 1 into R2 (this will be executed if the jump is taken)
            (byte)OpCodes.END // End of program
        };

        //This has a loop that will increment R0 until it reaches 10, then it will end the program.
        public static byte[] LoopSample = new byte[]
        {
            (byte)OpCodes.LOAD, (byte)Register.R0, 0,  // Load the value 0 into R0
            (byte)OpCodes.LOAD, (byte)Register.R1, 10, // Load the value 10 into R1
            (byte)OpCodes.CMP, (byte)Register.R0, (byte)Register.R1, // Compare R0 and R1
            (byte)OpCodes.JE, 15, // Jump to address 15 if R0 and R1 are equal
            (byte)OpCodes.INC, (byte)Register.R0, // Increment R0
            (byte)OpCodes.JMP, 6, // Jump back to the comparison (Index 6)
            (byte)OpCodes.END // End of program
        };

        //This will print the value of R0, which is 42 (*).
        public static byte[] PrintSample = new byte[]
        {
            (byte)OpCodes.LOAD, (byte)Register.R0, 42, // Load the value 42 into R0
            (byte)OpCodes.PRT, (byte)OutputType.Decimal, 0, (byte)Register.R0, // Print the value of R0
            (byte)OpCodes.END // End of program
        };


        //This will print "Hello, World!" to the console.
        public static byte[] PrintStringSample = new byte[]
        {
            (byte)OpCodes.LOAD, 0, (byte)Register.R0,
            (byte)OpCodes.PRT, (byte)OutputType.String, 2,
            (byte)'H', (byte)'e', (byte)'l', (byte)'l', (byte)'o', (byte)',', (byte)' ',
            (byte)'W', (byte)'o', (byte)'r', (byte)'l', (byte)'d', (byte)'!', 0
        };

        // This program will ask the user to input two numbers, then it will subtract the second number from the first number and print the result.
        public static byte[] SubtractInputExample = new byte[]
        {
            (byte)OpCodes.PRT, (byte)OutputType.Decimal, 0, (byte)Register.R0, // Print the value of R0 (initially 0)
            (byte)OpCodes.PRT, (byte)OutputType.Decimal, 0, (byte)Register.R1, // Print the value of R1 (initially 0)
            (byte)OpCodes.IPT, 0, (byte)Register.R0, // Input a BYTE into R0
            (byte)OpCodes.IPT, 1, (byte)Register.R1, // Input a CHAR into R1
            (byte)OpCodes.SUB, (byte)Register.R0, (byte)Register.R1, // Subtract R1 from R0, store in R0
            (byte)OpCodes.PRT, (byte)OutputType.Decimal, 0, (byte)Register.R0, // Print the result in R0
            (byte)OpCodes.END // End of program
            };

        /// <summary>
        /// Outputs a string to the console, then prompts the user to input a string and outputs that string back to the console.
        /// </summary>
        public static byte[] OutPutInputString = new byte[]
        {
            (byte)OpCodes.IPT, 2, 0, // Input a string into memory starting at address 0
            (byte)OpCodes.PRT, (byte)OutputType.String, 1, 0, // Print the string from memory starting at address 0
            (byte)OpCodes.END // End of program
        };


        public static byte[] OnlyBytes = new byte[] 
        {
            3, 2, 0, 2, 0, 1, 0, 0
        };
    }
}
