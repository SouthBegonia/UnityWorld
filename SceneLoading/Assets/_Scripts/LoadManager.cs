using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public static LoadManager instance;

    private SynchronousLoading synchronous;

    public string[] SceneNames;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        synchronous = GetComponent<SynchronousLoading>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            synchronous.Load(SceneNames[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            synchronous.Load(SceneNames[1]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            synchronous.Load(SceneNames[2]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            synchronous.Load("ProgressLoad");
        }
    }
}
