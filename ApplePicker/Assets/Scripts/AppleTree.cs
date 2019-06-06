using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    public GameObject applePrefab;
    public float speed = 5f;
    public float leftAndRightEdge = 14f;
    public float chanceToChangeDirecyions = 0.4f;
    public float secondsBetweenAppleDrops = 1f;
    public int targetFrameRate = 50;

    private void Awake()
    {
        //锁定帧数
        Application.targetFrameRate = targetFrameRate;
    }

    private void Start()
    {
        //开局2s后掉落苹果，以后每 secondsBetweenAppleDrops 间隔掉落
        //其他:开局后 即便禁用该AppleTree脚本，该函数依旧会持续进行
        InvokeRepeating("DropApple", 2f, secondsBetweenAppleDrops);
    }

    private void Update()
    {
        //运动过程
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        //左右边界限定
        if (pos.x < -leftAndRightEdge)
            speed = Mathf.Abs(speed);
        else if (pos.x > leftAndRightEdge)
            speed = -Mathf.Abs(speed);
    }

    private void FixedUpdate()
    {
        //随机改变运动方向，概率为 chanceToChangeDirecyions
        if ((Random.value / Time.deltaTime) < chanceToChangeDirecyions)
            speed *= -1;
    }

    //实例化苹果并掉落
    void DropApple()
    {
        GameObject apple = Instantiate(applePrefab) as GameObject;
        Vector3 pos = transform.position;
        pos.y += 1;
        apple.transform.position = pos;

        /*想要实现 不同苹果下降速度不同，但修改 质量mass 无用
        因为 vt=v0t+gt^2/2 ......
        Rigidbody appleMash = apple.GetComponent<Rigidbody>();
        appleMash.mass = Random.Range(1f, 5f);
        */
    }
}
