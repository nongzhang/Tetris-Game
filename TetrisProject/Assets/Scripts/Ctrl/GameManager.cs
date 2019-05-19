using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private bool isPause = true;

    public Shape[] shapes;

    private Ctrl ctrl;

    private Transform blockHolder;

    public Color[] colors;

    private Shape currentShape = null;

    //public List<Shape> currentShapes = new List<Shape>();

    //public bool isStartFall = false;  //是否开始下落
    private void Awake()
    {
        ctrl = GetComponent<Ctrl>();
        blockHolder = transform.Find("BlockHolder");
    }


    // Update is called once per frame
    void Update () {
        if (isPause)
        {
            return;
        }
        if (currentShape == null)
        {
            SpawnShape();
        }

    }

    public void ClearShape()
    {
        if (currentShape != null)
        {
            Destroy(currentShape.gameObject);
            currentShape = null;
        }
    }

    public void StartGame()
    {
        isPause = false;
        if (currentShape != null)
        {
            currentShape.Resume();
        }
    }


    public void PauseGame()
    {
        isPause = true;
        if (currentShape != null)
        {
            currentShape.Pause();
        }
    }
    void SpawnShape() 
    {
        int index = Random.Range(0, shapes.Length);
        int indexColor = Random.Range(0, colors.Length);
        currentShape = GameObject.Instantiate(shapes[index]);
        currentShape.transform.parent = blockHolder;
        currentShape.Init(colors[indexColor],ctrl,this);

        //currentShapes.Add(currentShape);
        //currentShapes[0].gameObject.tag = "first";
    }

    /// <summary>
    /// 方块落下来了
    /// </summary>
    public void FallDown()
    {
        currentShape = null;
        if (ctrl.model.isDataUpdate)
        {
            ctrl.view.UpdataGameUI(ctrl.model.Score,ctrl.model.HighScore);
        }
        foreach (Transform t in blockHolder)
        {
            if (t.childCount <=1)
            {
                Destroy(t.gameObject);
            }
        }
        if (ctrl.model.IsGameOver())
        {
            PauseGame();
            ctrl.view.ShowGameOverUI(ctrl.model.Score);
        }
    }
}
