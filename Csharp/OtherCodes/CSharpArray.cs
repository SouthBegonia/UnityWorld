using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;   //使用List容器需要添加该命名空间

public class CSharpArray : MonoBehaviour {

    public List<int> list = new List<int>();    //声明一个元素类型为 int 的 List 容器

	void Start () {
	    for(int i=10;i>0;i--)
        {
            list.Add(i);    //按10，9，8...1的顺序往 list 里面添加内容
        }
        list.Sort();    //排序

        foreach (int item in list) Debug.Log(item);     //打印 list 里的内容
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
