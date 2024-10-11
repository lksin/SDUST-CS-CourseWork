using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace ex
{
    public partial class Form1 : Form
    {
        Matrix A = new Matrix();
        Matrix B = new Matrix();

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        { 

            A.Read(textBox2.Text, "A");
            B.Read(textBox3.Text, "B");
            Print_Matrix(A.Add(B));
        }
        private void button2_Click(object sender, EventArgs e)
        {
            A.Read(textBox2.Text, "A");
            B.Read(textBox3.Text, "B");
            Print_Matrix(A.Sub(B));
        }
        private void button3_Click(object sender, EventArgs e)
        {
            A.Read(textBox2.Text, "A");
            B.Read(textBox3.Text, "B");
            Print_Matrix(A.Mul(B));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            A.Read(textBox2.Text, "A");
            B.Read(textBox3.Text, "B");
            Print_Matrix(A.Trans());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            A.Read(textBox2.Text, "A");
            B.Read(textBox3.Text, "B");
            Print_Matrix(A.Inverse());
        }
        public void Print_Matrix(Matrix matrix) //输出
        {
            if(matrix == null)
            {
                MessageBox.Show("请重新输入矩阵", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }
            textBox1.Clear();
            double eps = 1e-10;
            for (int i = 0; i < matrix.Rows; i++)
            {
                textBox1.Text += "|";
                for (int j = 0; j < matrix.Columns; j++)
                {
                    if (matrix.data[i, j] - Math.Floor(matrix.data[i, j]) < eps)
                    {
                        textBox1.Text += matrix.data[i, j] + "    ";
                    }
                    else
                    {
                        textBox1.Text += matrix.data[i, j].ToString("N2") + " ";
                    }
                }
                textBox1.Text += "|\r\n";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/lksin/SDUST-CS-CourseWork/");
        }
    }
    public class Matrix
    {
        public double[,] data;
        public Matrix(double[,] data) //直接从二维数组获取数据
        {
            this.data = data;
        }
        public Matrix() { }
        public int Rows
        {
            get { return data.GetLength(0); }
        }
        public int Columns
        {
            get { return data.GetLength(1); }
        }
       

        private double deterMinant(double[,] array) //求矩阵行列式
        {
            int length = array.GetLength(0);
            if (length == 1)
            {
                return (array[0, 0]);
            }
            else if (length == 2)
            {
                return (array[0, 0] * array[1, 1] - array[0, 1] * array[1, 0]);
            }
            double det = 0;
            for(int i = 0;i < length; i++)
            {
                double[,] minor = getMinor(array, 0, i);
                if(i % 2 == 0)
                {
                    det += deterMinant(minor) * array[0, i];
                }
                else
                {
                    det -= deterMinant(minor) * array[0, i];
                }
            }
            return det;
        }

        private double[,] getMinor(double[,] array, int xPos, int yPos) //获取余子式
        {
            int length = array.GetLength(0);
            double[,] minor = new double[length - 1, length - 1];
            int mx = 0;
            for(int x = 0; x < length; x++)
            {
                if (x == xPos)
                {
                    continue;
                }
                int my = 0;
                for(int y = 0; y < length; y++)
                {
                    if(y == yPos)
                    {
                        continue;
                    }
                    minor[mx, my] = array[x, y];
                    my++;
                }
                mx++;
            }
            return minor;
        }
        public double Cofactor(double[,] matrix, int r, int c) //求出代数余子式
        {
            double[,] array = getMinor(matrix, r, c);
            double det = deterMinant(array);
            if ((r + c) % 2 == 0)
            {
                return det;
            }
            else return -det;
        }
        public Matrix Add(Matrix other) //加法
        {
            if (this.Rows != other.Rows || this.Columns != other.Columns)
            {
                MessageBox.Show("无法对两个不同的矩阵进行加法运算");
                return null;
                //throw new ArgumentException("两个矩阵不匹配");
            }
            double[,] results = new double[this.Rows, this.Columns];
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    results[i, j] = this.data[i, j] + other.data[i, j];
                }
            }
            return new Matrix(results);
        }
        public Matrix Sub(Matrix other) //减法
        {
            if (this.Rows != other.Rows || this.Columns != other.Columns)
            {
                MessageBox.Show("无法对两个不同的矩阵进行加法运算");
                return null;
            }
            double[,] results = new double[this.Rows, this.Columns];
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    results[i, j] = this.data[i, j] - other.data[i, j];
                }
            }
            return new Matrix(results);
        }
        public Matrix Mul(Matrix other) //乘法
        {
            if (this.Columns != other.Rows) //矩阵乘法的条件
            {
                MessageBox.Show("不能两个矩阵进行乘法操作", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            double[,] results = new double[this.Rows, other.Columns];
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < other.Columns; j++)
                {
                    results[i, j] = 0;
                    for (int k = 0; k < this.Columns; k++)
                    {
                        results[i, j] += this.data[i, k] * other.data[k, j];
                    }
                }
            }
            return new Matrix (results);
        }
        public Matrix Trans() //转置  bugs: error when matrix c!=r
        {
            double[,] results = new double[this.Columns, this.Rows];
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    results[j, i] = this.data[i, j];
                }
            }
            return new Matrix(results);
        }
        public Matrix Adjoint() //生成伴随矩阵
        {
            if(this.Rows != this.Columns)
            {
                MessageBox.Show("矩阵不是方阵", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            double[,] tempArray = new double[this.Rows, this.Columns];
            for(int i = 0; i < this.Rows; i++)
            {
                for(int j = 0; j < this.Columns; j++)
                {
                    tempArray[i, j] = Cofactor(this.data, i, j);
                }
            }
            Matrix adjoint = new Matrix(tempArray);
            return adjoint.Trans();
        }
        public Matrix Inverse() //求逆
        {
            if(this.Columns != this.Rows)
            {
                MessageBox.Show("矩阵不是方阵", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            if(this.Rows == 2)
            {
                double det = this.data[0, 0] * this.data[1, 1] - this.data[0, 1] * this.data[1, 0]; //求行列式
                if (det <= 0)
                {
                    MessageBox.Show("该矩阵不存在逆矩阵", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                double[,] results = new double[2, 2];
                results[0, 0] = this.data[1, 1] / det;
                results[1, 1] = this.data[0, 0] / det;
                results[0, 1] = -this.data[0, 1] / det;
                results[1, 0] = -this.data[1, 0] / det;
                return new Matrix (results);
            }
            else
            {
                double[,] results = new double[this.Rows, this.Columns];
                //求伴随矩阵
                double[,] adjoint = Adjoint().data;
                //求矩阵行列式
                if(deterMinant(this.data) <= 0 )
                {
                    MessageBox.Show("该矩阵不存在逆矩阵", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                double det = 1 / deterMinant(this.data);
                for(int i = 0; i < this.Rows; i++)
                {
                    for(int j = 0; j < this.Columns; j++)
                    {
                        results[i, j] = adjoint[i, j] * det;
                    }
                }
                return new Matrix (results);
            }
        }

        public void Read(string input, string id)
        {
            if (input == "")
            {
                MessageBox.Show($"矩阵{id}是空的，请重新输入", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                data = new double[0, 0];
                return;
            }
            try
            {
                // 先按行拆分矩阵
                string[] rows = input.Trim().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                int Rows = rows.Length;  // 行数

                // 按空格拆分第一行，得到列数
                string[] firstRow = rows[0].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int Columns = firstRow.Length;  // 列数

                // 初始化矩阵
                data = new double[Rows, Columns];

                // 填充矩阵数据
                for (int i = 0; i < Rows; i++)
                {
                    // 按空格拆分每行
                    string[] values = rows[i].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length != Columns)
                    {
                        MessageBox.Show("每行列数不一致", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    for (int j = 0; j < Columns; j++)
                    {
                        data[i, j] = Convert.ToDouble(values[j]);
                    }
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("读取矩阵失败: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
    public class Regn //判断输入是否为数字
    {
        public bool regn(string input)
        {
            Regex reg = new Regex("^-?\\d+$|^(-?\\d+)(\\.\\d+)?$");
            return reg.IsMatch(input);
        }
    }
}
