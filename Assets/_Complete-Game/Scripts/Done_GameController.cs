using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Done_GameController : MonoBehaviour {
    
    
    public int x_num = 0;
    public int y_num = 0;
    public bool IsChange = false;
    public int box_num;
    public string animator_name;
 


  
    
  

    // Use this for initialization
    void Start()
    {
      
    }

    public bool IsMove()
    {
        int x, y;
        
        //获取Player坐标
        Transform playerPos = GameObject.Find("Player").GetComponent<Transform>();
        x = (int)playerPos.position.x;
        y = (int)playerPos.position.y;

        GameObject g;



        // 如果人物下一个运动目标点为空
        
        if ((Done_Build.temp_map[-y * 9 + x + (int)dir] == 0 || Done_Build.temp_map[-y * 9 + x + (int)dir] == 9) && IsChange == true)
        {
            //改变数组内人物位置
            Done_Build.temp_map[-y * 9 + x] = 0;
            Done_Build.temp_map[-y * 9 + x + (int)dir] = 2;

            // 回传数据
            //FindObjectOfType<Done_Build>().temp_map = temp_map;
            IsChange = false;
            FindObjectOfType<Done_Build>().playerDestory = true;
            return true;
        }

        //如果人物下一个运动目标点为墙壁
        else if (Done_Build.temp_map[-y * 9 + x + (int)dir] == 1 && IsChange == true)
        {
            IsChange = false;
            return false;
        }
        //   如果人物下一个运动目标点为箱子
        else if (Done_Build.temp_map[-y * 9 + x + (int)dir] == 3 && (Done_Build.temp_map[-y * 9 + x + (int)dir*2] == 0 || Done_Build.temp_map[-y * 9 + x + (int)dir*2] == 9) && IsChange == true)         //temp_map[-y * 9 + x + array_num]：下一个运动目标点的数组位置
        {


            //改变数组内人物及箱子位置
            Done_Build.temp_map[-y * 9 + x] = 0;
            Done_Build.temp_map[-y * 9 + x + (int)dir] = 2;
            Done_Build.temp_map[-y * 9 + x + (int)dir*2] = 3;

            //回传数据
            //FindObjectOfType<Done_Build>().temp_map = temp_map;
            IsChange = false;
            FindObjectOfType<Done_Build>().playerDestory = true;
            FindObjectOfType<Done_Build>().boxDestory = true;
            return true;

        }

        return false;
    }

    public enum Direction { Up = -9,Down = 9,Left = -1,Right = 1}
    public Direction dir;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) { dir = Direction.Up; }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { dir = Direction.Down; }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { dir = Direction.Left; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { dir = Direction.Right; }

        switch (dir)
        {
            case Direction.Up:
                IsChange = true;
                x_num = 0;                  //生成新Gameboject时的实际位置坐标
                y_num = 1;
                animator_name = "Up";       //动画控制
                break;

            case Direction.Down:
                IsChange = true;
                x_num = 0;
                y_num = -1;
                animator_name = "Down";
                break;

            case Direction.Left:
                IsChange = true;
                x_num = -1;
                y_num = 0;
                animator_name = "Left";
                break;
            case Direction.Right:
                IsChange = true;
                x_num = 1;
                y_num = 0;
                animator_name = "Right";
                break;

        }
            IsMove();
    }
}
