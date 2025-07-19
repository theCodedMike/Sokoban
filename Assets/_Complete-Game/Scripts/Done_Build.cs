using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Done_Build : MonoBehaviour
{
    public static int[] temp_map;
    public int[] final_map;
    public GameObject[] destoryObj;
    public Sprite[] mapSprites;
    public bool playerDestory = false;
    public bool boxDestory = false;


    GameObject g;

    public GameObject mapPrefab;//地图的墙壁
    public GameObject playerPrefab;//角色
    public GameObject boxPrefab;//箱子
    public GameObject finalBoxPrefab;//到终点后的箱子
    public GameObject finalPrefab;//游戏获胜后刷新一张图片，用于提示游戏获胜。

    /// <summary>
    /// 数组初始化
    /// </summary>
    private void Awake()
    {
        
        final_map = new int[9 * 9];//传值数组
        temp_map = new int[]
            {
                1, 1, 1, 1, 1, 0, 0, 0, 0,
                1, 2, 0, 0, 1, 0, 0, 0, 0,
                1, 0, 3, 0, 1, 0, 1, 1, 1,
                1, 0, 3, 0, 1, 0, 1, 9, 1,
                1, 1, 1, 3, 1, 1, 1, 9, 1,
                0, 1, 1, 0, 0, 0, 0, 9, 1,
                0, 1, 0, 0, 0, 1, 0, 0, 1,
                0, 1, 0, 0, 0, 1, 1, 1, 1,
                0, 1, 1, 1, 1, 1, 0, 0, 0

            };

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                final_map[j * 9 + i] = temp_map[j * 9 + i];
            }
        }
    }



    // Use this for initialization
    void Start()
    {
        BuildMap();
    }

    /// <summary>
    /// 地图初始化
    /// </summary>
    void BuildMap()
    {

        destoryObj = new GameObject[9 * 9];//用于销毁GameObject

        int i = 0;
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                
               
                switch (temp_map[i])
                {
                    
                    case 1: //生成墙壁
                    case 0://无图
                
                    g = Instantiate(mapPrefab) as GameObject;
                    g.transform.position = new Vector3(x, -y, 0);
                    g.name = x.ToString() + y;

                    destoryObj[y * 9 + x] = g;

                    Sprite icon = mapSprites[temp_map[i]];//使贴图与地图数组吻合
                    g.GetComponent<SpriteRenderer>().sprite = icon;
                        break;
                        
                    case 2://生成人物

                    g = Instantiate(playerPrefab) as GameObject;
                    g.transform.position = new Vector3(x, -y, 0);
                    g.name = "Player";

                    destoryObj[y * 9 + x] = g;
                        break;

                    
                    case 3://生成箱子

                    g = Instantiate(boxPrefab) as GameObject;
                    g.transform.position = new Vector3(x, -y, 0);
                    g.name = "Box";

                    destoryObj[y * 9 + x] = g;
                        break;
                        
                }
                if (i < 80)
                {
                    i++;
                }
            }
        }
    }
    
    /// <summary>
    /// 角色移动
    /// </summary>
    void PlayerMove()
    {//获取各项数值

        int x, y;
        int x_num = 0;
        int y_num = 0;

        //获取Player坐标
        Transform playerPos = GameObject.Find("Player").GetComponent<Transform>();
        x = (int)playerPos.position.x;
        y = (int)playerPos.position.y;

        //获取界面移动坐标数值
        x_num = FindObjectOfType<Done_GameController>().x_num;
        y_num = FindObjectOfType<Done_GameController>().y_num;

        //获取动画 一定要放在销毁Player前
        string animator_name = FindObjectOfType<Done_GameController>().animator_name;

        //销毁原有Player
        Destroy(destoryObj[-y*9+x]);

       
        g = Instantiate(playerPrefab) as GameObject;

        g.GetComponent<Animator>().Play(animator_name);
        g.transform.position = new Vector3(x + x_num, y + y_num, 0);
        g.name = "Player";

        //将新的Player存入销毁数组
        destoryObj[-((y + y_num) * 9) + x + x_num] = g;
        playerDestory = false;
    }
    /// <summary>
    /// 箱子移动
    /// </summary>
    void BoxMove()
    {//获取各项数值

        int x, y;
        int x_num = 0;
        int y_num = 0;


        //获取界面移动坐标数值及Box界面编号
        x_num = FindObjectOfType<Done_GameController>().x_num;
        y_num = FindObjectOfType<Done_GameController>().y_num;


        //获取相应Box坐标
        Transform playerPos = GameObject.Find("Player").GetComponent<Transform>();
        x = (int)playerPos.position.x + x_num;//Player的X坐标+x_num为下一个目标点的X坐标，即为相应Box的X坐标，Y同理
        y = (int)playerPos.position.y + y_num;

        //销毁原有Box
        Destroy(destoryObj[-y * 9 + x]);
        
        if (final_map[-((y + y_num) * 9) + x + x_num]== 0 || final_map[-((y + y_num) * 9) + x + x_num] == 3)
        {
            g = Instantiate(boxPrefab) as GameObject;
            g.transform.position = new Vector3(x + x_num, y + y_num, 0);
            g.name = "Box";
        }

        else if (final_map[-((y + y_num) * 9) + x + x_num] == 9)
        {
            g = Instantiate(finalBoxPrefab) as GameObject;
            g.transform.position = new Vector3(x + x_num, y + y_num, 0);
            g.name = "FinalBox";
        }
        //将新的Box存入销毁数组
        destoryObj[-((y + y_num) * 9) + x + x_num] = g;
        playerDestory = false;
        boxDestory = false;
    }

    private void Update()
    {
        if (playerDestory)//如果Player移动并且原有Player需要销毁
        {
            if (boxDestory)//如果Box移动并且原有Box需要销毁
            {
                BoxMove();
            }
            PlayerMove();
        }
        //游戏结束判定
        if (GameObject.Find("Box") == null&&GameObject.Find("Final")==null)
        {
            g = Instantiate(finalPrefab) as GameObject;
            g.transform.position = new Vector3(4, -4, 0);
            g.name = "Final";
        }
    }
}
