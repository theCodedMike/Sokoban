using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType
{
    Blank = 0,
    Wall = 1,
    Player = 2,
    Box = 3,
    EndPoint = 9
}

public class GameController : MonoBehaviour
{
    public const int Width = 9;
    public const int Height = 9;
    private static TileType[,] _tileMap;


    public Sprite[] mapSprites;
    public GameObject brickPrefab;
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject endPointPrefab;

    private Player _player;
    private Vector3 _moveStep;
    private readonly List<Box> _boxes = new();
    private readonly HashSet<Vector3> _endPointsPosition = new();
    
    
    private void Start()
    {
        // 这里可以使用文本保存瓦片类型，然后读取文本进行初始化
        _tileMap = new[,] {
            // 1 1 1 1 1 0 0 0 0
            {TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Blank},
            // 1 2 0 0 1 0 0 0 0
            {TileType.Wall, TileType.Player, TileType.Blank, TileType.Blank, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Blank},
            // 1 0 3 0 1 0 1 1 1
            {TileType.Wall, TileType.Blank, TileType.Box, TileType.Blank, TileType.Wall, TileType.Blank, TileType.Wall, TileType.Wall, TileType.Wall},
            // 1 0 3 0 1 0 1 9 1
            {TileType.Wall, TileType.Blank, TileType.Box, TileType.Blank, TileType.Wall, TileType.Blank, TileType.Wall, TileType.EndPoint, TileType.Wall},
            // 1 1 1 3 1 1 1 9 1
            {TileType.Wall, TileType.Wall, TileType.Wall, TileType.Box, TileType.Wall, TileType.Wall, TileType.Wall, TileType.EndPoint, TileType.Wall},
            // 0 1 1 0 0 0 0 9 1
            {TileType.Blank, TileType.Wall, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Blank, TileType.EndPoint, TileType.Wall},
            // 0 1 0 0 0 1 0 0 1
            {TileType.Blank, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Wall},
            // 0 1 0 0 0 1 1 1 1
            {TileType.Blank, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall},
            // 0 1 1 1 1 1 0 0 0
            {TileType.Blank, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Wall, TileType.Blank, TileType.Blank, TileType.Blank}
        };
        
        GenerateMap();
    }
    
    private void GenerateMap()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                TileType tileType = _tileMap[i, j];
                Vector3 position = new Vector3(j, -i, 0);
                switch (tileType)
                {
                    case TileType.Blank:
                    case TileType.Wall:
                        GameObject brickObj = Instantiate(brickPrefab, transform, true);
                        brickObj.transform.position = position;
                        brickObj.name = $"{j} {-i}";
                        brickObj.GetComponent<SpriteRenderer>().sprite = mapSprites[(int)tileType];
                        break;
                    
                    case TileType.Player:
                        GameObject playerObj = Instantiate(playerPrefab, transform, true);
                        playerObj.transform.position = position;
                        playerObj.name = "Player";
                        _player = playerObj.GetComponent<Player>();
                        break;
                    
                    case TileType.Box:
                        GameObject boxObj = Instantiate(boxPrefab, transform, true);
                        boxObj.transform.position = position;
                        boxObj.name = "Box";
                        _boxes.Add(boxObj.GetComponent<Box>());
                        break;
                    
                    case TileType.EndPoint:
                        GameObject endPointObj = Instantiate(endPointPrefab, transform, true);
                        endPointObj.transform.position = position;
                        endPointObj.name = "EndPoint";
                        _endPointsPosition.Add(position);
                        break;
                }
            }
        }
    }

    private void Update()
    {
        if (IsGameOver())
        {
            print("游戏结束了...");
            Application.Quit();
        }
        
        _moveStep = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            _moveStep = Vector3.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            _moveStep = Vector3.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            _moveStep = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            _moveStep = Vector3.right;

        // 没有移动
        if (_moveStep == Vector3.zero)
            return;
        
        Move();
    }
    
    private void Move()
    {
        Vector3 playerPos = _player.transform.position;
        (int x, int y) = ((int)playerPos.x, (int)playerPos.y);

        Vector3 nextPos = playerPos + _moveStep;
        TileType nextTile = _tileMap[-(int)nextPos.y, (int)nextPos.x];
        print($"IsMove: {x} {y} {_moveStep} {nextTile}");
        
        // 如果人物下一个运动目标点为空
        if (nextTile is TileType.Blank or TileType.EndPoint)
        {
            // 改变数组内人物位置
            _tileMap[-y, x] = TileType.Blank; // 0
            _tileMap[-(int)nextPos.y, (int)nextPos.x] = TileType.Player; // 2

            // 玩家移动
            _player.Move(_moveStep);
            return;
        }
        
        Vector3 nextNextPos = playerPos + _moveStep * 2;
        TileType nextNextTile = _tileMap[-(int)nextNextPos.y, (int)nextNextPos.x];
        if (nextTile == TileType.Box && (nextNextTile is TileType.Blank or TileType.EndPoint))
        {
            _tileMap[-y, x] = TileType.Blank;
            _tileMap[-(int)nextPos.y, (int)nextPos.x] = TileType.Player;
            _tileMap[-(int)nextNextPos.y, (int)nextNextPos.x] = TileType.Box;
            
            //箱子移动
            _boxes.First(box => box.transform.position == nextPos).Move(_moveStep);
            //玩家移动
            _player.Move(_moveStep);
        }
    }

    // 判定游戏是否结束
    private bool IsGameOver()
    {
        foreach (Box box in _boxes)
        {
            if (!_endPointsPosition.Contains(box.transform.position))
                return false;
        }

        return true;
    }
}