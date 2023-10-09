using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
       int pnum = 0;
       int pointnum = 3;
       public Point[] Arr = new Point[3];
        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseClick += new MouseEventHandler(Form1_MouseClick);
        }
        Graphics G; // Создаем бъект графики


        void Form1_MouseClick(object sender, MouseEventArgs e)//добавление точки по нажатию мыши
        {
            Point w = new Point(e.X, e.Y);
            Arr[pnum] = new Point(e.X, e.Y);
            if (pnum == 0)
            {
                G = Graphics.FromHwnd(pictureBox1.Handle);
                G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                G.Clear(Color.White);
            }
            pnum++;
            G = Graphics.FromHwnd(pictureBox1.Handle);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawPoint(w);
            if (pnum == pointnum)
                pnum = 0;
        }

        private void button1_Click(object sender, EventArgs e)//кнопка рисования
        {
            G = Graphics.FromHwnd(pictureBox1.Handle);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Draw();
        }
        private void button2_Click(object sender, EventArgs e)//кнопка ввода кол-ва точек
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = "Окно ввода";
            label.Text = "Введите количество точек";
            textBox.Text = pointnum.ToString();

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            pointnum = Int32.Parse(textBox.Text);
            Arr = new Point[pointnum]; // Исходный массив точек
        }
        private void button3_Click(object sender, EventArgs e)//кнопка ручного ввода точек
        {
            Form form = new Form();
            Label[] label = new Label[pointnum * 2];


            for (int i = 0; i < pointnum * 2; i++)
            {
                label[i] = new Label();
            }
            TextBox[] textBox = new TextBox[pointnum * 2];
            for (int i = 0; i < pointnum * 2; i++)
            {
                textBox[i] = new TextBox();
            }

            form.Text = "Окно ввода";
            for (int i = 0; i < pointnum * 2; i += 2)
            {
                label[i].Text = "X" + (i / 2).ToString();
                textBox[i].Text = Arr[(int)(i / 2)].X.ToString();
                label[i + 1].Text = "Y" + (i / 2).ToString();
                textBox[i + 1].Text = Arr[(int)(i / 2)].Y.ToString();
            }
            for (int i = 0; i < pointnum * 2; i += 2)
            {
                label[i].SetBounds(10, 10 * (i + 1) + 5, 10, 13);
                label[i + 1].SetBounds(190, 10 * (i + 1) + 5, 10, 13);
                textBox[i].SetBounds(50, 10 * (i + 1), 50, 20);
                textBox[i + 1].SetBounds(230, 10 * (i + 1), 50, 20);
                label[i].AutoSize = true;
                label[i + 1].AutoSize = true;
            }

            form.ClientSize = new Size(10, 20 * (pointnum + 3));
            for (int i = 0; i < pointnum * 2; i++)
            {
                form.Controls.AddRange(new Control[] { label[i], textBox[i] });
                form.ClientSize = new Size(Math.Max(300, label[i].Right + 10), form.ClientSize.Height);
            }
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;


            DialogResult dialogResult = form.ShowDialog();
            for (int i = 0; i < pointnum * 2; i += 2)
            {
                Arr[i / 2].X = Int32.Parse(textBox[i].Text);
                Arr[i / 2].Y = Int32.Parse(textBox[i + 1].Text);
            }
            G = Graphics.FromHwnd(pictureBox1.Handle);
            G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            G.Clear(Color.White);
            for (int i = 0; i < pointnum; i++)
            {
                Point w = new Point(Arr[i].X, Arr[i].Y);
                DrawPoint(w);
            }
        }
        void DrawPoint(Point e)
        {
            Point[] Pt = new Point[]
                {
                new Point(e.X+1,e.Y-1),
                new Point(e.X-1, e.Y+1),
                new Point(e.X-1, e.Y - 1),
                new Point(e.X+1, e.Y + 1)
                };
            G.DrawLines(new Pen(Color.Red), Pt);

        }



        int Fuc(int n) // Функция вычисления факториала
        {            
            int fuc;
            if (n > 0)
                fuc = Fuc(n - 1) * n;
            else
                fuc = 1;
            return fuc;
        }
        float N(int i, int n, float t)// Вычисление N
        {
            int a = Fuc(n) / (Fuc(i) * Fuc(n - i));
            float b = (float)Math.Pow(t, i);
            float c = (float)Math.Pow(1 - t, n - i);
            return a * b *c ;
        }
        void Draw()// Функция рисования кривой
        {
            int j = 0;
            float step = 0.01f;// задающий параметр

            PointF[] P = new PointF[101];//Конечный массив точек кривой
            for (float t = 0; t < 1; t += step)
            {
                float y = 0;
                float x = 0;
                for (int i = 0; i < Arr.Length; i++)
                {
                    x = x + Arr[i].X * N(i, Arr.Length - 1, t); // произведение веса координаты и весового коэффициента
                    y = y + Arr[i].Y * N(i, Arr.Length - 1, t);
                }
                P[j] = new PointF(x, y);
                j++;

            }
            G.DrawLines(new Pen(Color.Black), P);// Рисуем полученную кривую Безье
        }

       
    }
   
}
