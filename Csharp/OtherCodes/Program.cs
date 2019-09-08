using System;

namespace Csharp
{
    /*
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int num;
            num = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("num = {0}", num);
            Console.ReadKey();
        }
    }
    */

    /* 基本语法
    class Rectangle
    {
        double lenght;  //Rectangle 类的成员变量
        double width;   //Rectangle 类的成员变量

        public void Accetdetails()  //Rectangle 类的成员函数
        {
            lenght = 4.5;
            width = 1.5;
        }

        public double GetArea()
        {
            return lenght * width;
        }

        public void Display()
        {
            Console.WriteLine("length: {0}", lenght);
            Console.WriteLine("width: {0}", width);
            Console.WriteLine("Area: {0}", GetArea());
        }
    }

    class ExecuteRectangle
    {
        static void Main(string[] args)
        {
            Rectangle r = new Rectangle();
            r.Accetdetails();
            r.Display();
            Console.ReadLine();
        }
    }
    */

    /* 数据类型
    class DataType
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Size of double: {0}", sizeof(double));
            Console.WriteLine("Size of int: {0}", sizeof(int));
            Console.WriteLine("Size of bool: {0}", sizeof(bool));

            //对象Object类型：所有数据类型的基类
            object obj;
            obj = 12;

            //动态Dynamic类型：可以储存任何类型的值在动态数据类型变量内
            dynamic d = 10;

            //字符串String类型：
            string str = "hello";
            Console.WriteLine(str);

            //指针类型
            //char* cptr;
            //int* iptr;

            Console.ReadLine();
        }
    }
    */

    /*类型转换
    class ExplicitiConversion
    {
        static void Main(string[] args)
        {
            double d = 3.14;
            int i;

            i = (int)d;
            Console.WriteLine(i);
            
            int i2 = 12;
            float f = 1.41f;
            double d2 = 3.14f;
            bool b = false;
            Console.WriteLine(i2.ToString());
            Console.WriteLine(f.ToString());
            Console.WriteLine(d2.ToString());
            Console.WriteLine(b.ToString());
            Console.ReadKey();
        }
    }
    */

    /*定义常量及使用
    public class ConstTest
    {
        class SampleClass
        {
            public int x, y;
            public const int c1 = 5, c2 = 10;

            public SampleClass(int p1, int p2)
            {
                x = p1;
                y = p2;
            }
        }

        static void Main()
        {
            SampleClass mc = new SampleClass(11, 22);
            Console.WriteLine("x = {0}, y = {1}", mc.x, mc.y);
            Console.WriteLine("c1 = {0}, c2 = {1}", SampleClass.c1, SampleClass.c2);
            Console.ReadKey();
        }
    }
    */

}
