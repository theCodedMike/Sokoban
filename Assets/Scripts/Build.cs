using System;
using UnityEngine;

public class Build : MonoBehaviour
{
    public static TileType[] tempMap; // 传值数组
    public static TileType[] finalMap; // 用于终点检测
    public const int Width = 9;
    public const int Height = 9;

    public Sprite[] mapSprites;
    public GameObject mapPrefab;
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject finalBoxPrefab;

    private GameObject obj;
    
    
    private void Start()
    {
        finalMap = new TileType[Height * Width];
        tempMap = new []
        {
            // 1 1 1 1 1 0 0 0 0
            TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Blank,
            // 1 2 0 0 1 0 0 0 0
            TileType.Wall, TileType.Character, TileType.Blank, TileType.Blank, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Blank,
            // 1 0 3 0 1 0 1 1 1
            TileType.Wall, TileType.Blank, TileType.Box, TileType.Blank, TileType.Wall, TileType.Blank, TileType.Wall, TileType.Wall, TileType.Wall,
            // 1 0 4 0 1 0 1 9 1
            TileType.Wall, TileType.Blank, TileType.Box1, TileType.Blank, TileType.Wall, TileType.Blank, TileType.Wall, TileType.EndPoint, TileType.Wall,
            // 1 1 1 5 1 1 1 9 1
            TileType.Wall, TileType.Wall, TileType.Wall, TileType.Box2, TileType.Wall, TileType.Wall, TileType.Wall, TileType.EndPoint, TileType.Wall,
            // 0 1 1 0 0 0 0 9 1
            TileType.Blank, TileType.Wall, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Blank, TileType.EndPoint, TileType.Wall,
            // 0 1 0 0 0 1 0 0 1
            TileType.Blank, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Wall,
            // 0 1 0 0 0 1 1 1 1
            TileType.Blank, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall,
            // 0 1 1 1 1 1 0 0 0
            TileType.Blank, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank
        };

        for (int i = 0, len = tempMap.Length; i < len; i++)
            finalMap[i] = tempMap[i];
        
        BuildMap();
    }


    private void BuildMap()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                int idx = i * Width + j;
                switch (tempMap[idx])
                {
                    case TileType.Blank:
                    case TileType.Wall:
                        obj = Instantiate(mapPrefab, transform, true);
                        obj.transform.position = new Vector3(j, -i, 0);
                        obj.name = $"{i} {j}";
                        obj.GetComponent<SpriteRenderer>().sprite = mapSprites[(int)tempMap[idx]];
                        break;
                    
                    case TileType.Character:
                        obj = Instantiate(playerPrefab, transform, true);
                        obj.transform.position = new Vector3(j, -i, 0);
                        obj.name = "Player";
                        break;
                    
                    case TileType.Box:
                        obj = Instantiate(boxPrefab, transform, true);
                        obj.transform.position = new Vector3(j, -i, 0);
                        obj.name = "Box";
                        break;
                }
            }
        }
    }
    
    
}

[Serializable]
public enum TileType
{
    Blank = 0,
    Wall = 1,
    Character = 2,
    Box = 3,
    Box1 = 4,
    Box2 = 5,
    EndPoint = 9
}