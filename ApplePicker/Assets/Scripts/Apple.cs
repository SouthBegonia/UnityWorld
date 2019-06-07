using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public static float bottomY = -20f;     //苹果下降底线，越线销毁

    private void Update()
    {
        /*
         此处可能存在的Bug：
         当第二个苹果还未实例化，第一个实例化的苹果就因下降越线被销毁，则场景内再无Apple预制体，
         则此时 AppleTree脚本 就失去对象applePrefab，
         也就是说后续AppleTree脚本内无法再实例化产生苹果；
         同理后续篮筐也可能带来同样的bug

        解决方案：保证场景内任意时刻存在一个applePrefab
        - 调高树的高度(本例方案)
        - 缩短实例化苹果的间隔
        - 先隐藏再延时销毁苹果(理论可行，但感觉大材小用)
         */

        //越线销毁，且判断到篮筐未接住
        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject);

            //当未接住苹果时
            //获取主摄像机的ApplePicker组件的引用
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();

            //调用apScript 的 AppleDestroyed 方法
            apScript.AppleDestroyed();
        }
    }
}
