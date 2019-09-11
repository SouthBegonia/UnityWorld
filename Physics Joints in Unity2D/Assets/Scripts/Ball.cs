using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Vector3 pos;

    void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        //如果球被移动到一定范围外,则将其重新移动到初始位置及状态
        if (transform.position.y < -8)
        {
            this.GetComponent<Rigidbody2D>().isKinematic = true;
            transform.position = pos;
            this.GetComponent<Rigidbody2D>().isKinematic = false;
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
