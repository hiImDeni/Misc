using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace editor {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        void saveImage()
        {
            if (opened == true)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Images|*.png;*.bmp;*.jpg";
                ImageFormat format = ImageFormat.Png;// default format
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string ext = Path.GetExtension(sfd.FileName);
                    switch (ext)
                    {
                        case ".jpg":
                            {
                                format = ImageFormat.Jpeg;
                                break;
                            }
                        case ".bmp":
                            {
                                format = ImageFormat.Bmp;
                                break;
                            }
                    }
                    pictureBox1.Image.Save(sfd.FileName, format);
                }
                saved = true;

            }
            else
            {
                DialogResult renunta;
                renunta = MessageBox.Show("Open an image first", "No image opened",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (renunta == DialogResult.Cancel) this.Close();
            }
        }

        void reloadImage()
        {
            if (opened)
            {
                file = Image.FromFile(openFileDialog.FileName);
                pictureBox1.Image = file;
            }
        }

		void pasteImage() {
			if (opened) {
				pictureBox1.Image = photo;
			}
		}


		private void button1_Click(object sender, EventArgs e) {
            reloadImage();
            saved = false;
		}

		Image file; //retine fisierul
		Boolean opened = false; //verifica daca a fost selectata o imagine
		Boolean saved = false; //verifica daca imaginea a fost salvata
		Image photo;
		//Bitmap map = new Bitmap(img.Width, img.Height);

		private void button9_Click(object sender, EventArgs e) {//open
			DialogResult dr = openFileDialog.ShowDialog();
			if (dr == DialogResult.OK) {
				file = Image.FromFile(openFileDialog.FileName);
				pictureBox1.Image = file;
				opened = true;
			}
		}

		private void button8_Click(object sender, EventArgs e) {
            saveImage();
		}

		private void trackBar1_Scroll(object sender, EventArgs e) {//luminozitate
			if (!opened) {
				MessageBox.Show("Open an image then apply changes");
			} else {
				reloadImage();

				saved = false;
				float val = trackBar1.Value / 100.0f;

				Image img = pictureBox1.Image;
				Bitmap bmp = new Bitmap(img.Width, img.Height);

				ImageAttributes ia = new ImageAttributes();
				ColorMatrix cm = new ColorMatrix(new float[][]
				{
					new float[] {1 , 0, 0, 0, 0},
					new float[] {0, 1, 0, 0, 0},
					new float[] {0, 0, 1, 0, 0},
					new float[] {0, 0, 0, 1, 0},
					new float[] {val, val, val, 0, 1}
				});
				ia.SetColorMatrix(cm);
				Graphics g = Graphics.FromImage(bmp);
				g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

				g.Dispose();
				pictureBox1.Image = bmp;
				photo = bmp;
			}
		}

		private void trackBar2_Scroll(object sender, EventArgs e) {//highlights
			if (!opened) {
				MessageBox.Show("Open an image then apply changes");
			} 
			else {
				reloadImage();

				saved = false;
				float c = 0.04f * trackBar2.Value;

				Image img = pictureBox1.Image;
				Bitmap bmp = new Bitmap(img.Width, img.Height);

				ImageAttributes ia = new ImageAttributes();
				ColorMatrix cm = new ColorMatrix(new float[][]
				{
					new float[] {1 + c, 0, 0, 0, 0},
					new float[] {0, 1 + c, 0, 0, 0},
					new float[] {0, 0, 1 + c, 0, 0},
					new float[] {0, 0, 0, 1, 0},
					new float[] {0.001f, 0.001f, 0.001f, 0, 1}
				});
				ia.SetColorMatrix(cm);
				Graphics g = Graphics.FromImage(bmp);
				g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

				g.Dispose();
				pictureBox1.Image = bmp;
				photo = bmp;
			}
		}

		private void trackBar3_Scroll(object sender, EventArgs e) {//contrast
			if (!opened) {
				MessageBox.Show("Open an image then apply changes");
			} 
			else {
				reloadImage();

				saved = false;

				Image img = pictureBox1.Image;
				Bitmap bmp = new Bitmap(img.Width, img.Height);
				ImageAttributes ia = new ImageAttributes();
				
				float c = 1 + trackBar3.Value / 100.0f;
				float t = (-0.5f * c + 0.5f) * 255.0f;

				/*float R = (1.0f - s) * 0.3086f;
				float G = (1.0f - s) * 0.6094f;
				float B = (1.0f - s) * 0.0820f;*/

				ColorMatrix cm = new ColorMatrix(new float[][]
				{
					new float[] {c, 0, 0, 0, t},
					new float[] {0, c, 0, 0, t},
					new float[] {0, 0, c, 0, t},
					new float[] {0, 0, 0, 1, 0},
					new float[] {0, 0, 0, 0, 1}
				});
				ia.SetColorMatrix(cm);
				Graphics g = Graphics.FromImage(bmp);
				g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

				g.Dispose();
				pictureBox1.Image = bmp;
				photo = bmp;
			}
		}

		private void Form1_Load(object sender, EventArgs e) {

		}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult cls;
            if (saved == false && opened == true)
            {
                
                cls = MessageBox.Show("Are you sure you want to exit without saving?", "Image not saved",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (cls == DialogResult.No) e.Cancel = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)//grayscale
        {
            reloadImage();

            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {

                Image img = pictureBox1.Image; // retine imaginea din PictureBox                            
                Bitmap bmp = new Bitmap(img.Width, img.Height);// creaza un bitmap de aceleasi dimensiuni cu imaginea initiala

                ImageAttributes ia = new ImageAttributes(); // pentru schimbarea atributelor imaginii               
                ColorMatrix cm = new ColorMatrix(new float[][]       
                {
                    new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                    new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                    new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                    new float[] { 0,      0,      0,      1, 0 },
                    new float[] { 0,      0,      0,      0, 1 }
                });
                ia.SetColorMatrix(cm);  // aplicarea filtrului 
                Graphics g = Graphics.FromImage(bmp);   /*create a new object of graphics named g, ; Create graphics object for alteration.
                                                            Graphics newGraphics = Graphics.FromImage(imageFile); is the format of loading image into graphics for alteration*/

                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);// "afisarea" imaginii


                /*   g.drawimage(image, new rectangle(location of rectangle axix-x, location axis-y, width of rectangle, height of rectangle),
               location of image in rectangle x-axis, location of image in rectangle y-axis, width of image, height of image,
               format of graphics unit,provide the image attributes   */


                g.Dispose();        //Releases all resources used by this Graphics.
                pictureBox1.Image = bmp;
				photo = bmp;
			}
            saved = false;
        }

        private void button3_Click(object sender, EventArgs e)//sepia
        {
            reloadImage();

            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {

                Image img = pictureBox1.Image;                            
                Bitmap bmp = new Bitmap(img.Width, img.Height);

                ImageAttributes ia = new ImageAttributes();             
                ColorMatrix cm = new ColorMatrix(new float[][]
                {
                    new float[] {.393f, .349f, .272f, 0, 0},
                    new float[] {.769f, .686f, .534f, 0, 0},
                    new float[] {.189f, .168f, .131f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });
                ia.SetColorMatrix(cm);  
                Graphics g = Graphics.FromImage(bmp);   
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

                g.Dispose();        
                pictureBox1.Image = bmp;
				photo = bmp;
			}
            saved = false;
        }

        private void button4_Click(object sender, EventArgs e)//fade
        {
            reloadImage();

            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {

                Image img = pictureBox1.Image;                         
                Bitmap bmp = new Bitmap(img.Width, img.Height);

                ImageAttributes ia = new ImageAttributes();               
                ColorMatrix cm = new ColorMatrix(new float[][]
                {
                    new float[] {1, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 0, 0},
                    new float[] {0, 0, 1, 0, 0},
                    new float[] {0, 0, 0, 0.7f, 0},
                    new float[] {0, 0, 0, 0, 1}
                });
                ia.SetColorMatrix(cm);  
                Graphics g = Graphics.FromImage(bmp);   
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

                g.Dispose();        
                pictureBox1.Image = bmp;
				photo = bmp;
			}
            saved = false;
        }

        private void button5_Click(object sender, EventArgs e)//rose
        {
            reloadImage();
            saved = false;
            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                Image img = pictureBox1.Image;
                Bitmap bmp = new Bitmap(img.Width, img.Height);

                ImageAttributes ia = new ImageAttributes();
                
                 ColorMatrix cm = new ColorMatrix(new float[][]
                {
                    new float[] {1.143f, 0, 0, 0, 0},
                    new float[] {0, 1, 0, 1, 0},
                    new float[] {0, 0, 1.34f, 0, 0},
                    new float[] {0, 0, 0, 1.7f, 0},
                    new float[] {0, 0, 0, 0, 2}
                });

                ia.SetColorMatrix(cm);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
                
                g.Dispose();
                pictureBox1.Image = bmp;
				photo = bmp;
			}
        }

        private void button6_Click(object sender, EventArgs e)//lumin
        {
            reloadImage();
            saved = false;
            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                Image img = pictureBox1.Image;
                Bitmap bmp = new Bitmap(img.Width, img.Height);

                ImageAttributes ia = new ImageAttributes();

                ColorMatrix cm = new ColorMatrix(new float[][]
               {
                    new float[] {1.6f, 0, 0, 2f, 0},
                    new float[] {0, 1.7f, 0, 1.543f, 0},
                    new float[] {0, 0, 1.5f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 4 }
               });

                ia.SetColorMatrix(cm);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

                g.Dispose();
                pictureBox1.Image = bmp;
				photo = bmp;
			}
        }

        private void button7_Click(object sender, EventArgs e)//polaroid
        {
            reloadImage();
            saved = false;
            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                Image img = pictureBox1.Image;
                Bitmap bmp = new Bitmap(img.Width, img.Height);

                ImageAttributes ia = new ImageAttributes();

                ColorMatrix cm = new ColorMatrix(new float[][]
               {
                    new float[] {1.438f, -0.062f, -0.062f, 0, 0},
                    new float[] {-0.122f, 1.378f, -0.122f, 0, 0},
                    new float[] {-0.016f, -0.016f, 1.483f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] { -0.03f, 0.05f, -0.02f, 0, 1 }
               });

                ia.SetColorMatrix(cm);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

                g.Dispose();
                pictureBox1.Image = bmp;
				photo = bmp;
			}
        }

		private void button10_Click(object sender, EventArgs e) {
			DialogResult cls;
			cls = MessageBox.Show("Are you sure you want to exit?", "",
					MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if (cls == DialogResult.Yes)
				this.Close();
		}

        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
