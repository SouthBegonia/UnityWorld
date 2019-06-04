using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExampleZig : EnemyExample
{
    //重写父类 EnemyExample 中的 虚函数Move()，用EnemyExampleZig.Move()代替
    //要想实现重写函数，父类函数必须为虚函数
    public override void Move()
    {
        Vector3 tempPos = pos;
        tempPos.x = Mathf.Sin(Time.time * Mathf.PI * 2) * 4;
        pos = tempPos;      //使用父类中定义的 pos 属性
        base.Move();    //调用父类中的 Move() 方法
    }
    //重写的Move()函数只适用于子类，父类中的虚函数不受影响
}
