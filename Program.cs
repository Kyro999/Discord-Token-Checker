using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Security;
using System.IO;
using System.Security.Principal;
using Console = Colorful.Console;
namespace Discord_Token_Checker
{
    internal class Program
    {
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);
        static void Main(string[] args)
        {
            home:
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                Console.Clear();
                Console.Title = $"Discord Token Checker | Made By Kyro#0102";
                MessageBox((IntPtr)0, "This program wasn't ran as administrator, this will cause the program to run into error most of the time, please re-start the program and run as administrator.", "Discord Token Checker", 0);
                Console.WriteLine("Please run this program as admin.");
                Console.ReadLine();
            }
            else
            {
                Console.Title = $"Discord Token Checker | Made By Kyro#0102";
            }
            string url = "https://discordapp.com/api/v9/users/@me/guilds";
            Console.Clear();
            Console.WriteAscii("Token Checker", System.Drawing.Color.Purple);
            Console.Write("[?] Enter Token: ", System.Drawing.Color.Blue);
            string token = Colorful.Console.ReadLine();
            Console.Clear();
            Console.WriteAscii("Token Checker", System.Drawing.Color.Purple);
            Console.WriteLine("[!] Checking Token...", System.Drawing.Color.Blue);
            Thread.Sleep(3000);
            WebClient wb = new WebClient();
            try
            {
                using (FileStream fs = File.Create("valid_tokens.txt"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes($"{token}");
                    fs.Write(info, 0, info.Length);
                    File.Delete("invalid_tokens.txt");
                }
                Colorful.Console.Clear();
                wb.Headers.Add(HttpRequestHeader.Authorization, token);
                wb.DownloadString(url);
                int feq = 400;
                int time = 500;
                Console.Beep(time, feq);
                Console.WriteAscii("Token Checker", System.Drawing.Color.Purple);
                Console.WriteLine($"[!] Token is valid!", System.Drawing.Color.Green);
                Console.ReadKey();
                goto home;
            }
            catch (WebException e)
            {
                using (FileStream fs = File.Create("invalid_tokens.txt"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes($"{token}");
                    fs.Write(info, 0, info.Length);
                    File.Delete("valid_tokens.txt");
                }
                HttpWebResponse response = (HttpWebResponse)e.Response;
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Console.Clear();
                    Console.WriteAscii("Token Checker", System.Drawing.Color.Purple);
                    Colorful.Console.WriteLine($"[ERROR] The token is invalid!\n\n[!] Restarting...", System.Drawing.Color.Red);
                    Thread.Sleep(4000);
                    goto home;
                }
                else
                {
                    Console.Clear();
                    Console.WriteAscii("Token Checker", System.Drawing.Color.Purple);
                    Colorful.Console.WriteLine("[ERROR] You are being rate limited!\n\n[!] Restarting...", System.Drawing.Color.Red);
                    Thread.Sleep(4000);
                    goto home;
                }
            }
        }
    }
}
