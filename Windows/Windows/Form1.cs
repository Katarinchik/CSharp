using System;
using System.ComponentModel;
using System.Threading;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Windows
{
    
    public partial class Form1 : Form
    {
        int pnum = 0;
        int pnum2 = 0;
        int pointnum = 3;
        int pointnum2 = 3;
        public List<PointF> Arr1 = new List<PointF>(3) { };
        public List<PointF> Arr2 = new List<PointF>(3) { };
        RadioButton radioButton;
        public Form1()
        {

            InitializeComponent();
            pictureBox1.MouseClick += new MouseEventHandler(Form1_MouseClick);
        }
        Graphics G; 

        void DrawPoint(PointF e, Pen a)
        {
            PointF[] Pt = new PointF[]
                {
                new PointF(e.X+1, e.Y - 1),
                new PointF(e.X-1, e.Y + 1),
                new PointF(e.X-1, e.Y - 1),
                new PointF(e.X+1, e.Y + 1)
                };
            G.DrawLines(a, Pt);

        }

        void Form1_MouseClick(object sender, MouseEventArgs e)//добавление точки по нажатию мыши
        {
            G = Graphics.FromHwnd(pictureBox1.Handle);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Point w = new Point(e.X, e.Y);
           
            if (radioButton.Text == "Первый многоугольник")
            {
                
               
                if (pnum == 0)
                {
                    Arr1.Clear();
                    Arr1.Add(new Point(e.X, e.Y));
                    G.Clear(Color.White);
                }
                else
                {
                    Arr1.Add(new Point(e.X, e.Y));
                    G.DrawLine(new Pen(Color.Orange),Arr1[pnum], Arr1[pnum-1]);
                }
                
                pnum++;
            }
            else
            {
                
                if (pnum2 != 0)
                {
                    Arr2.Add(new PointF(e.X, e.Y));
                    G.DrawLine(new Pen(Color.Blue), Arr2[pnum2], Arr2[pnum2 - 1]);
                }
                else
                {
                    Arr2.Clear();
                    Arr2.Add(new PointF(e.X, e.Y));
                }
              
                  


                pnum2++;
            }

            G = Graphics.FromHwnd(pictureBox1.Handle);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawPoint(w, new Pen(Color.Red));
            if (pnum == pointnum)
            {
                G.DrawLine(new Pen(Color.Orange), Arr1[pnum-1], Arr1[0]);
                pnum = 0;
            }
            if (pnum2 == pointnum2)
            {
                G.DrawLine(new Pen(Color.Blue), Arr2[pnum2-1], Arr2[0]);
                pnum2 = 0;
            }
 
        }
        bool are_cross(PointF p1, PointF p2, PointF p3, PointF p4)
        {           
        float f1, f2, f3, f4;
        f1= (p4.X - p3.X) * ((556- p1.Y) -(556-p3.Y)) - ((556- p4.Y)- (556 - p3.Y)) * (p1.X - p3.X);
        f2= (p4.X - p3.X) * ((556- p2.Y)- (556-p3.Y)) - ((556- p4.Y)- (556 - p3.Y)) * (p2.X - p3.X);
        f3= (p2.X - p1.X) * ((556- p3.Y)- (556-p1.Y)) - ((556- p2.Y)- (556 - p1.Y)) * (p3.X - p1.X);
        f4= (p2.X - p1.X) * ((556- p4.Y)- (556-p1.Y)) - ((556 -p2.Y)- (556 - p1.Y)) * (p4.X - p1.X);
                                       
            return (f1 * f2 < 0) & (f3 * f4 < 0);
        }
        PointF intersection_point(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            float sn, fn,q, n;
            if (p2.Y - p1.Y != 0)
            {  
                q = (p2.X - p1.X) / (p1.Y - p2.Y);
                sn = (p3.X - p4.X) +(p3.Y - p4.Y) * q; 
                fn = (p3.X - p1.X) +(p3.Y - p1.Y) * q;   
                n = fn / sn;
            }
            else
                n = (p3.Y - p1.Y) / (p3.Y - p4.Y);
            
            return new PointF((int)(p3.X + (p4.X - p3.X) * n),(int)( p3.Y + (p4.Y - p3.Y) * n));
        }
        List<PointF> sort(List<PointF> arr)
        {
            int num = 0, f = arr.Count() - 1;
            List<PointF> result = new List<PointF>();
            List<double> len = new List<double>();
            for (int i = 0; i < f; i++)
                len.Add( Math.Sqrt((arr[0].X - Math.Abs(arr[i + 1].X)) * (arr[0].X - Math.Abs(arr[i + 1].X)) + (arr[0].Y - arr[i + 1].Y) * (arr[0].Y - arr[i + 1].Y)));
            result.Add(arr[0]);
            arr.RemoveAt(0);
            double min = len[0];                       
            for (int j = 0; j < f; j++)
            {
                num = 0;
                for (int i = 0; i < len.Count(); i++)
                    if (len[i] < min)
                        num = i;
                result.Add(arr[num]);
                arr.RemoveAt(num);
                len.RemoveAt(num);
            }
            
            return result;
        }
        bool is_inside(List <PointF> p, PointF t)
        {
            int count = 0;
            for (int i = 0; i < p.Count(); i++)
                if (are_cross(new PointF(0, 0), t, p[i], p[(i + 1) % p.Count()]))
                    count++;
            return count % 2 == 1;
        }
        bool clockwise(List<PointF>arr)
        {
           int n=0;
            float min = 1000, x1,x2,y1,y2;
            for(int i = 0;i<arr.Count() ; i++)
            {
                if (arr[i].X<min)
                {
                    min = arr[i].X;
                    n = i;
                }
            }

            x1 = arr[n].X- arr[(n - 1 + arr.Count()) % arr.Count()].X;
            y1 = arr[n].Y - arr[(n - 1 + arr.Count()) % arr.Count()].Y;
            x2 = arr[(n + 1 + arr.Count()) % arr.Count()].X - arr[n].X;
            y2 = arr[(n + 1 + arr.Count()) % arr.Count()].Y - arr[n].Y;

            return (x1*y2-y1*x2)>0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            List<PointF> first = new List<PointF>(Arr1);
            List<PointF> second = new List<PointF>(Arr2);
            List<PointF> fill = new List<PointF>();
            List<PointF> current = new List<PointF>();
            List<PointF> next = new List<PointF>();
            List<PointF> swap = new List<PointF>();
            
            int i1, j1,i2,j2,d1,d2,outs = 0,cross = 0;
            PointF w;
          
            for (int j = 0; j<pointnum2;j++)
              for (int i = 0; i < pointnum; i++)
            {
                    i1 =(pointnum  + i)%pointnum;
                    i2 =(pointnum  + i+1)%pointnum;
                    j1 =(pointnum2 + j)%pointnum2;
                    j2 =(pointnum2 + j+1)%pointnum2;

                    if (are_cross(Arr1[i1], Arr1[i2], Arr2[j1], Arr2[j2]))
                    {
                        w = intersection_point(Arr1[i1], Arr1[i2], Arr2[j1], Arr2[j2]);                        
                        w = new PointF(-w.X, w.Y);
                        d1 = (first.IndexOf(Arr1[i2]));
                        d2 = (second.IndexOf(Arr2[j2]));
                        first.Insert(d1, w);
                        second.Insert(d2, w);
                        cross++;
                    }
               }
            if (!clockwise(Arr1))
                first.Reverse();
            if (!clockwise(Arr2))
                second.Reverse();
            while (first[0].X < 0)
            {
                first.Add(first[0]);
                first.RemoveAt(0);
            }
            
            while (second[0].X < 0)
            {
                second.Add(second[0]);
                second.RemoveAt(0);
            }            
            do 
            {
                swap.Add(first[0]);
                first.RemoveAt(0);
                if(first.Count() > 0)
                    if(first[0].X>0)
                    {
                        if (swap.Count() > 2)
                            next.AddRange(sort(swap));
                        else
                            next.AddRange(swap);
                        swap.Clear();
                    }

            } while (first.Count() > 0);
            if (swap.Count() > 2)
                next.AddRange(sort(swap));
            else
                next.AddRange(swap);
            swap.Clear();
            first = new List<PointF>(next);
            next.Clear();
            do
            {
                swap.Add(second[0]);
                second.RemoveAt(0);
                if (second.Count() > 0)
                    if (second[0].X > 0)
                    {
                        if (swap.Count() > 2)
                            next.AddRange(sort(swap));
                        else
                            next.AddRange(swap);
                        swap.Clear();
                    }

            } while (second.Count() > 0);
            if (swap.Count() > 2)
                next.AddRange(sort(swap));
            else
                next.AddRange(swap);
            swap.Clear();
            second = new List<PointF>(next);
            next.Clear();
            
            int n = 0,k;
            while( n < Arr2.Count())
            {
                if (!is_inside(Arr1, Arr2[n]))
                {
                    if (cross != 0)
                    {
                        k = (second.IndexOf(Arr2[n]) + 1) % second.Count();
                        while (second[k % second.Count()].X > 0)
                            k = (k + 1) % second.Count();
                        second[k] = new PointF(second[k].X, -Math.Abs(second[k].Y));
                    }
                    outs++;
                }
                n++;
            }
            DialogResult m;
            if (cross == 0 & outs == 0)
            {
                Arr2.Add(Arr2[0]);
                PointF[] shape = new PointF[Arr2.Count()];
                Arr2.CopyTo(shape);
                G.DrawLines(new Pen(Color.Green, 5), shape);
            }
            else if (cross == 0 & outs != 0)
                m = MessageBox.Show("Обрезающий многоугольник больше обрезаемого.");
            else
            {
                PointF stpoint, temp;
                int c, l = second.Count;
                List<string> mess = new List<string>();
                for (int i = 0; i < first.Count(); i++)
                {
                    first[i] = new PointF(Math.Abs(first[i].X), first[i].Y);
                }

                for (int i = 0; i < second.Count(); i++)
                {
                    second[i] = new PointF(Math.Abs(second[i].X), second[i].Y);
                }
                while (second.Exists(item => item.Y < 0))
                {
                    c = 0;
                    while (second[c].Y > 0)
                        c++;
                    stpoint = new PointF(second[c].X, -second[c].Y);
                    fill.Add(stpoint);

                    current = second;
                    next = first;
                    do
                    {
                        c++;

                        if ((next.IndexOf(new PointF(current[c % current.Count()].X, Math.Abs(current[c % current.Count()].Y))) != -1) | (next.IndexOf(new PointF(current[c % current.Count()].X, -Math.Abs(current[c % current.Count()].Y))) != -1))
                        {
                            DrawPoint(new PointF(current[c % current.Count()].X, Math.Abs(current[c % current.Count()].Y)), new Pen(Color.Green));
                            c = next.IndexOf(new PointF(current[c % current.Count()].X, Math.Abs(current[c % current.Count()].Y))) + 1 + next.IndexOf(new PointF(current[c % current.Count()].X, -Math.Abs(current[c % current.Count()].Y)));
                            swap = next;
                            next = current;
                            current = swap;
                        }
                        current[c % current.Count()] = new PointF(current[c % current.Count()].X, Math.Abs(current[c % current.Count()].Y));//
                        fill.Add(current[c % current.Count()]);


                    } while (current[c % current.Count()] != stpoint);
                    PointF[] shape = new PointF[fill.Count()];
                    fill.CopyTo(shape);
                    G.DrawLines(new Pen(Color.Green, 5), shape);

                    fill.Clear();


                }
            }
            Arr1.Clear();
            Arr2.Clear();
            second.Clear();
            first.Clear();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
          radioButton = (RadioButton)sender;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
          radioButton = (RadioButton)sender;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Label label2 = new Label();
            TextBox textBox2 = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = "Окно ввода";
            label.Text = "1 многоугольник";
            label2.Text = "2 многоугольник";
            textBox.Text = pointnum.ToString();
            textBox2.Text = pointnum2.ToString();

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 150, 13);
            textBox.SetBounds(12, 36, 150, 20);
            label2.SetBounds(150, 20, 150, 13);
            textBox2.SetBounds(150, 36, 150, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            label2.AutoSize = true;
            textBox2.Anchor = textBox2.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, label2, textBox2, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            pointnum = Int32.Parse(textBox.Text);
            Arr1 = new List<PointF>(pointnum); // Исходный массив точек
            pointnum2 = Int32.Parse(textBox2.Text);
            Arr2 = new List<PointF>(pointnum2); // Исходный массив точек
    }
    }

}
