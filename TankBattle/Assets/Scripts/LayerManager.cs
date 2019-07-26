using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public static int blueLayer = 10;
    public static int greenLayer = 11;
    public static int redLayer = 12;

    static public LayerMask GetEnemyLayer(Team team)
    {
        LayerMask mask = 0;
        switch (team)
        {
            case Team.Blue:
                mask = (1 << redLayer) | (1 << greenLayer);     //开启redLayer层和greenLayer层
                break;
            case Team.Green:
                mask = (1 << redLayer) | (1 << blueLayer);
                break;
            case Team.Red:
                mask = (1 << blueLayer) | (1 << greenLayer);
                break;
        }
        return mask;
    }
}
