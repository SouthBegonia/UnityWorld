using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class A_example : MonoBehaviour
{
    public GameObject game;
    public Vector2 movingValue;

    //public Transform cube;

    // Use this for initialization

    void Start()
    {
        movingValue = game.transform.position;

        //参数解释：获取movingValue的值，设置变化的值给movingValue
        //设置目标值为（1，1，1）,设置动画时间 1 秒

        DOTween.To(() => movingValue, x => movingValue = x, new Vector2(10, 10), 1);

    }

    void Update()
    {

        //把变化的值设置给cube，让Cube随之移动

        game.transform.position = movingValue;

    }
}
