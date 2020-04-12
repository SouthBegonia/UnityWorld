using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Tooltip("初始时UI页面")]
    public GameObject startScreen;
    [Tooltip("UI页面从Open切换到Close的parameter")]
    public string outTrigger;

    private List<GameObject> screenHistory;     //保留全部UICanvas的表

    void Awake()
    {
        screenHistory = new List<GameObject> { startScreen };
    }

    /// <summary>
    /// 跳转到到对应UI页面的函数
    /// </summary>
    /// <param name="target">对象UI页面</param>
    public void ToScreen(GameObject target)
    {
        GameObject current = this.screenHistory[screenHistory.Count - 1];

        // 跳转页面的合法性检验
        if (target == null || target == current)
            return;

        // 执行页面跳转动画
        PlayScreen(current, target, false, screenHistory.Count);

        // 记录跳转页面的历史
        screenHistory.Add(target);
    }

    /// <summary>
    /// 返回上次的UI页面
    /// </summary>
    public void GoBack()
    {
        if (screenHistory.Count > 1)
        {
            int currentIndex = screenHistory.Count - 1;
            PlayScreen(screenHistory[currentIndex], screenHistory[currentIndex - 1], true, currentIndex - 2);
            screenHistory.RemoveAt(currentIndex);
        }
    }

    /// <summary>
    /// 播放UI页面跳转函数
    /// </summary>
    /// <param name="current">当前页面</param>
    /// <param name="target">目标页面</param>
    /// <param name="isBack">是否为返回页面</param>
    /// <param name="order">Sorting Order</param>
    private void PlayScreen(GameObject current, GameObject target, bool isBack, int order)
    {
        current.GetComponent<Animator>().SetTrigger(outTrigger);

        if (isBack)
        {
            current.GetComponent<Canvas>().sortingOrder = order;
        }
        else
        {
            current.GetComponent<Canvas>().sortingOrder = order - 1;
            target.GetComponent<Canvas>().sortingOrder = order;
        }

        target.SetActive(true);
    }
}