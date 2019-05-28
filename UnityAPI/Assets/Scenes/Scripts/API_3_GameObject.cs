using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API_3_GameObject : MonoBehaviour
{
    public GameObject prefab;
    public GameObject go;


    void Start()
    {
        /*创建游戏对象的方法*/
        //方法1：创建名为 Enemy 的游戏对象，用go接收
        //GameObject go =  new GameObject("Enemy");

        //方法2：根据创建、引用的prefab(但此处会出错，无限Clone产生物体)
        //GameObject go = Instantiate(prefab);

        //方法3：创建默认几个类型的游戏对象，但再没其他组件
        go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = new Vector3(0f, 2f, 0f);


        //AddComponent<>()实现添加组件或者脚本
        //go.AddComponent<API_2_Time>();

        //Destory()销毁游戏对象，延时5s执行
        //Destroy(go.GetComponent<API_2_Time>(),5f);

        //activeInHierarchy 检测对象(及其子物体、父物体)的 activeSelf 属性
        Debug.Log(go.activeInHierarchy);
        go.SetActive(false);
        Debug.Log(go.activeInHierarchy);

        //tag 游戏对象的标签
        Debug.Log(go.tag);
        
    }



}
