using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ConsoleApplication1
{
    class Program
    {
        static int levelID = 0;
        static void Main(string[] args)
        {
            string[] filePaths = Directory.GetFiles(@".\","*.stream");
            int tracker = 0;
            foreach (string file in filePaths)
            {
                levelID++;
                Console.WriteLine(file);
                try
                {
                    Decrypt(file);
                }
                catch
                {

                }
                tracker++;
                Console.Title = ("Done " + (float)tracker / (float)filePaths.Length * 100+ "%");
            }
            
        }

        static void Decrypt(string path)
        {
            byte[] file = File.ReadAllBytes(path);
            int cnt = 0;
            while (cnt < file.Length)
            {
                if (file[cnt] == 0x00)
                {
                    //Console.WriteLine("Server Identification");
                    cnt = cnt + 3 + 128;
                }
                else if (file[cnt] == 0x01)
                {
                    //Console.WriteLine("Ping");
                    cnt++;
                }
                else if (file[cnt] == 0x02)
                {
                    //Console.WriteLine("Level Warning");
                    cnt++;
                }
                else if (file[cnt] == 0x03)
                {
                    //Console.WriteLine("Level Data");
                    int length = BitConverter.ToInt16(new byte[] { file[cnt + 2], file[cnt + 1] }, 0);
                    try
                    {
                        AppendData("./" + levelID + ".level", file, length, cnt + 3);
                    }
                    catch
                    {

                    }
                    cnt = cnt + 1 + 2 + 1024 + 1;
                }
                else if (file[cnt] == 0x04)
                {
                    //Console.WriteLine("Level Fianlise");
                    int x = BitConverter.ToInt16(new byte[] { file[cnt + 2], file[cnt + 1] }, 0);
                    int y = BitConverter.ToInt16(new byte[] { file[cnt + 4], file[cnt + 3] }, 0);
                    int z = BitConverter.ToInt16(new byte[] { file[cnt + 6], file[cnt + 5] }, 0);
                    try
                    {
                        File.Move("./" + levelID + ".level", "./" + levelID + string.Format("_{0}_{1}_{2}.level", x, y, z));
                    }
                    catch
                    {
                    }
                    cnt = cnt + 7;
                }
                else if (file[cnt] == 0x06)
                {
                    //Console.WriteLine("Block Set");
                    cnt = cnt + 8;
                }
                else if (file[cnt] == 0x07)
                {
                    //Console.WriteLine("Spawn Player");
                    cnt = cnt+10 + 64;
                }
                else if (file[cnt] == 0x08)
                {
                    //Console.WriteLine("Position and Orientation (Player Teleport)");
                    cnt = cnt +10;
                }
                else if (file[cnt] == 0x09)
                {
                    //Console.WriteLine("Position and Orientation Update");
                    cnt = cnt + 7;
                }
                else if (file[cnt] == 0x0a)
                {
                    //Console.WriteLine("Position Update");
                    cnt = cnt + 5;
                }
                else if (file[cnt] == 0x0b)
                {
                    //Console.WriteLine("View Update");
                    cnt = cnt + 4;
                }
                else if (file[cnt] == 0x0c)
                {
                    //Console.WriteLine("Despawn player");
                    cnt = cnt + 2;
                }
                else if (file[cnt] == 0x0d)
                {
                    //Console.WriteLine("Chat Message");
                    cnt = cnt + 2 + 64;
                }
                else if (file[cnt] == 0x0e)
                {
                    //Console.WriteLine("Kick Message");
                    cnt = cnt + 1 + 64;
                }
                else if (file[cnt] == 0x0f)
                {
                    //Console.WriteLine("Opped?");
                    cnt = cnt + 2;
                }
                else
                {
                    Console.WriteLine("Unknown packet {0} At {1} on file", file[cnt], cnt);
                    break;
                }
            }
        }

        static void AppendData(string filename, byte[] lotsOfData, int count, int index)
        {
            using (Stream fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                using (BinaryWriter bw = new BinaryWriter(fileStream))
                {
                    bw.Write(lotsOfData, index, count);
                }
            }
        }

    }
}
