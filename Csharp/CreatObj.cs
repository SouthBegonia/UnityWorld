//在移动端使用Input类中的触摸操作
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatObj : MonoBehaviour {

    public GameObject player;   //定义玩家对象
    public float num = 0;      //定义实例化的玩家数量

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		for(int i=0;i<Input.touchCount;i++)     //遍历当前触摸的
        {
            //判断当前状态是否为刚开始触摸屏幕
            if(Input.GetTouch(i).phase == TouchPhase.Began)
            {
                //从手指的方向发射一条射线
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit ,100))
                {
                    //如果碰撞到的物体名字为Plane
                    if (hit.collider.name == "Plane") ;
                    {
                        //在碰撞点位置实例化一个player对象
                        Instantiate(player, hit.point, player.transform.rotation);
                        num++;
                    }
                }
            }
        }
	}
    private void OnGUI()
    {
        //在屏幕上显示实例化玩家数量
        GUILayout.Label("共实例化玩家 " + num);
    }
}
