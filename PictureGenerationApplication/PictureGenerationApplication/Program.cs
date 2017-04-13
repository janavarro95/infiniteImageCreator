using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureGenerationApplication
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static class FormManager{


        public static Form myForm;
        public static PictureBox pictureBox = new PictureBox();
         public static Random r = new Random(19950041);
            public static MemoryStream memoryStream;
       public static     int width = 4;
         public static   int height = 4;
            public static string path = "C:/Users/owner/Desktop/green.png";
            public static string path2 = "C:/Users/owner/Desktop/green2.png";
            public static void initialize()
            {
                myForm = new Form();
                myForm.Text= "Image Viewer";
                pictureBox.Dock = DockStyle.Fill;
                myForm.Controls.Add(pictureBox);
                myForm.KeyDown += Form_KeyDown;
            }
            public static bool once;
            public static int baseCount;
            public static int baseCount2;
            public static int iterAmount=32; //halving this number, I.E adding twice as many colors will increase the run time by a factor of 8, or it will be 8 times slower.
            /*
             * Approx run time analysis
             * iter=1 // run time 524288 seconds,or 6.06 Days for a 2x2 image
             * iter=1 //run time for a 1x1 px image would be 131072 seconds, or 2184.4 minutes or 36.4 hours or 1.51 days
             * //under these same circumstances to fill out a single 1920x 1080 image would take 3145728 days or 8618.432 years....
             * 
             * //to get this to movie standard of 24 frames per second would take 206842.389 years for a single second of film.
             * //knowing that
             * 1 second= 206842.389 years
             * 1 minute=12410543.34 years
             * 90 minute full featured film (no audio) = 1116948901 years. Or basically 1.116 billion years. But you could watch any film ever to exist at that resolution.
             * //at writing this (4/13/2017) the earth is 4.543 billion years old. Essentially it would take 1/4th the age of the earth to produce a movie like this one. 
             * 
             * 4x4px, iter=32= 1m:22s:14ms
             * 3x3px, iter=32= 43.68 seconds
             * 2x2 px,iter=32= 16.85 seconds
             * 
             * estimated time to completely color 1 pixel ~4-5 seconds
             * 
             * estimated time to completely color a 1920px by 1080 px screen= 10368000 seconds or 172800 minutes or 2880 hours or 120 days or about 4 months.
             * 
             */
            
            public static Color newRandomColor()
            {
                int red = r.Next(0,255);
                int green = r.Next(0,255);
                int blue = r.Next(0,255);
                return Color.FromArgb(255, red, green, blue);
            }

          


        }

        [STAThread]
        static void Main()
        {
            


            FormManager.initialize();
            FormManager.memoryStream = new MemoryStream();
            FormManager.once = false;
            FormManager.baseCount = 0;
            FormManager.baseCount2 = 0;
           // pictureBox.Image = YourImage;
           

            using (Bitmap b = new Bitmap(50, 50))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.Green);
                    
                }
                  b.Save(FormManager.path, ImageFormat.Png);

             FormManager.pictureBox.Image = Image.FromFile(FormManager.path);
            }

            //pictureBox.Update();

            Application.Run(FormManager.myForm);



            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
          //  Thread.Sleep(10000);
        

            if ((FormManager.once == true)) return;
            int iter = 32; ///iteration# = 256/iter
            FormManager.baseCount++;
            Console.WriteLine("Base count: " + FormManager.baseCount);
            FormManager.once = true;
            FormManager.pictureBox.Width = FormManager.pictureBox.Width * 4;
            FormManager.pictureBox.Height = FormManager.pictureBox.Height * 4;

            using (Bitmap b = new Bitmap(FormManager.width, FormManager.height))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    Color c = Color.Black;
                    g.Clear(c);
                }
                recursiveColor(b,0,0);
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            /*
                           for(int w=0; w<width; w++)
                            {

                                for(int h=0; h<height; h++)
                                {

                                    for(int red=0; red<255; red+= iter)
                                    {
                                        for (int green = 0; green < 255; green += iter)
                                        {

                                            for (int blue = 0; blue < 255; blue += iter)
                                            {
                                                Color c = Color.FromArgb(255, red, green, blue);
                                                b.SetPixel(w, h, c);

                                                using (var memoryStream = new MemoryStream())
                                                {
                                                    b.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                                    FormManager.pictureBox.Image = Image.FromStream(memoryStream);

                                                    FormManager.pictureBox.Refresh();


                                                    for(int w2 = w; w2 >= 0; w2--)
                                                    {
                                                        for(int h2=h; h2>=0; h2--)
                                                        {

                                                            for (int red = 0; red < 255; red += iter)
                                                            {
                                                                for (int green = 0; green < 255; green += iter)
                                                                {

                                                                    for (int blue = 0; blue < 255; blue += iter)
                                                                    {
                                                                        Color c = Color.FromArgb(255, red, green, blue);
                                                                        b.SetPixel(w, h, c);

                                                                        using (var memoryStream = new MemoryStream())
                                                                        {
                                                                            b.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                                                            FormManager.pictureBox.Image = Image.FromStream(memoryStream);

                                                                            FormManager.pictureBox.Refresh();

                                                                        }


                                                                    }


                                                }

                                            }

                                        }

                                    }

                                    // b.SetPixel(w, h, FormManager.newRandomColor());
                                }
                            }


                           /*
                            using (Graphics g = Graphics.FromImage(b))
                            {
                                Color c = FormManager.newRandomColor();
                                g.Clear(c);
                            }
                            */
            // b.Save(FormManager.path2, ImageFormat.Png);


        }


        public static void recursiveColor(Bitmap b, int x, int y)
        {
            FormManager.memoryStream.SetLength(0);
             int iterAmount=FormManager.iterAmount;
            //            Console.WriteLine("Start X: " + x);
            //            Console.WriteLine("Start Y: " + y);

            FormManager.baseCount2++;
            Console.WriteLine("Base count2: " + FormManager.baseCount2);

            if (y == FormManager.height-1)
            {
                x++;
                y = 0;
            }
            else
            {
                y++;
            }
            if (x > FormManager.width-1)
            {
                x = FormManager.width-1;
                y = FormManager.height - 1;
                return;
                //recursively call EVERYTHING???
            }

            Color startColor= b.GetPixel(x,y);
            int newRed = startColor.R;
            int newBlue=startColor.B;
            int newGreen=startColor.G;


            if (newRed >= 255 && newGreen >= 255 && newBlue >= 255)
            {
                //edgecase
                Console.WriteLine("COMPLETE");
                return;
            }

            if (newGreen >= 255)
            {
                newGreen = 0;

                newRed += iterAmount;
                if (newRed >= 255) newRed = 255;
            }

            bool blueOver = false;
            bool greenOver = false;

            if (newBlue >= 255)
            {
                newGreen += iterAmount;
                newBlue = 0;
                blueOver = true;
                if (newGreen >= 255)
                {
                    newRed += iterAmount;
                    newGreen = 0;
                    greenOver = true;
                    if (newRed >= 255)
                    {
                        newRed = 255;
                        if (blueOver == true && greenOver == true)
                        {
                            newBlue = 255;
                            newGreen = 255;
                            newRed = 255;
                        }
                    }
                }


            }

            newBlue += iterAmount;
            if (newBlue >= 255) newBlue = 255;

            b.SetPixel(x, y, Color.FromArgb(255, newRed, newGreen, newBlue));

     


           
                b.Save(FormManager.memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            FormManager.pictureBox.Invalidate();
            FormManager.pictureBox.Image.Dispose();

            FormManager.pictureBox.Image = Image.FromStream(FormManager.memoryStream);

            System.Threading.Thread.Sleep(5);
            FormManager.pictureBox.Refresh();
                

            
            int width = x;
            int height = y;
            bool f=true;
            while (f==true)
            {
                Console.WriteLine(b.GetPixel(x, y));
                f = recursiveColor2(b, x, y);
               
            }
            
            Console.WriteLine("UHHH1: "+x);
            Console.WriteLine("WTF2: "+y);
            for (int one = x; one >= 0; one--)
            {
                for(int two = y; two >= 0; two--)
                {
                   
                    
                    recursiveColor(b, one, two);
                }

            }
            return;
           // Color c = Color.FromArgb(255, red, green, blue);
           // b.SetPixel(w, h, c);

       
        }

        public static bool recursiveColor2(Bitmap b, int x, int y)
        {
            FormManager.memoryStream.SetLength(0);
             int iterAmount = FormManager.iterAmount;
            //            Console.WriteLine("Start X: " + x);
            //            Console.WriteLine("Start Y: " + y);



            Color startColor = b.GetPixel(x, y);
            int newRed = startColor.R;
            int newBlue = startColor.B;
            int newGreen = startColor.G;


            if (newRed >= 255 && newGreen >= 255 && newBlue >= 255)
            {
                //edgecase
                return false;
            }

            if (newGreen >= 255)
            {
                newGreen = 0;

                newRed += iterAmount;
                if (newRed >= 255) newRed = 255;
            }

            bool blueOver = false;
            bool greenOver=false;

            if (newBlue >= 255)
            {
                newGreen += iterAmount;
                newBlue = 0;
                blueOver = true;
                if (newGreen >= 255)
                {
                    newRed += iterAmount;
                    newGreen = 0;
                    greenOver = true;
                    if (newRed >= 255)
                    {
                        newRed = 255;
                        if(blueOver==true && greenOver == true)
                        {
                            newBlue = 255;
                            newGreen = 255;
                            newRed = 255;
                        }
                    }
                }

                
            }

            newBlue += iterAmount;
            if (newBlue >= 255) newBlue = 255;

            b.SetPixel(x, y, Color.FromArgb(255, newRed, newGreen, newBlue));





            b.Save(FormManager.memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            FormManager.pictureBox.Invalidate();
            FormManager.pictureBox.Image.Dispose();
            
                        Bitmap f = new Bitmap(b.Size.Width*4, b.Size.Height *4);
                        using (Graphics g = Graphics.FromImage((Image)f))
                        {
                          //  g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(f, 0, 0, f.Size.Width*4, f.Size.Height*4);
                        }
                        
            
            b.Save(FormManager.memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            FormManager.pictureBox.Image = Image.FromStream(FormManager.memoryStream);

            System.Threading.Thread.Sleep(5);
            FormManager.pictureBox.Refresh();

            if (newRed == 255 && newGreen == 25 && newBlue == 255){
                return false;
            }

            int width = x;
            int height = y;
            
            while (width>=0)
            {
                if(height==0)
                {
                    height = FormManager.height-1;
                    width--;
                }
                else
                {
                    height--;
                }
                if (width >=0) recursiveColor2(b, width, height);
                else break;
            }
            return false;
            Console.WriteLine("UHHH1: " + x);
            Console.WriteLine("WTF2: " + y);
            for (int one = x; one >= 0; one--)
            {
                for (int two = y; two >= 0; two--)
                {


                    recursiveColor(b, one, two);
                }

            }
            return false;
            // Color c = Color.FromArgb(255, red, green, blue);
            // b.SetPixel(w, h, c);


        }
    }
}
