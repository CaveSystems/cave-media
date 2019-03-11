using System.Runtime.InteropServices;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// CDG data packet.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 24)]
    public struct CdgPacket
    {
        const byte CDG_COMMAND_MASK = 0x3F;
        const byte CDG_COMMAND_VALUE = 0x09;

        byte Command;

        byte InstructionCode;

        /// <summary>Unused.</summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] ParityQ;

        /// <summary>The data.</summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Data;

        /// <summary>Unused.</summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] ParityP;

        /// <summary>Gets a value indicating whether this instance is CDG packet.</summary>
        /// <value>
        /// <c>true</c> if this instance is CDG packet; otherwise, <c>false</c>.
        /// </value>
        public bool IsCdgPacket
        {
            get { return (Command & CDG_COMMAND_MASK) == CDG_COMMAND_VALUE; }
        }

        /// <summary>Gets the instruction.</summary>
        /// <value>The instruction.</value>
        public CdgInstruction Instruction
        {
            get
            {
                return IsCdgPacket ? (CdgInstruction)(InstructionCode & CDG_COMMAND_MASK) : CdgInstruction.Unknown;
            }
        }
    }
}