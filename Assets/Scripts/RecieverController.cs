using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;



public class RecieverController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 NewCameraPoisition;
    public GameObject NewEmitter;
    private bool isHitted = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void hitByLaser()
    {
        if (isHitted) return;
        isHitted = true;
        Camera.main.transform.DOMove(NewCameraPoisition, 2f);
        if (NewEmitter)
        {
            GameManager.setEmitter(NewEmitter);
            LineRenderer line = NewEmitter.GetComponentInChildren<LineRenderer>();
            NewEmitter.GetComponentInChildren<LaserController>().enabled = true;
            if (!LineManager.lineRenderers.Contains(line)) LineManager.AddLineRenderer(line);
        }

    }
}
