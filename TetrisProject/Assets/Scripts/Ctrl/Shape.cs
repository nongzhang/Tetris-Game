using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour {

    private Transform pivot;

    private Ctrl ctrl;

    private GameManager gameManager;

    private bool isPause = false;

    private bool isSpeedUp = false;

    private float timer = 0;

    private float stepTime = 0.8f;  //0.8s下落一格

    private int multiple = 16;

    private void Awake()
    {
        pivot = transform.Find("Povit");
    }

    private void Update()
    {
        if (isPause)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer > stepTime)
        {
            timer = 0;
            Fall();

        }
        InputControl();

    }
    public void Init(Color color,Ctrl ctrl,GameManager gameManager)
    {
        foreach (Transform t in transform)
        {
            if (t.tag == "Block")
            {
                t.GetComponent<SpriteRenderer>().color = color;
            }
        }
        this.ctrl = ctrl;
        this.gameManager = gameManager;
    }

    /// <summary>
    /// 图形下落
    /// </summary>
    private  void Fall()
    {
        Vector3 pos = transform.position;
        pos.y -= 1;
        transform.position = pos;
        if (ctrl.model.IsValidMapPosition(this.transform) == false)                   //此位置超出边界或者此位置有别的图形占用
        {
            pos.y += 1;
            transform.position = pos;
            isPause = true;
            
            bool isLineClear = ctrl.model.PlaceShape(this.transform);
            if (isLineClear)
            {
                ctrl.audioManager.PlayLineClear();
            }
            gameManager.FallDown();
            return;
        }
        else
        {
            ctrl.audioManager.PlayDrop();
        }
        
    }

    private void InputControl()
    {
        //if (isSpeedUp)    //加速下落时是否可以控制图形移动及旋转
        //{
        //    return;
        //}
        float h = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            h = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            h = 1;
        }
        if (h != 0)
        {
            Vector3 pos = transform.position;
            pos.x += h;
            transform.position = pos;
            if (ctrl.model.IsValidMapPosition(this.transform) == false)
            {
                pos.x -= h;
                transform.position = pos;
                return;
            }
            ctrl.audioManager.PlayControl();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(pivot.position,Vector3.forward,90);
            if (ctrl.model.IsValidMapPosition(this.transform) == false)
            {
                transform.RotateAround(pivot.position, Vector3.forward, -90);
            }
            else
            {
                ctrl.audioManager.PlayControl();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isSpeedUp = true;
            stepTime /= multiple;
        }
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Resume()
    {
        isPause = false;
    }
}
