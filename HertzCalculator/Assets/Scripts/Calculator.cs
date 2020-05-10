using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    [Header("滚动轴承参数InputField")]
    public InputField Dw;
    public InputField DD;
    public InputField dd;
    public InputField Z;
    public InputField ea;
    public InputField eb;

    public InputField fi;
    public InputField fe;

    [Space]
    [Header("下拉选框选择已有数据")]
    public Dropdown dropdown;

    [Space]
    [Header("结果Text")]
    public Text ans_内环_曲率和;
    public Text ans_内环_曲率差;
    public Text ans_外环_曲率和;
    public Text ans_外环_曲率差;

    public struct 环
    {
        public decimal 曲率和;
        public decimal 曲率差;
        public decimal a;
        public decimal b;

        public decimal d;
        public decimal r;
    };

    public 环 内环 = new 环();
    public 环 外环 = new 环();

    private decimal Num_Dw;     //滚子直径
    private decimal Num_D;      //外环直径
    private decimal Num_d;      //内环直径
    private decimal Num_Z;      //滚子数
    private decimal Num_ea;     //接触系数（根据曲率和查表
    private decimal Num_eb;     //接触系数（根据曲率和查表
    private decimal Num_fi;     //求解内圈曲率半径参数（0.51~0.60
    private decimal Num_fe;     //求解外圈曲率半径参数（0.51~0.60

    public struct TestData      //测试数据结构
    {
        public string index;
        public string Num_Dw;
        public string Num_D;
        public string Num_d;
        public string Num_Z;
        public string Num_ea;
        public string Num_eb;
        public string Num_fi;
        public string Num_fe;
    }


    private List<TestData> testDatas = new List<TestData>();//测试数据的表

    void Start()
    {
        //初始化，并创建默认可选择的测试数据
        GenerateDefaultData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CheckData();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetNumbers(dropdown.value);
            Debug.Log("设置测试数据完成");
        }
    }

    /// <summary>
    /// 初始化，制作已有测试数据表
    /// </summary>
    public void GenerateDefaultData()
    {
        //标号 61801滚动轴承参数
        TestData Index_61801 = new TestData
        {
            index = "61801",
            Num_Dw = "2.381",
            Num_D = "21",
            Num_d = "12",
            Num_Z = "12",
            Num_ea = "0.10563",
            Num_eb = "0.00898",
            Num_fi = "0.515",
            Num_fe = "0.525",
        };
        testDatas.Add(Index_61801);

        TestData Index_61901 = new TestData
        {
            index = "61901",
            Num_Dw = "3.175",
            Num_D = "24",
            Num_d = "12",
            Num_Z = "10",
            Num_ea = "0.05189",
            Num_eb = "0.01319",
            Num_fi = "0.515",
            Num_fe = "0.525",
        };
        testDatas.Add(Index_61901);

        //下拉选框的创建
        dropdown.ClearOptions();
        Dropdown.OptionData option;
        for (int i = 0; i < testDatas.Count; i++)
        {
            option = new Dropdown.OptionData();
            option.text = testDatas[i].index;
            dropdown.options.Add(option);
        }
        dropdown.captionText.text = testDatas[0].index;
    }

    /// <summary>
    /// 设置测试数据到UI页面
    /// </summary>
    public void SetNumbers(int index)
    {
        Dw.text = testDatas[index].Num_Dw;
        DD.text = testDatas[index].Num_D;
        dd.text = testDatas[index].Num_d;
        Z.text = testDatas[index].Num_Z;
        ea.text = testDatas[index].Num_ea;
        eb.text = testDatas[index].Num_eb;
        fi.text = testDatas[index].Num_fi;
        fe.text = testDatas[index].Num_fe;
    }

    /// <summary>
    /// 从UI输入页面取得数值
    /// </summary>
    public void GetNumbers()
    {
        Num_Dw = Convert.ToDecimal(Dw.text);
        Num_D = Convert.ToDecimal(DD.text);
        Num_d = Convert.ToDecimal(dd.text);
        Num_Z = Convert.ToDecimal(Z.text);
        Num_ea = Convert.ToDecimal(ea.text);
        Num_eb = Convert.ToDecimal(eb.text);
        Num_fi = Convert.ToDecimal(0.515f);
        Num_fe = Convert.ToDecimal(0.525f);
    }

    /// <summary>
    /// 核心计算函数
    /// </summary>
    public void Calculate()
    {
        GetNumbers();
        内环.d = ((Num_D + Num_d) / (decimal)2.0 - Num_Dw);
        内环.r = decimal.Round((Num_fi * Num_Dw), 2, MidpointRounding.AwayFromZero);
        内环.曲率和 = (4 / Num_Dw) + (2 / 内环.d) - (1 / 内环.r);
        内环.曲率差 = ((1 / 内环.r) + (2 / 内环.d)) / 内环.曲率和;


        外环.d = ((Num_D + Num_d) / 2 + Num_Dw);
        外环.r = decimal.Round((Num_fe * Num_Dw), 2, MidpointRounding.AwayFromZero);
        外环.曲率和 = (4 / Num_Dw) - (2 / 外环.d) - (1 / 外环.r);
        外环.曲率差 = ((1 / 外环.r) - (2 / 外环.d)) / 外环.曲率和;

        Refresh();
    }

    /// <summary>
    /// 刷新求解结果的UI
    /// </summary>
    public void Refresh()
    {
        //四舍五入保留4位小数
        内环.曲率和 = decimal.Round(内环.曲率和, 5, MidpointRounding.AwayFromZero);
        内环.曲率差 = decimal.Round(内环.曲率差, 5, MidpointRounding.AwayFromZero);
        ans_内环_曲率和.text = 内环.曲率和.ToString("F5");
        ans_内环_曲率差.text = 内环.曲率差.ToString("F5");

        外环.曲率和 = decimal.Round(外环.曲率和, 5, MidpointRounding.AwayFromZero);
        外环.曲率差 = decimal.Round(外环.曲率差, 5, MidpointRounding.AwayFromZero);
        ans_外环_曲率和.text = 外环.曲率和.ToString("F5");
        ans_外环_曲率差.text = 外环.曲率差.ToString("F5");
    }

    /// <summary>
    /// Debug相关
    /// </summary>
    public void CheckData()
    {
        Debug.Log($"内环di = {内环.d}\n内环ri = {内环.r}\n内环曲率和 = {内环.曲率和}\n内环曲率差 = {内环.曲率差}");
        Debug.Log($"外环di = {外环.d}\n外环ri = {外环.r}\n外环曲率和 = {外环.曲率和}\n外环曲率差 = {外环.曲率差}");
    }
}
