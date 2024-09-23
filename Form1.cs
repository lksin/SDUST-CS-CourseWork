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

namespace ex
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //add
        {

        }
    }
    public class Matrix
    {
        private double[,] data;

        public Matrix(double[,] data) //构造函数
        {
            this.data = data;
        }
        public int Rows //行数
        {
            get { return data.GetLength(0); }
        }
        public int Columns //列数
        {
            get { return data.GetLength(1); }
        }
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
            return new Matrix(results);
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
            return new Matrix(results);
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
            return new Matrix(results);
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
            return new Matrix(results);
        }
        public Matrix Inverse() //求逆
        {
            if (this.Rows != 2 || this.Columns != 2)
            {
                throw new ArgumentException("矩阵不是2*2");
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

            return new Matrix(results);
        }
        public void Print_Martrix() //输出
        {
            for (int i = 0; i < this.Rows; i++)
            {
                Console.Write("|");
                for (int j = 0; j < this.Columns; j++)
                {
                    Console.Write(this.data[i, j] + " ");
                }
                Console.Write("|");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static double[] Read(string input)
        {
            Regn regn = new Regn();
            double[,] matrix;
            string[] str = input.Split(' ');
            for(int i)
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
