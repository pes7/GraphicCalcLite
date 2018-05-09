using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parabollian
{
    public partial class Form1 : Form
    {
        OLua lua = new OLua();
        Thread Drowingth;
        bool is_Drowing = false;
        enum PointSize {Small=1,Middle=2,Large=4};
        PointSize SizeOfPoint = PointSize.Small;

        /*
         * Ukolov's Function : y=tg(x)*m+x^2*k; 
         * y=sin(x)*x^2+m*x
         * y=sin(x)*x/k+m*x
         * y=abs(x)*tg(x)+sin(x)*m
         * y=abs(x)*tg(x)/k+sin(x)*m/tanh(x)-abs(x)
         * y=(sin(x)*x^2+m*x)*tg(x)
         * y=sin(x)*x*tg(m)+x*k
         */

        public Form1()
        {
            InitializeComponent();
            lua.Lua["y"] = 0f;
            lua.Lua["x"] = 0f;
            lua.Lua["k"] = trackBar1.Value / 100f;
            lua.Lua["m"] = trackBar2.Value;
            lua.Lua["step"] = 1f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!is_Drowing)
            {
                listBox1.Items.Add(textBox2.Text);
                Fure(gr);
            }
        }

        private Graphics gr;
        private void Fure(Graphics ph, bool usethread = true)
        {
            float start_x = 300, start_y = 300;
            float y = 0;
            float x = 0;
            is_Drowing = true;
            Brush pn = Brushes.Red;
            Brush coordLine = Brushes.Black;
            ph.Clear(Color.White);
            ph.DrawLine(new Pen(coordLine, 1), new PointF(0, start_y), new PointF(600, start_y));
            ph.DrawLine(new Pen(coordLine, 1), new PointF(start_x, 0), new PointF(start_x, 600));
            if (usethread == true)
            {
                Drowingth = new Thread(() =>
                {
                    try
                    {
                        float step = 1f;
                        string s = lua.Lua["step"].ToString();
                        float.TryParse(s, out step);
                        for (x = FromToGenerate.X; x <= FromToGenerate.Y; x = x + step)
                        {

                            lua.Lua["x"] = x;
                            lua.Lua.DoString(textBox2.Text);
                            object j = lua.Lua["y"];
                            y = float.Parse(j.ToString());
                            ph.FillRectangle(pn, new RectangleF(new PointF(start_x + x, start_y + (-1) * y), new SizeF((int)SizeOfPoint, (int)SizeOfPoint)));
                        }
                    }
                    catch { }
                    is_Drowing = false;
                });
                Drowingth.Start();
            }
            else
            {
                try
                {
                    float step = 1f;
                    string s = lua.Lua["step"].ToString();
                    float.TryParse(s, out step);
                    for (x = FromToGenerate.X; x <= FromToGenerate.Y; x = x + step)
                    {

                        lua.Lua["x"] = x;
                        lua.Lua.DoString(textBox2.Text);
                        object j = lua.Lua["y"];
                        y = float.Parse(j.ToString());
                        ph.FillRectangle(pn, new RectangleF(new PointF(start_x + x, start_y + (-1) * y), new SizeF((int)SizeOfPoint, (int)SizeOfPoint)));
                    }
                }
                catch { }
                is_Drowing = false;
            }
        }

        private void InvokeUI(Action a)
        {
            BeginInvoke(new MethodInvoker(a));
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            changed();
            //Generate(trackBar1.Value / 100f, trackBar2.Value, trackBar3.Value / 100f);
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            changed();
            //Generate(trackBar1.Value / 100f, trackBar2.Value, trackBar3.Value/100f);
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            changed();
            //Generate(trackBar1.Value / 100f, trackBar2.Value, trackBar3.Value / 100f);
        }

        private void changed()
        {
            try
            {
                lua.Lua["k"] = trackBar1.Value / 100f;
                lua.Lua["m"] = trackBar2.Value;
                lua.Lua["step"] = trackBar3.Value / 100f;
                if (checkBox1.Checked)
                {
                    Drowingth?.Abort();
                    is_Drowing = true;
                    Fure(gr);
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lua.Lua["step"] = 1f/float.Parse(textBox1.Text);
            Drowingth?.Abort();
            is_Drowing = true;
            Fure(gr);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!is_Drowing)
            {
                if (listBox1.SelectedItem != null)
                {
                    textBox2.Text = listBox1.SelectedItem.ToString();
                    Fure(gr);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SizeOfPoint = (PointSize)Enum.Parse(typeof(PointSize),comboBox1.Text);
            changed();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FromToGenerate = new Point(-300, 300);
            gr = this.CreateGraphics();
            textBox3.Text = $"{FromToGenerate.X}";
            textBox4.Text = $"{FromToGenerate.Y}";
        }

        public Point FromToGenerate;
        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap bt = new Bitmap(600, 600);
            var gd = Graphics.FromImage(bt);
            Fure(gd,false);
            gd.DrawString(textBox2.Text,SystemFonts.MessageBoxFont,Brushes.Blue,new PointF(0, 560));
            Clipboard.SetImage(bt);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                FromToGenerate.X = int.Parse(textBox3.Text);
            }
            catch { }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            FromToGenerate.Y = int.Parse(textBox4.Text);
        }

        Record rec = null;
        private void button5_Click(object sender, EventArgs e)
        {
            if(rec == null)
            {
                Thread th = new Thread(() =>
                {
                    rec = new Record(this);
                    rec.Location = new Point(this.Location.X + this.Size.Width + 10, this.Location.Y);
                    rec.ShowDialog();
                });
                th.Start();
            }
            else
            {
                rec.Show();
                rec.Invoke((MethodInvoker)delegate
                {
                    rec.Location = new Point(this.Location.X + this.Size.Width + 10, this.Location.Y);
                });
            }
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (rec.Visible == true)
            {
                try
                {
                    rec.Invoke((MethodInvoker)delegate
                    {
                        rec.Location = new Point(this.Location.X + this.Size.Width + 10, this.Location.Y);
                    });
                }
                catch { }
            }
        }
    }
}
