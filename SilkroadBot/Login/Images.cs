using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;

namespace LeoBot
{
    class Images
    {
        [DllImport("Data/ZlibDll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Decompress(byte[] compressed_buffer, int compressed_size, byte[] decompressed_buffer, ref int decompressed_size);

        private static ImageSource Convert(Image fs)
        {
            System.Drawing.Bitmap dImg = new System.Drawing.Bitmap(fs);
            MemoryStream ms = new MemoryStream();
            dImg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();
            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(ms.ToArray());
            bImg.EndInit();

            return bImg;
        }

        public void ShowImage(UInt32[] pixels)
        {
            const Int32 width = 200;
            const Int32 height = 64;

            // Hard coded image header for the type the captcha uses
            byte[] header = new byte[]
	        {
		        0x42, 0x4D, 0x7A, 0xC8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7A, 0x00, 0x00, 0x00, 0x6C, 0x00, 
		        0x00, 0x00, 0xC8, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x01, 0x00, 0x20, 0x00, 0x03, 0x00, 
		        0x00, 0x00, 0x00, 0xC8, 0x00, 0x00, 0x12, 0x0B, 0x00, 0x00, 0x12, 0x0B, 0x00, 0x00, 0x00, 0x00, 
		        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00, 0x00, 0xFF, 0x00, 0x00, 0xFF, 0x00, 
		        0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
		        0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 
		        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 
		        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	        };

            Stream streamas = new MemoryStream();
            Image returnImage;
            using (BinaryWriter bw = new BinaryWriter(streamas))
            {
                bw.Write(header);
                for (int c = height - 1; c >= 0; --c)
                {
                    for (int r = 0; r < width; ++r)
                    {
                        bw.Write((UInt32)pixels[c * width + r]);
                    }
                }
                returnImage = Image.FromStream(streamas);
                bw.Flush();
            }
            if (Globals.MainWindow.captcha.Dispatcher.CheckAccess())
            {
                Globals.MainWindow.captcha.Source = Convert(InvertImageColorMatrix(returnImage));
            }
            else
            {
                Globals.MainWindow.captcha.Dispatcher.Invoke((Action)delegate()
                {
                    Globals.MainWindow.captcha.Source = Convert(InvertImageColorMatrix(returnImage));
                });
            }
        }
        
        private Image InvertImageColorMatrix(Image originalImg)
        {
            Bitmap invertedBmp = new Bitmap(originalImg.Width, originalImg.Height);

            //Setup color matrix
            ColorMatrix clrMatrix = new ColorMatrix(new float[][]
                                                    {
                                                    new float[] {-1, 0, 0, 0, 0},
                                                    new float[] {0, -1, 0, 0, 0},
                                                    new float[] {0, 0, -1, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {1, 1, 1, 0, 1}
                                                    });

            using (ImageAttributes attr = new ImageAttributes())
            {
                //Attach matrix to image attributes
                attr.SetColorMatrix(clrMatrix);

                using (Graphics g = Graphics.FromImage(invertedBmp))
                {
                    g.DrawImage(originalImg, new Rectangle(0, 0, originalImg.Width, originalImg.Height),
                                0, 0, originalImg.Width, originalImg.Height, GraphicsUnit.Pixel, attr);
                }
            }

            return invertedBmp;
        }

        public void CreateImage(Packet packet)
        {
            UInt32[] pixels;
            pixels = GeneratePacketCaptcha(packet);
            ShowImage(pixels);
        }

        public static UInt32[] GeneratePacketCaptcha(Packet packet)
        {
            byte flag = packet.ReadUInt8();
            UInt16 remain = packet.ReadUInt16();
            UInt16 compressed = packet.ReadUInt16();
            UInt16 uncompressed = packet.ReadUInt16();
            UInt16 width = packet.ReadUInt16();
            UInt16 height = packet.ReadUInt16();

            if (width != 200 || height != 64)
            {
                throw new NotImplementedException("The captcha is expected to be 200 x 64 pixels.");
            }

            byte[] compressed_buffer = new byte[packet.GetBytes().Length - 11];
            for (int i = 0; i < packet.GetBytes().Length - 11; i++)
            {
                compressed_buffer[i] = packet.ReadUInt8();
            }
          
            Int32 uncompressed_size = uncompressed;

            byte[] uncompressed_buffer = new byte[uncompressed];
            int result = Decompress(compressed_buffer, compressed, uncompressed_buffer, ref uncompressed_size);
            if (result != 0)
            {
                throw new Exception("Decompress returned error code " + result);
            }

            byte[] uncompressed_bytes = new byte[uncompressed_size];
            Buffer.BlockCopy(uncompressed_buffer, 0, uncompressed_bytes, 0, uncompressed_size);
            uncompressed_buffer = null;

            UInt32[] pixels = new UInt32[width * height];

            int ind_ = 0;
            for (int row_ = 0; row_ < height; ++row_)
            {
                for (int col_ = 0; col_ < width; ++col_)
                {
                    UInt32 write_index = (UInt32)(row_ * width + col_);
                    pixels[write_index] = (UInt32)((byte)(Math.Pow(2.0f, ind_++ % 8)) & uncompressed_bytes[write_index / 8]);
                    if (pixels[write_index] == 0)
                    {
                        pixels[write_index] = 0xFF000000;
                    }
                    else
                    {
                        pixels[write_index] = 0xFFFFFFFF;
                    }
                }
            }

            return pixels;
        }

        public static void CaptchaResult(Packet packet)
        {
            byte result = packet.ReadUInt8();
            if (result == 2)
            {
                uint maxTry = packet.ReadUInt32();
                uint curTry = packet.ReadUInt32();
                Globals.MainWindow.UpdateLogs(string.Format("Wrong captcha entered! You have {0} attempts left!", (maxTry - curTry)));
            }
            else if (result == 1)
            {
                Globals.MainWindow.UpdateLogs("Captcha has been successfully entered!");
            }
        }

    }
}
