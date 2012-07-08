using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace Minecraft_Classic_Boom
{
    class Program
    {
        static string SessionToken = "";
        static int counter = 0;
        static bool Console_In_Use = false;
        static void Main(string[] args)
        {
            Console.Write("Please Inpit your Session Token: ");
            SessionToken = Console.ReadLine();

            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("http://www.minecraft.net/classic/list");
            WebReq.Method = "GET";
            WebReq.Headers["Cookie"] = "PLAY_SESSION=" + SessionToken;
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);

            string[] lines = _Answer.ReadToEnd().Split('\n');

            int index = 0;
            string[] serverURLs = new string[lines.Length];
            foreach (string line in lines)
            {
                if(line.Contains("<a href=\"/classic/play/"))
                {
                    serverURLs[index] = line.Split('"')[1];
                    index++;
                }
            }
            Console.WriteLine("Grabbed {0} Servers", index);
            Console.WriteLine("Starting Connection Round");

            int blah = 0;
            foreach (string server in serverURLs)
            {
                if (server != null)
                {
                    string swag = SessionToken + ":" + server;
                    string[] connect = swag.Split(':');
                    Thread[] threads = new Thread[1];
                    threads[0] = new System.Threading.Thread(() => MCConnecter(connect[0], connect[1]));
                    threads[0].Start(); 
                    blah++;
                    Console.Title = ("Connected to " + counter + " Servers or " + (float)blah / (float)index * 100 + "%");
                    Thread.Sleep(250);
                }
            }
            Thread.Sleep(3000);
            Environment.Exit(0);
        }

        private static void MCConnecter(string SessionToken, string sserver)
        {
            int my_id = counter + Environment.TickCount; // Used to Seed the RNG
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create("http://www.minecraft.net" + sserver);
            WebReq.Method = "GET";
            WebReq.Headers["Cookie"] = "PLAY_SESSION=" + SessionToken;
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);

            string[] lines = _Answer.ReadToEnd().Split('\n');
            string IP = "0.0.0.0";
            string port = "0";
            string mpass = "swag";
            string username = "";
            foreach (string line in lines)
            {
                if (line.Contains("<param name=\"server\" value=\"")){
                    IP = line.Split('"')[3];
                }
                if (line.Contains("<param name=\"port\" value=\""))
                {
                    port = line.Split('"')[3];
                }
                if (line.Contains("<param name=\"mppass\" value=\""))
                {
                    mpass = line.Split('"')[3];
                }
                if (line.Contains("<span class=\"logged-in\">"))
                {
                    username = line.Split('>')[1].Split(' ')[3];
                }
            }
            //Console.WriteLine("I'm going to connect to {0} on {1} using {2}", IP, port, mpass);
            byte[] data = new byte[1024];
            string stringData;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IP), Int32.Parse(port));

            Socket server = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ipep);
                server.Send(MakeLoginPayload(username, mpass));
            }
            catch (SocketException e)
            {
                //Console.WriteLine("Unable to connect to server.");
                //Console.WriteLine(e.ToString());
                return;
            }
            int recv = 0;
            try
            {
                recv = server.Receive(data);
                counter++;
            }
            catch
            {
                return;
            }
            while (true)
            {
                try
                {
                    data = new byte[1024];
                    recv = server.Receive(data);
                    AppendData("./" + IP.Replace('.', '_') + ".stream", data,recv);

                        if (data[0] == 0x0d)
                        {
                            if (data[2] == 0x26)
                            {
                            back:
                                stringData = Encoding.ASCII.GetString(data, 0, recv);
                                stringData = stringData.Substring(2, 63);
                                if (!Console_In_Use)
                                {
                                    Console_In_Use = true;
                                    Console.Write("<" + IP + ">");
                                    Console.WriteLine(stringData);
                                    Console_In_Use = false;
                                }
                                else
                                {
                                    Random rer = new Random(my_id);
                                    Thread.Sleep(rer.Next(10, 100));
                                    goto back; // HA, TAKE THAT COMPILER OH GOD WHAT AM I DOING
                                }
                            }
                            //Console.WriteLine("<{0}> {1}}",IP,stringData);
                        }
                }
                catch
                {
                    break;
                }
            }
            Console.WriteLine("Disconnecting from server...");
            counter--;
            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        static byte[] MakeLoginPayload(string username, string mpass)
        {
            byte[] un = Encoding.ASCII.GetBytes(username.PadRight(64, ' '));
            byte[] mp = Encoding.ASCII.GetBytes(mpass.PadRight(64, ' '));
            byte[] pl = new byte[] { 0x00,0x07};
            byte[] fn = new byte[] { 0x00 };
            return Combine(pl, un, mp,fn);
        }

        static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        static void AppendData(string filename, byte[] lotsOfData, int count)
        {
            using (Stream fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (BinaryWriter bw = new BinaryWriter(fileStream))
                {
                    bw.Write(lotsOfData,0,count);
                }
            }
        }

    }
}
