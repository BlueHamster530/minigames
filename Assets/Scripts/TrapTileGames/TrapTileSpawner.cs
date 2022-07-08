using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTileSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject TileObject;

    [SerializeField]
    int MapSizeX = 8;
    [SerializeField]
    int MapSizeY = 8;

    GameObject[,] Tiles;

    // Start is called before the first frame update
    void Start()
    {
        if (TileObject == null)
        {
            print("TileObject IS Null");
            return;
        }
        Tiles = new GameObject[MapSizeX, MapSizeY];
        for (int i = 0; i < MapSizeX; i++)
        {
            for (int ii = 0; ii < MapSizeY; ii++)
            {
                Tiles[i, ii] = Instantiate(TileObject, new Vector3(i, 0, ii) * 5.0f, Quaternion.identity);
                Tiles[i, ii].transform.SetParent(this.transform);
               Tiles[i, ii].GetComponent<Tilescript>().Init();
            }
        }
        SetTile();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < MapSizeX; i++)
            {
                for (int ii = 0; ii < MapSizeY; ii++)
                {
                    Destroy(Tiles[i,ii]);
                }
            }
            Tiles = new GameObject[MapSizeX, MapSizeY];
            for (int i = 0; i < MapSizeX; i++)
            {
                for (int ii = 0; ii < MapSizeY; ii++)
                {
                    Tiles[i, ii] = Instantiate(TileObject, new Vector3(i, 0, ii) * 5.0f, Quaternion.identity);
                    Tiles[i, ii].transform.SetParent(this.transform);
                    Tiles[i, ii].GetComponent<Tilescript>().Init();
                }
            }
            SetTile();
        }
    }
    void SetTile()
    {//x방향으로 진행시
        int PosX = 0;
        int PosY = Random.Range(0, MapSizeY); ;
        Tiles[PosX, PosY].GetComponent<Tilescript>().SetFakeTile(false) ;

        while (true)
        {
            while (true)
            {
                int x = Random.Range(0, 2);
                int y = Random.Range(-1, 2);
                if (PosY == 0)
                    y = Random.Range(0, 2);
                else if (PosY == 7)
                    y = Random.Range(-1, 1);

                Tilescript nexttile = Tiles[PosX + x, PosY + y].GetComponent<Tilescript>();
                if (nexttile.GetFakeTile() == true)
                {
                    PosX += x;
                    PosY += y;
                    nexttile.SetFakeTile(false);
                    break;
                }
            }
            if (PosX >= 7)
                break;
        }

    }

}
