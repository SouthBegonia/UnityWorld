using System;

class ArrayExample
{
    public static void Main()
    {
        int[] a = new int[10];

        for (int i = 0; i < a.Length; i++)
            a[i] = i * i;

        for (int i = 0; i < a.Length; i++)
        {
            //Console.WriteLine($"a[{i}] = {a[i]}");
            Console.WriteLine(string.Format("a[{0}] = {1}",i,a[i]));
        }

        Console.ReadLine();
    }
}