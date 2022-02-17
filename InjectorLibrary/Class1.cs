using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RGiesecke.DllExport;

namespace InjectorLibrary
{
    public class Class1
    {

        [DllExport("runcmd", CallingConvention = CallingConvention.StdCall)] 
        public static void runcmd()
        {
            Process ExternalProcess = new Process();
            ExternalProcess.StartInfo.FileName = "cmd.exe";
            ExternalProcess.Start();
        }


        [DllExport("runpowershell", CallingConvention = CallingConvention.StdCall)]
        public static void runpowershell()
        {
            Process ExternalProcess = new Process();
            ExternalProcess.StartInfo.FileName = "powershell.exe";
            ExternalProcess.Start();
        }

        [DllExport("executeA", CallingConvention = CallingConvention.StdCall)]
        public static void executeA([MarshalAs(UnmanagedType.LPStr)] string command)
        {

            Process.Start(command);
        }

        [DllExport("executeW", CallingConvention = CallingConvention.StdCall)]
        public static void executeW([MarshalAs(UnmanagedType.LPWStr)] string command)
        {

            Process.Start(command);
        }

        [DllExport("geturlA", CallingConvention = CallingConvention.StdCall)]
        public static void geturlA([MarshalAs(UnmanagedType.LPStr)] string url="https://www.google.com/")
        {
            WebClient client = new WebClient();
            string content = client.DownloadString(url);
            Console.WriteLine(content);
        }

        [DllExport("geturlW", CallingConvention = CallingConvention.StdCall)]
        public static void geturlW([MarshalAs(UnmanagedType.LPWStr)] string url = "https://www.google.com/")
        {
            WebClient client = new WebClient();
            string content = client.DownloadString(url);
            Console.WriteLine(content);
        }



        [DllExport("getUrlMsg", CallingConvention = CallingConvention.StdCall)]
        public static void getUrlMsg()
        {
            WebClient client = new WebClient();
            string content = client.DownloadString("https://www.google.com/");
            if (content.Length > 100)
            {
                content = content.Substring(0, 100);
            }
            MessageBox.Show(content);
        }



        [DllExport("getUrlFile", CallingConvention = CallingConvention.StdCall)]
        public static void getUrlFile()
        {
            string outFile = Path.Combine(Path.GetTempPath(), "urlcontent.txt");
            if (!(File.Exists(outFile))) {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient client = new WebClient();
                string content = client.DownloadString("https://www.google.com/");
                Console.WriteLine("Writing content to {0}", outFile);
                File.WriteAllText(outFile, content);
            } else
            {
                Console.WriteLine("File {0} exists, skipping", outFile);
            }

        }

        [DllExport("messagebox", CallingConvention = CallingConvention.StdCall)] 
        public static void messagebox()
        {
            MessageBox.Show("Hello from InjectorDlls messagebox function!");
        }

        [DllExport("consolemessage", CallingConvention = CallingConvention.StdCall)]
        public static void consolemessage()
        {
            Console.WriteLine("Hello from InjectorDlls consolemessage function!");
        }


        // See the following reference for info on calling from rundll32
        //https://web.archive.org/web/20150317213051/https://support.microsoft.com/en-us/kb/164787
        [DllExport("rundllCmdA", CallingConvention = CallingConvention.StdCall)] 
        public static void rundllCmdA(int hwnd, int hinst, [MarshalAs(UnmanagedType.LPStr)] string lpszCmdLine, int nCmdShow)
        {
            Process.Start(lpszCmdLine);
        }

        [DllExport("rundllCmdW", CallingConvention = CallingConvention.StdCall)]
        public static void rundllCmdW(int hwnd, int hinst, [MarshalAs(UnmanagedType.LPWStr)] string lpszCmdLine, int nCmdShow)
        {
            Process.Start(lpszCmdLine);
        }

        [DllExport("rundllUrlA", CallingConvention = CallingConvention.StdCall)]
        public static void rundllUrlA(int hwnd, int hinst, [MarshalAs(UnmanagedType.LPStr)] string url, int nCmdShow)
        {
            string[] urlParts = url.Split('/');
            string filename = urlParts[urlParts.Length - 1];
            if (filename.Length < 1 || urlParts.Length < 4)
            {
                filename = "index.html";
            }

            string outFile = Path.Combine(Path.GetTempPath(), filename);

            try
            {
                WebClient client = new WebClient();
                string content = client.DownloadString(url);
                File.WriteAllText(outFile, content);
            }
            catch (WebException) // Theres probably a neater way to handle this, but for now...
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient client = new WebClient();
                string content = client.DownloadString(url);
                File.WriteAllText(outFile, content);
            }

        }

        [DllExport("rundllUrlW", CallingConvention = CallingConvention.StdCall)]
        public static void rundllUrlW(int hwnd, int hinst, [MarshalAs(UnmanagedType.LPWStr)] string url, int nCmdShow)
        {
            string[] urlParts = url.Split('/');
            string filename = urlParts[urlParts.Length - 1];
            if (filename.Length < 1 || urlParts.Length < 4)
            {
                filename = "index.html";
            }

            string outFile = Path.Combine(Path.GetTempPath(), filename);
            
            try
            {
                WebClient client = new WebClient();
                string content = client.DownloadString(url);
                File.WriteAllText(outFile, content);
            } catch (WebException) 
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient client = new WebClient();
                string content = client.DownloadString(url);
                File.WriteAllText(outFile, content);
            }

        }

    }
}
