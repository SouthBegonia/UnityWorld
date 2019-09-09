using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Colliderable
{
    //各等级武器伤害参数:伤害,推力
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7 };
    public float[] pushForce = { 2.0f, 2.2f, 2.5f, 3.0f, 3.3f, 3.6f, 4.0f };

    //武器等级参数:武器等级,武器sprite
    public int weaponLevel = 0;
    private SpriteRenderer SpriteRenderer;

    //武器控制参数:动画控制,冷却时间
    private Animator animator;
    private float coolDown = 0.5f;
    private float lastSwing;


    protected override void Start()
    {
        base.Start();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        //读取输入,空格键实现普攻
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSwing > coolDown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    //检验武器碰撞到的物体
    protected override void OnCollide(Collider2D coll)
    {
        //必须为可被伤害类物体才可进行伤害检验
        if (coll.tag == "Fighter")
        {
            //武器不可伤害到玩家
            if (coll.name == "Player")
                return;

            //其余即为敌人
            Damag dmg = new Damag
            {
                //敌人受到的伤害/击退距离由各等级的武器决定
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            //发送消息给被碰撞物体,调用其接受伤害函数(Fighter类内)
            coll.SendMessage("ReceiveDamage", dmg);
        } 
    }

    //设置Animator状态函数:武器挥动swing
    private void Swing()
    {       
        animator.SetTrigger("Swing");
    }

    //升级武器
    public void UpgradeWeapon()
    {
        //等级提升,sprite变更
        weaponLevel++;
        SpriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    //设置武器等级
    public void SetWeaponLevel(int level)
    {
        weaponLevel = GameManager.instance.weapon.weaponLevel;
    }
}
