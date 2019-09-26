using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapAPI : MonoBehaviour
{
    public Tilemap tilemap;
    private TileBase tile;
    public GameObject explosionEffect;

    private Grid grid;

    void Start()
    {
        grid = GetComponent<Grid>();

        ////获取瓦片
        //TileBase tile_2 = tilemap.GetTile(new Vector3Int(-4, -2, 0));
        //Debug.Log(tile_2);

        ////设置指定位置为瓦片tile
        //tilemap.SetTile(Vector3Int.zero, tile);

        ////删除指定位置的瓦片
        //tilemap.SetTile(Vector3Int.zero, null);

        ////瓦片替换
        //tilemap.SwapTile(tile_2, tile);

        //清空瓦片
        //tilemap.ClearAllTiles();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //屏幕坐标 -> 世界坐标
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //世界坐标 -> 格子坐标
            Vector3Int cellPos = grid.WorldToCell(worldPos);

            //爆炸特效
            if ((tile = tilemap.GetTile(cellPos)) != null)
            {
                Debug.Log("TileName = " + tile);
                Explosion(cellPos);
            }
                
        }
    }

    private void Explosion(Vector3Int cellPos)
    {
        //设置格子为null
        tilemap.SetTile(cellPos, null);

        //格子 -> 世界坐标
        Vector3 worldPos = grid.CellToLocalInterpolated(cellPos + new Vector3(0.5f, 0.5f, 0f));
        Instantiate(explosionEffect, worldPos, Quaternion.identity);
    }
}
