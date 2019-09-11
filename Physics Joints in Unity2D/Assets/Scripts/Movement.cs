using UnityEngine;
using System.Collections;

//挂在于某对象,实现在2D界面上拖拽移动脚本:
public class Movement : MonoBehaviour
{

    private bool drag;                  //物体是否被拖拽
    private Rigidbody2D rigidbody2d;    //物体的Rigidbody2D组件
    private bool wasKinematic;          //物体运动状态是否为Kinematic


    //取得组件,取得初始状态下物体
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        wasKinematic = rigidbody2d.isKinematic;
    }

    //根据是否被拖拽来刷新运动
    void Update()
    {
        if (drag == true)
            DragFunction();
    }

    void OnMouseDown()
    {
        drag = true;

        //开启刚体的isKinematic状态:刚体不受到物理的影响
        rigidbody2d.isKinematic = true;
    }

    void OnMouseUp()
    {
        if (drag == true)
            rigidbody2d.isKinematic = wasKinematic;
        drag = false;
    }

    //实现拖拽移动函数:
    void DragFunction()
    {
        float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));

        transform.position = new Vector3(pos_move.x, pos_move.y, pos_move.z);
    }

}
