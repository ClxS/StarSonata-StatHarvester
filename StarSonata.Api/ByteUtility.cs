namespace StarSonata.Api
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class ByteUtility
    {
        public static string ByteArrayToAsciiString(ReadOnlySpan<byte> data)
        {
            return Encoding.ASCII.GetString(data);
        }

        public static string ByteArrayToHexString(ReadOnlySpan<byte> ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public static bool GetBoolean(ReadOnlySpan<byte> data, ref int offset)
        {
            var output = MemoryMarshal.Read<bool>(data.Slice(offset));
            offset += sizeof(bool);
            return output;
        }

        public static byte GetByte(ReadOnlySpan<byte> data, ref int offset)
        {
            offset++;
            return data[offset - 1];
        }

        public static short GetShort(ReadOnlySpan<byte> data, ref int offset)
        {
            var output = MemoryMarshal.Read<short>(data.Slice(offset));
            offset += sizeof(short);
            return output;
        }

        public static int GetInt(ReadOnlySpan<byte> data, ref int offset)
        {
            var output = MemoryMarshal.Read<int>(data.Slice(offset));
            offset += sizeof(int);
            return output;
        }

        public static long GetLong(ReadOnlySpan<byte> data, ref int offset)
        {
            var output = MemoryMarshal.Read<long>(data);
            offset += sizeof(long);
            return output;
        }

        public static string GetString(ReadOnlySpan<byte> data, ref int offset)
        {
            var baseOffset = offset;
            while (offset < data.Length && data[offset] != 0)
            {
                offset++;
            }

            offset++;
            return Encoding.ASCII.GetString(data.Slice(baseOffset, offset - baseOffset - 1));
        }
    }
}
