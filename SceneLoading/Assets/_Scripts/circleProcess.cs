using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class circleProcess : MonoBehaviour
{

    [SerializeField]
    float speed;

    [SerializeField]
    Transform process;
    Animation processShrink;

    [SerializeField]
    Transform indicator;

    public int targetProcess { get; set; }
    private float currentAmout = 0;

    void Start()
    {
        targetProcess = 100;
        processShrink = process.gameObject.GetComponent<Animation>();
        processShrink.enabled = false;
    }

    void Update()
    {

        if (currentAmout < targetProcess)
        {
            currentAmout += speed;
            if (currentAmout > targetProcess)
                currentAmout = targetProcess;
            indicator.GetComponent<Text>().text = ((int)currentAmout).ToString() + "%";
            process.GetComponent<Image>().fillAmount = currentAmout / 100.0f;
        }
        else
        {
            processShrink.enabled = true;
        }

    }
}
