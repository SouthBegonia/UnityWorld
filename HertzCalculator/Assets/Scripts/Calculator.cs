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
    public InputField 内环ea;
    public InputField 内环eb;
    public InputField 外环ea;
    public InputField 外环eb;

    public InputField fi;
    public InputField fe;

    public InputField fr;
    public InputField es;

    [Space]
    [Header("下拉选框选择已有数据")]
    public Dropdown dropdown;

    [Space]
    [Header("结果Text")]
    public Text ans_内环_曲率和;
    public Text ans_内环_曲率差;
    public Text ans_外环_曲率和;
    public Text ans_外环_曲率差;
    public Text ans_Qmax;
    public Text ans_内环_a;
    public Text ans_内环_b;
    public Text ans_外环_a;
    public Text ans_外环_b;
    public Text ans_内环_Pmax;
    public Text ans_内环_趋近量;
    public Text ans_外环_Pmax;
    public Text ans_外环_趋近量;

    public struct 环
    {
        public decimal 曲率和;
        public decimal 曲率差;

        public decimal d;
        public decimal r;

        public decimal a;
        public decimal b;

        public decimal ea;
        public decimal eb;

        public decimal Pmax;
        public decimal 趋近量;
    };

    public 环 内环 = new 环();
    public 环 外环 = new 环();

    private decimal Num_Dw;     //滚子直径
    private decimal Num_D;      //外环直径
    private decimal Num_d;      //内环直径
    private decimal Num_Z;      //滚子数
    private decimal Num_内环ea;     //接触系数（根据曲率和查表
    private decimal Num_内环eb;     //接触系数（根据曲率和查表
    private decimal Num_外环ea;     //接触系数（根据曲率和查表
    private decimal Num_外环eb;     //接触系数（根据曲率和查表
    private decimal Num_fi;     //求解内圈曲率半径参数（0.51~0.60
    private decimal Num_fe;     //求解外圈曲率半径参数（0.51~0.60
    private decimal Num_fr;     //施加的载荷（单位N
    private decimal Num_es;     //求解弹性趋近量的应变系数（单位10^-4
    private decimal Num_Qmax;   //最大接触载荷

    private decimal Pi = (decimal)Mathf.PI;

    public struct TestData      //测试数据结构
    {
        public string index;
        public string Num_Dw;
        public string Num_D;
        public string Num_d;
        public string Num_Z;
        public string Num_内环ea;
        public string Num_内环eb;
        public string Num_外环ea;
        public string Num_外环eb;
        public string Num_fi;
        public string Num_fe;
        public string Num_fr;
        public string Num_es;
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
            Num_内环ea = "0.10563",
            Num_内环eb = "0.00898",
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
            Num_内环ea = "0.05189",
            Num_内环eb = "0.01319",
            Num_fi = "0.515",
            Num_fe = "0.525",
        };
        testDatas.Add(Index_61901);

        TestData Index_6206 = new TestData
        {
            index = "6206",
            Num_Dw = "9.525",
            Num_D = "62",
            Num_d = "30",
            Num_Z = "9",
            Num_内环ea = "0.1016",
            Num_内环eb = "0.009153",
            Num_外环ea = "0.07677",
            Num_外环eb = "0.01060",
            Num_fi = "0.515",
            Num_fe = "0.52",
            Num_fr = "5000",
            Num_es = "1.84",
        };
        testDatas.Add(Index_6206);

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
        内环ea.text = testDatas[index].Num_内环ea;
        内环eb.text = testDatas[index].Num_内环eb;
        外环ea.text = testDatas[index].Num_外环ea;
        外环eb.text = testDatas[index].Num_外环eb;
        fi.text = testDatas[index].Num_fi;
        fe.text = testDatas[index].Num_fe;
        fr.text = testDatas[index].Num_fr;
        es.text = testDatas[index].Num_es;
    }

    /// <summary>
    /// 从UI输入页面取得数值
    /// </summary>
    public void GetNumbers()
    {
        Num_es = Convert.ToDecimal(es.text);
    }

    /// <summary>
    /// 核心计算函数
    /// </summary>
    public void CalculateFirst()
    {
        // 从各项text取得数值
        Num_Dw = Convert.ToDecimal(Dw.text);
        Num_D = Convert.ToDecimal(DD.text);
        Num_d = Convert.ToDecimal(dd.text);
        Num_Z = Convert.ToDecimal(Z.text);
        Num_fi = Convert.ToDecimal(0.515f);
        Num_fe = Convert.ToDecimal(0.525f);
        Num_fr = Convert.ToDecimal(fr.text);

        // 曲率和、曲率差的计算
        内环.d = ((Num_D + Num_d) / (decimal)2.0 - Num_Dw);
        内环.r = decimal.Round((Num_fi * Num_Dw), 2, MidpointRounding.AwayFromZero);
        内环.曲率和 = (4 / Num_Dw) + (2 / 内环.d) - (1 / 内环.r);
        内环.曲率差 = ((1 / 内环.r) + (2 / 内环.d)) / 内环.曲率和;

        外环.d = ((Num_D + Num_d) / 2 + Num_Dw);
        外环.r = decimal.Round((Num_fe * Num_Dw), 2, MidpointRounding.AwayFromZero);
        外环.曲率和 = (4 / Num_Dw) - (2 / 外环.d) - (1 / 外环.r);
        外环.曲率差 = ((1 / 外环.r) - (2 / 外环.d)) / 外环.曲率和;

        // 最大接触载荷Qmax的计算
        if (fr.text != null)
            Num_Qmax = (5 * Num_fr) / Num_Z;


        // 刷新UI
        //曲率和、曲率差
        内环.曲率和 = decimal.Round(内环.曲率和, 5, MidpointRounding.AwayFromZero);
        内环.曲率差 = decimal.Round(内环.曲率差, 5, MidpointRounding.AwayFromZero);
        ans_内环_曲率和.text = 内环.曲率和.ToString("F5");
        ans_内环_曲率差.text = 内环.曲率差.ToString("F5");
        外环.曲率和 = decimal.Round(外环.曲率和, 5, MidpointRounding.AwayFromZero);
        外环.曲率差 = decimal.Round(外环.曲率差, 5, MidpointRounding.AwayFromZero);
        ans_外环_曲率和.text = 外环.曲率和.ToString("F5");
        ans_外环_曲率差.text = 外环.曲率差.ToString("F5");

        //Qmax
        Num_Qmax = decimal.Round(Num_Qmax, 5, MidpointRounding.AwayFromZero);
        ans_Qmax.text = Num_Qmax.ToString() + " N";
    }

    public void CalculateSecond()
    {
        Num_内环ea = Convert.ToDecimal(内环ea.text);
        Num_内环eb = Convert.ToDecimal(内环eb.text);
        Num_外环ea = Convert.ToDecimal(外环ea.text);
        Num_外环eb = Convert.ToDecimal(外环eb.text);

        // a、b的计算
        if (Num_内环ea != decimal.Zero && Num_内环eb != decimal.Zero && Num_外环ea != decimal.Zero && Num_外环eb != decimal.Zero)
        {
            内环.a = Num_内环ea * 1 * (decimal)Mathf.Pow((float)(Num_Qmax / 内环.曲率和), (float)(1.0 / 3));
            内环.b = Num_内环eb * 1 * (decimal)Mathf.Pow((float)(Num_Qmax / 内环.曲率和), (float)(1.0 / 3));
            外环.a = Num_外环ea * 1 * (decimal)Mathf.Pow((float)(Num_Qmax / 外环.曲率和), (float)(1.0 / 3));
            外环.b = Num_外环eb * 1 * (decimal)Mathf.Pow((float)(Num_Qmax / 外环.曲率和), (float)(1.0 / 3));
        }

        //刷新UI
        内环.a = decimal.Round(内环.a, 5, MidpointRounding.AwayFromZero);
        内环.b = decimal.Round(内环.b, 5, MidpointRounding.AwayFromZero);
        ans_内环_a.text = 内环.a.ToString();
        ans_内环_b.text = 内环.b.ToString();
        外环.a = decimal.Round(外环.a, 5, MidpointRounding.AwayFromZero);
        外环.b = decimal.Round(外环.b, 5, MidpointRounding.AwayFromZero);
        ans_外环_a.text = 外环.a.ToString();
        ans_外环_b.text = 外环.b.ToString();
    }

    public void CalculateThird()
    {
        // Pmax的计算
        if (内环.a != decimal.Zero && 外环.a != decimal.Zero && Num_Qmax != decimal.Zero)
        {
            内环.Pmax = ((decimal)1.5 * Num_Qmax) / (Pi * 内环.a * 内环.b);
            外环.Pmax = ((decimal)1.5 * Num_Qmax) / (Pi * 外环.a * 外环.b);
        }

        内环.Pmax = decimal.Round(内环.Pmax, 5, MidpointRounding.AwayFromZero);
        ans_内环_Pmax.text = 内环.Pmax.ToString() + " N/mm2";
        外环.Pmax = decimal.Round(外环.Pmax, 5, MidpointRounding.AwayFromZero);
        ans_外环_Pmax.text = 外环.Pmax.ToString() + " N/mm2";

        // 弹性趋近量的计算
    }

    /// <summary>
    /// Debug相关
    /// </summary>
    public void CheckData()
    {
        Debug.Log($"内环di = {内环.d}\n内环ri = {内环.r}\n内环曲率和 = {内环.曲率和}\n内环曲率差 = {内环.曲率差}");
        Debug.Log($"外环di = {外环.d}\n外环ri = {外环.r}\n外环曲率和 = {外环.曲率和}\n外环曲率差 = {外环.曲率差}");
    }

    public void ClearAns()
    {
        ans_内环_曲率和.text = "0";
        ans_内环_曲率差.text = "0";
        ans_外环_曲率和.text = "0";
        ans_外环_曲率差.text = "0";
        ans_Qmax.text = "0";
        ans_内环_a.text = "0";
        ans_内环_b.text = "0";
        ans_外环_a.text = "0";
        ans_外环_b.text = "0";
        ans_内环_Pmax.text = "0";
        ans_内环_趋近量.text = "0";
        ans_外环_Pmax.text = "0";
        ans_外环_趋近量.text = "0";
        内环.Pmax = decimal.Zero;
        外环.Pmax = decimal.Zero;
    }
}
