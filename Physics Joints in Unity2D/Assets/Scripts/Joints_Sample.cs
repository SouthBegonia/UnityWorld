using UnityEngine;
using System.Collections;
using System.Linq;

//挂在于MainCamera上的UI脚本,暂不用
public class Joints_Sample : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Label(new Rect(2, 2, 300, 300), "Distance Joint 2D\nUse the mouse to move");
        GUI.Label(new Rect(300, 2, 300, 300), "Spring Joint 2D\nUse the mouse to move");
        GUI.Label(new Rect(550, 10, 300, 300), "Wheel Joint 2D");
        GUI.Label(new Rect(200, 250, 300, 300), "Hinge Joint 2D");
        GUI.Label(new Rect(570, 270, 300, 300), "Slider Joint 2D");
    }
}
