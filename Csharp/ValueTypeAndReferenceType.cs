using System;

//使用class关键字声明的类，故TestRef为引用类型
class TestRef
{
    public int a;
    public int b;

    //构造的函数对公共字段赋值
    public TestRef(int x, int y)
    {
        this.a = x;
        this.b = y;
    }
}

//使用struct关键字声明的结构，故TestVal为值类型
struct TestVal
{
    public int a;
    public int b;

    //构造的函数对公共字段赋值
    public TestVal(int x, int y)
    {
        this.a = x;
        this.b = y;
    }
}

public static class Test
{
    public static void Main()
    {
        //构造引用类型实例，其内存空间 在托管堆上 分配
        //tr的值是对TestRef类的实例的引用
        TestRef tr = new TestRef(1, 2);

        //构造值类型实例，其内存空间 在线程栈上 分配
        //tv的值就是TestVal结构的实例化本身
        TestVal tv = new TestVal(1, 2);

        //输出tr,tv的值
        System.Console.WriteLine("tr.a 的值是 " + tr.a);
        System.Console.WriteLine("tr.b 的值是 " + tr.b);
        System.Console.WriteLine("tv.a 的值是 " + tv.a);
        System.Console.WriteLine("tv.b 的值是 " + tv.b);

        //引用类型的赋值：将tr的值所指向的对象地址复制给tr2
        //此时tr和tr2 指向同一个对象，并不需要在托管堆上重新分配空间
        TestRef tr2 = tr;

        //值类型的赋值：在线程上重新分配空间，并将tv的值复制给tv2
        //此后tv和tv2相互独立
        TestVal tv2 = tv;

        //tr2所指对象的字段赋值，会改变tr
        tr2.a = 3;

        //tv2字段的修改不影响tv
        tv2.a = 3;

        System.Console.WriteLine("---------------------");
        System.Console.WriteLine("进行 tr2.a=3  tv2.a=3 后：");
        System.Console.WriteLine("tr.a 的值是 " + tr.a);
        System.Console.WriteLine("tr.b 的值是 " + tr.b);
        System.Console.WriteLine("tr2.a 的值是 " + tr2.a);
        System.Console.WriteLine("tr2.b 的值是 " + tr2.b);
        System.Console.WriteLine("tv.a 的值是 " + tv.a);
        System.Console.WriteLine("tv.b 的值是 " + tv.b);
        System.Console.WriteLine("tv2.a 的值是 " + tv2.a);
        System.Console.WriteLine("tv2.b 的值是 " + tv2.b);

        Console.ReadLine();
    }
}