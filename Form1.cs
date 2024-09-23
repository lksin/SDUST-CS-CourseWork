using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ex
{
    public partial class Form1 : Form
    {
        double[,] matrixA = null;
        double[,] matrixB = null;
        Matrix A = new Matrix();
        Matrix B = new Matrix();

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //add
        { 
            A.Read(textBox2.Text);
            B.Read(textBox3.Text);
            Print_Matrix(A.Add(B));
        }
        private void button2_Click(object sender, EventArgs e)
        {
            A.Read(textBox2.Text);
            B.Read(textBox3.Text);
            Print_Matrix(A.Sub(B));
        }
        public void Print_Matrix(Matrix matrix) //输出
        {
            textBox1.Clear();
            for (int i = 0; i < matrix.Rows; i++)
            {
                textBox1.Text += "|";
                for (int j = 0; j < matrix.Columns; j++)
                {
                    textBox1.Text += matrix.data[i, j] + " ";
                }
                textBox1.Text += "|\r\n";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            A.Read(textBox2.Text);
            B.Read(textBox3.Text);
            Print_Matrix(A.Mul(B));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            A.Read(textBox2.Text);
            B.Read(textBox3.Text);
            Print_Matrix(A.Trans());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            A.Read(textBox2.Text);
            B.Read(textBox3.Text);
            Print_Matrix(A.Inverse());
        }
    }
    public class Matrix
    {
        public Matrix() {}

        public double[,] data;
        public int Rows;
        public int Columns;

        public Matrix Add(Matrix other) //加法
        {
            if (this.Rows != other.Rows || this.Columns != other.Columns)
            {
                throw new ArgumentException("两个矩阵不匹配");
            }
            double[,] results = new double[this.Rows, this.Columns];
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    results[i, j] = this.data[i, j] + other.data[i, j];
                }
            }
            return new Matrix { data = results, Rows = this.Rows, Columns = this.Columns};
        }
        public Matrix Sub(Matrix other) //减法
        {
            if (this.Rows != other.Rows || this.Columns != other.Columns)
            {
                throw new ArgumentException("两个矩阵不匹配");
            }
            double[,] results = new double[this.Rows, this.Columns];
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    results[i, j] = this.data[i, j] - other.data[i, j];
                }
            }
            return new Matrix { data = results, Rows = this.Rows, Columns = this.Columns };
        }
        public Matrix Mul(Matrix other) //乘法
        {
            if (this.Columns != other.Rows) //矩阵乘法的条件
            {
                throw new ArgumentException("两个矩阵不匹配");
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
            return new Matrix { data = results, Rows = this.Rows, Columns = this.Columns };
        }
        public Matrix Trans() //转置
        {
            double[,] results = new double[this.Columns, this.Rows];
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    results[j, i] = this.data[i, j];
                }
            }
            return new Matrix{data = results, Rows = this.Rows, Columns = this.Columns };
        }
        public Matrix Inverse() //求逆
        {
            if (this.Rows != 2 || this.Columns != 2)
            {
                //throw new ArgumentException("矩阵不是2*2");

            }
            double det = this.data[0, 0] * this.data[1, 1] - this.data[0, 1] * this.data[1, 0]; //求行列式
            if (det == 0)
            {
                throw new ArgumentException("矩阵不可逆");
            }
            double[,] results = new double[2, 2];
            results[0, 0] = this.data[1, 1] / det;
            results[1, 1] = this.data[0, 0] / det;
            results[0, 1] = -this.data[0, 1] / det;
            results[1, 0] = -this.data[1, 0] / det;

            return new Matrix{data = results, Rows = this.Rows, Columns = this.Columns };
        }

        public void Read(string input)
        {
            try
            {
                string[] str = input.Trim().Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                // 检查输入的第一部分是否可以转为行数和列数
                Rows = Convert.ToInt32(str[0]);
                Columns = Convert.ToInt32(str[1]);

                // 初始化矩阵数据
                data = new double[Rows, Columns];

                // 填充矩阵数据，从输入的第三个元素开始（跳过行数和列数）
                int index = 2;
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        data[i, j] = Convert.ToDouble(str[index]);
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取矩阵失败: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
