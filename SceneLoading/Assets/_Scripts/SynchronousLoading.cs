using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SynchronousLoading : MonoBehaviour
{
    public void Load(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
