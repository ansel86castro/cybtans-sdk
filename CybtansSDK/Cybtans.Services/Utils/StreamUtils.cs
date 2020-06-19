using System;
using System.Text;
using System.IO;
using System.Globalization;

namespace Cybtans.Services.Utils
{
    public static class StreamUtils
    {
        static UTF8Encoding UTF8 = new UTF8Encoding();

        public static short ReadInt16(Stream stream)
        {
            byte[] buff = new byte[2];

            stream.Read(buff, 0, 2);
            return BitConverter.ToInt16(Convert(buff), 0);
        }

        public static ushort ReadUInt16(Stream stream)
        {
            byte[] buff = new byte[2];

            stream.Read(buff, 0, 2);
            return BitConverter.ToUInt16(Convert(buff), 0);
        }

        public static int ReadInt32(Stream stream)
        {
            byte[] buff = new byte[4];

            stream.Read(buff, 0, 4);
            return BitConverter.ToInt32(Convert(buff), 0);
        }

        public static uint ReadUInt32(Stream stream)
        {
            byte[] buff = new byte[4];

            stream.Read(buff, 0, 4);
            return BitConverter.ToUInt32(Convert(buff), 0);
        }

        public static long ReadInt64(Stream stream)
        {
            byte[] buff = new byte[8];

            stream.Read(buff, 0, 8);
            return BitConverter.ToInt64(Convert(buff), 0);
        }

        public static ulong ReadUInt64(Stream stream)
        {
            byte[] buff = new byte[8];

            stream.Read(buff, 0, 8);
            return BitConverter.ToUInt64(Convert(buff), 0);
        }

        public static byte[] ReadBinary32(Stream stream)
        {
            uint len = ReadUInt32(stream);
            byte[] buff = new byte[len];

            stream.Read(buff, 0, (int)len);
            if (len > int.MaxValue)
            {
                stream.Read(buff, int.MaxValue, (int)(len - int.MaxValue));
            }
            return buff;
        }

        public static byte[] ReadBinary16(Stream stream)
        {
            ushort len = ReadUInt16(stream);
            byte[] buff = new byte[len];

            stream.Read(buff, 0, len);
            return buff;
        }

        public static byte ReadByte(Stream stream)
        {
            return (byte)stream.ReadByte();
        }

        public static double ReadDouble(Stream stream)
        {
            // TODO: create global
            byte[] buff = new byte[8];

            stream.Read(buff, 0, 8);
            return BitConverter.ToDouble(Convert(buff), 0);
        }

        public static string ReadHexBinaryFixed(Stream stream, int len)
        {
            byte[] buff = new byte[len];

            stream.Read(buff, 0, len);

            return ByteArrayToHex(buff);
        }

        public static string ReadHexBinary16(Stream stream)
        {
            int len = ReadUInt16(stream);
            byte[] buff = new byte[len];

            stream.Read(buff, 0, len);

            return ByteArrayToHex(buff);
        }

        public static string ReadString16(Stream stream)
        {
            int len = ReadUInt16(stream);
            byte[] buff = new byte[len];

            stream.Read(buff, 0, len);

            return Encoding.UTF8.GetString(buff);
        }

        public static void WriteBytes(Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteDouble(Stream stream, double d)
        {
            byte[] bytes = Convert(BitConverter.GetBytes(d));

            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteInt32(Stream stream, int i)
        {
            byte[] bytes = Convert(BitConverter.GetBytes(i));

            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteUInt32(Stream stream, uint i)
        {
            byte[] bytes = Convert(BitConverter.GetBytes(i));

            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteUInt64(Stream stream, ulong i)
        {
            byte[] bytes = Convert(BitConverter.GetBytes(i));

            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteInt16(Stream stream, short s)
        {
            byte[] bytes = Convert(BitConverter.GetBytes(s));

            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteUInt16(Stream stream, ushort s)
        {
            byte[] bytes = Convert(BitConverter.GetBytes(s));

            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteInt64(Stream stream, long i)
        {
            byte[] bytes = Convert(BitConverter.GetBytes(i));

            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteBinary16(Stream stream, byte[] bytes)
        {
            WriteUInt16(stream, (ushort)bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteString32(Stream stream, string s)
        {
            byte[] bytes = UTF8.GetBytes(s);

            WriteUInt32(stream, (uint)bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void WriteString16(Stream stream, string s)
        {
            if (s == null)
            {
                WriteUInt16(stream, 0);
            }
            else
            {
                byte[] bytes = UTF8.GetBytes(s);

                WriteUInt16(stream, (ushort)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void WriteString8(Stream stream, string s)
        {
            if (s == null)
            {
                stream.WriteByte(0);
            }
            else
            {
                byte[] bytes = UTF8.GetBytes(s);

                stream.WriteByte((byte)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void WriteBinary32(Stream stream, byte[] bytes)
        {
            if (bytes == null)
            {
                WriteUInt32(stream, 0);
            }
            else
            {
                WriteUInt32(stream, (uint)bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void WriteHexBinaryFixed(Stream stream, string hex, int len)
        {
            if (hex == null)
            {
                for (int i = 0; i < len; i++)
                {
                    stream.WriteByte(0);
                }
            }
            else
            {
                int realLen = hex.Length / 2;

                if (realLen != len)
                {
                    throw new Exception();
                }

                stream.Write(HexToByteArray(hex), 0, len);
            }
        }

        public static void WriteHexBinary16(Stream stream, string hex)
        {
            if (hex == null)
            {
                stream.WriteByte(0);
            }
            else
            {
                WriteBinary16(stream, HexToByteArray(hex));
            }
        }

        public static byte[] HexToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new Exception();
            }

            byte[] HexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }

        public static string ByteArrayToHex(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();

            foreach (byte b in bytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }
            return builder.ToString();
        }

        public static byte[] Convert(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
    }
}
