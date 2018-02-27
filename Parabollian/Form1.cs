using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                Fure();
            }
        }

        private void Generate(float k, float m, float step = 0.1f)
        {
            float start_x = 300, start_y = 300;
            float y = 0;
            float x = 0;
            Brush pn = Brushes.Red;
            Brush coordLine = Brushes.Black;
            Graphics gr = this.CreateGraphics();
            gr.Clear(Color.White);
            gr.DrawLine(new Pen(coordLine, 1), new PointF(0, start_y), new PointF(600, start_y));
            gr.DrawLine(new Pen(coordLine, 1), new PointF(start_x, 0), new PointF(start_x, 600));
            for (x = -200; x <= 200; x = x + step)
            {
                y = (float)(k * x + m * Math.Sin(x));
                gr.FillRectangle(pn, new RectangleF(new PointF(start_x + x, start_y + (-1) * y), new SizeF(1, 1)));
                y = (float)(k * x);
                gr.FillRectangle(Brushes.Blue, new RectangleF(new PointF(start_x + x, start_y + (-1) * y), new SizeF(1, 1)));
            }
        }

        private void Fure()
        {
            float start_x = 300, start_y = 300;
            float y = 0;
            float x = 0;
            is_Drowing = true;
            Brush pn = Brushes.Red;
            Brush coordLine = Brushes.Black;
            Graphics gr = this.CreateGraphics();
            gr.Clear(Color.White);
            gr.DrawLine(new Pen(coordLine, 1), new PointF(0, start_y), new PointF(600, start_y));
            gr.DrawLine(new Pen(coordLine, 1), new PointF(start_x, 0), new PointF(start_x, 600));
            Drowingth = new Thread(() =>
            {
                try
                {
                    float step = 1f;
                    string s = lua.Lua["step"].ToString();
                    float.TryParse(s, out step);
                    for (x = -200; x <= 200; x = x + step)
                    {
                    
                            lua.Lua["x"] = x;
                            lua.Lua.DoString(textBox2.Text);
                            object j = lua.Lua["y"];
                            y = float.Parse(j.ToString());
                            gr.FillRectangle(pn, new RectangleF(new PointF(start_x + x, start_y + (-1) * y), new SizeF((int)SizeOfPoint,(int)SizeOfPoint)));
                    }
                }
                catch { }
                is_Drowing = false;
            });
            Drowingth.Start();
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
                    Fure();
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lua.Lua["step"] = 1f/float.Parse(textBox1.Text);
            Drowingth?.Abort();
            is_Drowing = true;
            Fure();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!is_Drowing)
            {
                if (listBox1.SelectedItem != null)
                {
                    textBox2.Text = listBox1.SelectedItem.ToString();
                    Fure();
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

        }
    }
}
