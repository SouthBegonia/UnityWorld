using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour
{
	public Transform[] backgrounds;				//背景层列表
	public float parallaxScale;					//视差范围 相机移动背景的比例值.
	public float parallaxReductionFactor;		//视差递减系数：每一层递减多少视差
	public float smoothing;						//平滑度：视差移动的平滑度


	private Transform cam;						//主相机
	private Vector3 previousCamPos;				//主相机坐标


	void Awake ()
	{
		cam = Camera.main.transform;
	}


	void Start ()
	{	
		previousCamPos = cam.position;
	}


	void Update ()
	{
        //计算视差值：从上一帧移动到当前帧的增量值的相反数
		float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;

		for(int i = 0; i < backgrounds.Length; i++)
		{
            //视差移动后背景层的坐标 = 当前坐标 + 视差增量值
			float backgroundTargetPosX = backgrounds[i].position.x + parallax * (i * parallaxReductionFactor + 1);

            //视差移动仅限制在X方向，而其他两轴不变
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //更新各背景计算后的坐标
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

        //取得当前帧的相机坐标给下一个update
		previousCamPos = cam.position;
	}
}
