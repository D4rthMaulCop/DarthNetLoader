using System;
using System.Text;

namespace DarthLoaderHelper
{
    class DarthLoaderHelper
    {
        public static string xorKey = "";

        public static string XorString(string stringInput, string key)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < stringInput.Length; i++)
                sb.Append((char)(stringInput[i] ^ key[(i % key.Length)]));
            String result = sb.ToString();
            return result;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
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

        public static string ConvertStringToBytes(string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            byte[] xoredBytes = XorBytes(bytes, xorKey);
            return BitConverter.ToString(xoredBytes).Replace("-", "");
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
                    uint decval = Convert.ToUInt32(hs, 16);
                    char character = Convert.ToChar(decval);
                    ascii += character;
                }
                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return string.Empty;
        }

        static void Main(string[] args)
        {
            try
            {
                xorKey = args[0];

                // strings to encrypt
                string etwBypass = "etwbypass";
                string amsi = "amsibypass";
                string ntdDll = "ntdll.dll";
                string etw = "EtwEventWrite";
                string amsiDll = "amsi.dll";
                string amsiBuffer = "AmsiScanBuffer";

                Console.WriteLine($"[+] Encrypting with key: {xorKey}");
                Console.WriteLine($"[+] Encrypted string 'etwbypass': {ConvertStringToBytes(etwBypass)}");
                Console.WriteLine($"[+] Encrypted string 'amsibypass': {ConvertStringToBytes(amsi)}");
                Console.WriteLine($"[+] Encrypted string 'ntdll.dll': {ConvertStringToBytes(ntdDll)}");
                Console.WriteLine($"[+] Encrypted string 'EtwEventWrite': {ConvertStringToBytes(etw)}");
                Console.WriteLine($"[+] Encrypted string 'amsi.dll': {ConvertStringToBytes(amsiDll)}");
                Console.WriteLine($"[+] Encrypted string 'AmsiScanBuffer': {ConvertStringToBytes(amsiBuffer)}");

                // Converting back
                //string hexASCIIVar = ConvertHex(encryptedPayload);
                //Console.WriteLine($"[+] Hex to ASCII value var: {hexASCIIVar}");
                //string decrytedPayload = XorString(hexASCIIVar, xorKey);
                //Console.WriteLine($"[+] Decrypted payload: {decrytedPayload}");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("[!] Provide an Xor key string.");
            }
        }
    }
}
