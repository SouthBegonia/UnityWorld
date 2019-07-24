//字段 Fields
using System;

public class Color
{
    //静态字段
    public static readonly Color Black = new Color(0, 0, 0);
    public static readonly Color White = new Color(255, 255, 255);
    public static readonly Color Red = new Color(255, 0, 0);
    public static readonly Color Green = new Color(0, 255, 0);
    public static readonly Color Blue = new Color(0, 0, 255);

    //实例字段
    private byte r, g, b;    
    public Color(byte r, byte g, byte b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }
}

public class Test
{
    public static void Main()
    {
        Color c = new Color(0, 0, 255);

        //Console.ForegroundColor = ConsoleColor.Green;
        //Console.WriteLine("Hello, color text!");
        //Console.ForegroundColor = ConsoleColor.Red;
        //Console.WriteLine("Hello, color text!");

        Console.ReadLine();
    }
}