using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] filePaths = Directory.GetFiles(@"C:\Users\Ben\Documents\GitHub\MCClassicToolkit", "*.level");
            int indexx = 0;
            foreach (string file in filePaths)
            {
                if (file.Contains("_"))
                {
                    string[] bits = file.Split('\\');
                    Console.WriteLine(bits[bits.Length-1]);
                    try
                    {
                        Carto(file);
                    }
                    catch
                    {

                    }
                    Console.Title = "Done " + (float)indexx / (float)filePaths.Length * 100 + "%";
                }
            }
        }
        static void Carto(string filename)
        {
            string[] bits = filename.Split('\\');
            string fin = bits[bits.Length - 1].Split('.')[0];
            string[] cords = fin.Split('_');
            Bitmap bmp = new Bitmap(Int32.Parse(cords[1]), Int32.Parse(cords[3]), PixelFormat.Format32bppArgb);
            byte[] file = File.ReadAllBytes(filename);
            byte[] level;
            try
            {
                level = Decompress(file);
            }
            catch
            {
                return;
            }
            int depth = Int32.Parse(cords[3]);
            int Width = Int32.Parse(cords[1]);
            for (int x = 0; x < Int32.Parse(cords[1]); x++ )
            {
                for (int z = 0; z < Int32.Parse(cords[3]); z++)
                {
                    int y = Int32.Parse(cords[2]) - 1;

                    //int BlockPos = y * Int32.Parse(cords[3]) * Int32.Parse(cords[2]) + z * Int32.Parse(cords[2]) + x;
                    // Index = x + (y * Depth + z) * Width
                    //  x + (y * Int32.Parse(cords[2]) + z) * nt32.Parse(cords[1]))


                    while (level[x + (y * depth + z) * Width] == 0)
                    {
                        y--;
                        
                        if (y == 1)
                        {
                            break;
                        }
                        if (level[x + (y * depth + z) * Width] != 0)
                        {
                            
                            //Console.Write(level[y * Int32.Parse(cords[3]) * Int32.Parse(cords[2]) + z * Int32.Parse(cords[2]) + x] + " ");
                            int blocktype = level[x + (y * depth + z) * Width];
                            switch (blocktype)
                            {
                                #region colorCra]
                                case 1:
                                    bmp.SetPixel(x, z, Color.Gray);
                                    break;
                                case 2:
                                    bmp.SetPixel(x, z, Color.Green);
                                    break;
                                case 3:
                                    bmp.SetPixel(x, z, Color.Brown);
                                    break;
                                case 4:
                                    bmp.SetPixel(x, z, Color.DarkGray);
                                    break;
                                case 5:
                                    bmp.SetPixel(x, z, Color.DarkSalmon);
                                    break;
                                case 6:
                                    bmp.SetPixel(x, z, Color.DarkGreen);
                                    break;
                                case 7:
                                    bmp.SetPixel(x, z, Color.Black);
                                    break;
                                case 8:
                                    bmp.SetPixel(x, z, Color.Blue);
                                    break;
                                case 9:
                                    bmp.SetPixel(x, z, Color.Blue);
                                    break;
                                case 10:
                                    bmp.SetPixel(x, z, Color.Red);
                                    break;
                                case 11:
                                    bmp.SetPixel(x, z, Color.Red);
                                    break;
                                case 12:
                                    bmp.SetPixel(x, z, Color.LightGoldenrodYellow);
                                    break;
                                case 13:
                                    bmp.SetPixel(x, z, Color.Purple);
                                    break;
                                case 14:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 15:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 16:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 17:
                                    bmp.SetPixel(x, z, Color.Plum);
                                    break;
                                case 18:
                                    bmp.SetPixel(x, z, Color.DarkGreen);
                                    break;
                                case 19:
                                    bmp.SetPixel(x, z, Color.Yellow);
                                    break;
                                case 20:
                                    bmp.SetPixel(x, z, Color.White);
                                    break;
                                case 21:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 22:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 23:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 24:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 25:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 26:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 28:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 29:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 30:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 31:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 32:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 33:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 34:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 35:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 36:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 37:
                                    bmp.SetPixel(x, z, Color.LightYellow);
                                    break;
                                case 38:
                                    bmp.SetPixel(x, z, Color.LightPink);
                                    break;
                                case 39:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 40:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 41:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 42:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 43:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 44:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 45:
                                    bmp.SetPixel(x, z, Color.LightSalmon);
                                    break;
                                case 46:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 47:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 48:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                case 49:
                                    bmp.SetPixel(x, z, Color.LightGray);
                                    break;
                                #endregion
                            }
                            
                        }
                    }
                }
            }
            bmp.Save("./" + bits[bits.Length - 1].Split('.')[0] + ".bmp", ImageFormat.Bmp);
        }


        static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}
