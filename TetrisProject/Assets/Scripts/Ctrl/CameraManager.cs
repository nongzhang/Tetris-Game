using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraManager : MonoBehaviour {

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ZoomIn()
    {
        mainCamera.DOOrthoSize(14f,0.5f);
    }

    public void ZoomOut()
    {
        mainCamera.DOOrthoSize(20f,0.5f);
    }
}
