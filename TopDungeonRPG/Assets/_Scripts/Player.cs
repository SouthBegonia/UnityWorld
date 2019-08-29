using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    private BoxCollider2D PlayerCollider;       //Player的碰撞器
    private Vector3 moveDelta;                  //
    private RaycastHit2D hit; 

    private void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        //获取移动值 
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveDelta = new Vector3(x, y, 0);

        //变更方向:正向/逆向
        if (moveDelta.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //定向移动:上下左右
        //当物件处在Blocking层且包含BoxCollider2D时,不可越(wall)
        hit = Physics2D.BoxCast(transform.position, PlayerCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        //Debug.Log("Y :" + hit);
        hit = Physics2D.BoxCast(transform.position, PlayerCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        //Debug.Log("X :" + hit);
    }
}
