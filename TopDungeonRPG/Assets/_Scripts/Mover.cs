using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    private BoxCollider2D BoxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;

    protected float ySpeed = 0.75f;         //纵向速度比例
    protected float xSpeed = 1.0f;          //横向速度比例


    protected virtual void Start()
    {
        BoxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //变更方向:正向/逆向
        if (moveDelta.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //推力移动
        moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        //定向移动:上下左右
        //当物件处在Blocking层且包含BoxCollider2D时,不可越(wall)
        hit = Physics2D.BoxCast(transform.position, BoxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);

        hit = Physics2D.BoxCast(transform.position, BoxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);

    }
}
