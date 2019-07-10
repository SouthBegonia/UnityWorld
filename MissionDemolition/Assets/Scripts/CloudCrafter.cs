using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    public int numClouds = 40;          //云朵数量
    public GameObject[] cloudPrefabs;   //4个云朵预制体的数组
    public Vector3 cloudPosMin;         //云朵位置下限
    public Vector3 cloudPosMax;         //云朵位置上限
    public float cloudScaleMin = 1;     //云朵的最小缩放比例
    public float cloudScaleMax = 5;     //云朵的最大缩放比例
    public float cloudSpeedMult = 0.5f; //云朵移动速度
    public bool ______________________________________;

    public GameObject[] cloudInstances; //储存实例化numClouds个云朵的数组

    private void Awake()
    {
        //创建一个cloudInstances数组，储存所有云朵的实例
        cloudInstances = new GameObject[numClouds];

        //查找CloudAnchor父对象
        GameObject anchor = GameObject.Find("CloudAnchor");

        //遍历Cloud_[数字]并创建实例
        GameObject cloud;
        for(int i = 0; i < numClouds; i++)
        {
            int prefabNum = Random.Range(0, cloudPrefabs.Length);
            cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;

            //设置云朵位置
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            //设置云朵缩放比例
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            //较小的云朵(scaleU较小)离地面较近
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100 - 90 * scaleU;

            //参数配置于云朵
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            //使云朵成为CloudAnchor的子物体
            cloud.transform.parent = anchor.transform;

            //将云朵添加到CloudInstances数组中
            cloudInstances[i] = cloud;
        }
    }

    private void Update()
    {
        foreach(GameObject cloud in cloudInstances)
        {
            //获取云朵的缩放比例和位置
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            //云朵越大，移动速度越快
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            //若云朵已经处于画面左侧较远位置，则将其放于最右侧
            if (cPos.x <= cloudPosMin.x)
                cPos.x = cloudPosMax.x;

            //将新位置应用到云朵上
            cloud.transform.position = cPos;
        }
    }
}
