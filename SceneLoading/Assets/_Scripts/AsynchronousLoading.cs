using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsynchronousLoading : MonoBehaviour
{
    private AsyncOperation async;
    public Slider progressSlider;
    public Text progressText;

    public Image progressCircle;
    public float rotateSpeed;

    private void Start()
    {
        if (progressSlider == null)
            progressSlider = GameObject.Find("ProgressBar").GetComponent<Slider>();
        if (progressText == null)
            progressText = GameObject.Find("ProgressText").GetComponent<Text>();

        progressSlider.value = 0;
        progressText.text = "0 %";
        
        StartCoroutine(LoadScene("CircleProcess"));
    }

    IEnumerator LoadScene(string SceneName)
    {
        float progressValue;
        async = SceneManager.LoadSceneAsync(SceneName);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress < 0.9f)
                progressValue = async.progress;
            else
                progressValue = 1.0f;

            progressSlider.value = progressValue;
            progressText.text = (int)(progressSlider.value * 100) + " %";

            if (progressValue >= 0.9)
            {
                progressText.text = "按任意键继续";
                if (Input.anyKeyDown)
                {
                    async.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
