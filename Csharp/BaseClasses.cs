//基类
using System;

public class Point
{
    public int x, y;
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

//继承是指隐式包含其基类的所有成员的类，实例和静态构造函数以及基类的终结器除外
//派生类可以其继承的类添加新成员，但无法删除继承成员的定义
public class Point3D : Point
{
    public int z;
    public Point3D(int x, int y, int z) : base(x, y)
    {
        this.z = z;
    }
}

public class Test
{
    public static void Main()
    {
        //为新实例分配内存，调用构造函数来初始化实例，并返回对实例的引用
        Point a = new Point(10, 20);
        Point b = new Point3D(30, 40, 50);
        Point3D c = new Point3D(30, 40, 50);

        Console.WriteLine(a.x + " " + a.y);
        Console.WriteLine(b.x + " " + b.y);
        Console.WriteLine(c.x + " " + c.y + " " + c.z);

        Console.ReadLine();
    }
}