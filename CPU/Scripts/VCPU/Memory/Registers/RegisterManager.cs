using System;
using System.Collections.Generic;
namespace VirtualCPU
{
    public enum Register : byte
    {
        R0 = 0,
        R1 = 1,
        R2 = 2,
        R3 = 3,
        R4 = 4,
        R5 = 5,
        R6 = 6,
        R7 = 7,
        R8 = 8,
        R9 = 9,
        R10 = 10,
        R11 = 11,
        R12 = 12,
    }

    /// <summary>
    /// Class that represents the registers of the virtual CPU, it provides methods to get and set the values of the registers,
    /// </summary>
    /// <remarks>There currently are 12 general-purpose registers.</remarks>
    public class RegisterManager
    {
        private Dictionary<Register, byte> _registerValues;
        private Flags _flagsRegister;

        private VCPU _vCpu;
        private Action<string> _crashHandle;

        public Flags FlagsRegister { get => _flagsRegister; }

        #region Methods

        #region Public Methods  
        public RegisterManager(VCPU vCpu, Action<string> crashHandle = null)
        {
            _vCpu = vCpu;
            _crashHandle = crashHandle;
            InitializeRegisters();
        }

        public void SetFlagsRegister(Flags flags)
        {
            _flagsRegister = flags;
        }

        /// <summary>
        /// Gets the value of a register, and outputs it in the value parameter, 
        /// if the register does not exist it will crash the program
        /// </summary>
        /// <param name="register">The register number to get the value from</param>
        public byte GetRegisterValue(byte register)
        {
            byte value = 0;

            if (register >= Enum.GetValues(typeof(Register)).Length)
            {
                _crashHandle?.Invoke("Tried to access a non-existent register");
                return value;
            }

            Register register1 = (Register)register;
            value = GetRegisterValue(register1);

            return value;
        }

        /// <summary>
        /// Sets the specified register to the given value. 
        /// </summary>
        /// <param name="register">The index of the register to set.</param>
        /// <param name="value">The value to assign to the register.</param>
        public void SetRegisterValue(byte register, byte value)
        {
            if (register >= Enum.GetValues(typeof(Register)).Length)
            {
                _crashHandle?.Invoke("Tried to access a non-existent register");
                return;
            }

            Register register1 = (Register)register;
            SetRegisterValue(register1, value);
        }

        /// <summary>
        /// Updates the flags register based on the result of an addition or subtraction operation between two byte values.
        /// </summary>
        /// <param name="lhs">The left-hand side operand.</param>
        /// <param name="rhs">The right-hand side operand.</param>
        /// <param name="isSubtraction">Whether the operation is subtraction (or comparison).</param>
        public void UpdateFlags(byte lhs, byte rhs, bool isSubtraction = false)
        {
            UpdateCarryFlag(lhs, rhs, _vCpu, isSubtraction);
            UpdateOverflowFlag(lhs, rhs, _vCpu, isSubtraction);
            UpdateZeroFlag(lhs, rhs, _vCpu, isSubtraction);
            UpdateSignedFlag(lhs, rhs, _vCpu, isSubtraction); 
        }


        #endregion

        #region Private Methods
        private void InitializeRegisters()
        {
            _registerValues = new Dictionary<Register, byte>();
            foreach (Register reg in Enum.GetValues(typeof(Register)))
            {
                _registerValues[reg] = 0; // Initialize all registers to 0
            }
        }


        /// <summary>
        /// Sets the value of the specified register.
        /// </summary>
        /// <param name="register">The register to set the value of.</param>
        /// <param name="value">The value to set the register to.</param>
        private void SetRegisterValue(Register register, byte value)
        {
            _registerValues[register] = value;
        }

        /// <summary>
        /// Gets the value of the specified register.
        /// </summary>
        /// <param name="register">The register to get the value of.</param>
        /// <returns>The value of the specified register.</returns>
        private byte GetRegisterValue(Register register)
        {
            return _registerValues[register];
        }

        #region Flags
        private void UpdateCarryFlag(byte lhs, byte rhs, VCPU vCpu, bool isSubtraction = false)
        {
            // For subtraction/CMP, Carry Flag acts as a Borrow Flag. It is set if lhs < rhs.
            bool carry = isSubtraction ? lhs < rhs : lhs + rhs > byte.MaxValue;
            if (carry)
                vCpu.Registers.SetFlagsRegister((Flags)(vCpu.Registers.FlagsRegister | Flags.Carry));
            else
                vCpu.Registers.SetFlagsRegister((Flags)(vCpu.Registers.FlagsRegister & ~Flags.Carry));
        }

        private void UpdateZeroFlag(byte lhs, byte rhs, VCPU vCpu, bool isSubtraction = false)
        {
            int result = isSubtraction ? lhs - rhs : lhs + rhs;
            if ((byte)result == 0)
                vCpu.Registers.SetFlagsRegister((Flags)(vCpu.Registers.FlagsRegister | Flags.Zero));
            else
                vCpu.Registers.SetFlagsRegister((Flags)(vCpu.Registers.FlagsRegister & ~Flags.Zero));
        }

        private void UpdateOverflowFlag(byte lhs, byte rhs, VCPU vCpu, bool isSubtraction = false)
        {
            int result = isSubtraction ? lhs - rhs : lhs + rhs;
            // For addition: Overflow occurs if the sign of the result is different from the signs of both operands when they are the same.
            // For subtraction: Overflow occurs if signs of operands are different and sign of result matches sign of subtrahend.
            bool overflow = isSubtraction 
                ? ((lhs ^ rhs) & 0x80) != 0 && ((lhs ^ result) & 0x80) != 0
                : ((lhs ^ rhs) & 0x80) == 0 && ((lhs ^ result) & 0x80) != 0;

            if (overflow)
                vCpu.Registers.SetFlagsRegister((Flags)(vCpu.Registers.FlagsRegister | Flags.Overflow));
            else
                vCpu.Registers.SetFlagsRegister((Flags)(vCpu.Registers.FlagsRegister & ~Flags.Overflow));
        }

        private void UpdateSignedFlag(byte lhs, byte rhs, VCPU vCpu, bool isSubtraction = false)
        {
            int result = isSubtraction ? lhs - rhs : lhs + rhs;
            
            // If the highest bit (0x80) is 1, the number is negative in two's complement
            if (((byte)result & 0x80) != 0)
                vCpu.Registers.SetFlagsRegister((Flags)(vCpu.Registers.FlagsRegister | Flags.Signed));
            else
                vCpu.Registers.SetFlagsRegister((Flags)(vCpu.Registers.FlagsRegister & ~Flags.Signed));
        }
        #endregion

        #endregion

        #endregion
    }
}
