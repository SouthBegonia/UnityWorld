using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacmanMove : MonoBehaviour
{

    //吃豆人移动速度
    public float speed = 0.35f;

    private float Magic = 0.35f;

    //吃豆人下一个目的地
    private Vector2 dest = Vector2.zero;

    private void Start()
    {
        //初始地址为自身(保证开局不会动)
        dest = this.transform.position;
    }


    private void Update()
    {
        //返回一个初始位置到目的位置的中间值，直到position等于dest结束，强行转换2维向量
        Vector2 temp = Vector2.MoveTowards(transform.position, dest, speed);

        //移动刚体到temp位置，注意此处是Rigidbody2D
        GetComponent<Rigidbody2D>().MovePosition(temp);

        //直到抵达上一个dest位置才读取新的，为了解决墙壁碰撞
        if ((Vector2)transform.position == dest)
        {
            //按键检测
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Valid(Vector2.up))
            {
                //当前位置加向上一个单位
                dest = (Vector2)transform.position + Vector2.up;
            }
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Valid(Vector2.down))
            {
                dest = (Vector2)transform.position + Vector2.down;
            }
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Valid(Vector2.left))
            {
                dest = (Vector2)transform.position + Vector2.left;
            }
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Valid(Vector2.right))
            {
                dest = (Vector2)transform.position + Vector2.right;
            }
        }
        //获取移动方向
        Vector2 dir = dest - (Vector2)transform.position;

        //把获取到的移动方向设置到状态机
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);


        if (Input.GetKeyDown(KeyCode.W))
        {
            speed += Magic;
            Magic = -Magic;
        }
    }

    //检查目的地是否合法 dir方向值(上述的Vector.XXX)
    private bool Valid(Vector2 dir)
    {
        //pos 存储当前位置(墙内的合法位置)
        Vector2 pos = transform.position;

        //从 当前值pos+方向值dir 的位置发射一条射线到Pacman 当前的位置pos
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);

        //射线打到的碰撞器 是否等于 吃豆人的碰撞器：
        //若射线从墙中心(不合法位置)射出，hit.collider为墙的，不等于Pacman的，返回fault
        //若射线从路面(合法位置)射出，hit.collider等于Pacman的，返回true
        return (hit.collider == GetComponent<Collider2D>());
    }
}
