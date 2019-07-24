//静态与实例方法
using System;

class Entity
{
    static int nextSerialNo;
    int serialNo;

    public Entity()
    {
        serialNo = nextSerialNo++;
    }
    public int GetSerialNo()
    {
        return serialNo;
    }
    //静态方法
    public static int GetNextSerialNo()
    {
        return nextSerialNo;
    }
    //静态方法
    public static void SetNextSerialNo(int value)
    {
        nextSerialNo = value;
    }
}

public class Test
{
    public static void Main()
    {
        //在类中调用静态方法
        Entity.SetNextSerialNo(1000);
        Entity e1 = new Entity();
        Entity e2 = new Entity();

        //在类实例中调用实例方法
        Console.WriteLine(e1.GetSerialNo());   
        Console.WriteLine(e2.GetSerialNo());

        //在类中调用静态方法
        Console.WriteLine(Entity.GetNextSerialNo());

        Console.ReadLine();
    }
}