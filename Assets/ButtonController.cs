using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject NewEmitter;
    public GameObject PreEmitter;
    public GameObject Obstacle;
    private Vector2 StartPoisition;
    public Vector2 EndPoisition;
    private bool isHitted = false;
    // 控制当前的激光是否能够二次触发机关
    public bool triggerAble = true;
    public AudioSource audioSource;
    void Start()
    {
        if (Obstacle) StartPoisition = Obstacle.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void hitByLaser()
    {
        if (isHitted)
        {
            triggerAble = false;
            return;
        }
        
        isHitted = true;
        if (!audioSource.isPlaying) audioSource.Play();
        GetComponent<SpriteRenderer>().DOColor(new Color(255, 240, 0, 255), 1f);
        if (NewEmitter)
        {
            GameManager.setEmitter(NewEmitter);
            LineRenderer line = NewEmitter.GetComponentInChildren<LineRenderer>();
            line.enabled = true;
            NewEmitter.GetComponentInChildren<LaserController>().enabled = true;
            if (!LineManager.lineRenderers.Contains(line)) LineManager.AddLineRenderer(line);
        }

        if (Obstacle)
        {
            Obstacle.transform.DOMove(EndPoisition, 2f);
        }

    }

    public void unHitted()
    {
        Debug.Log("光线离开");
        isHitted = false;
        GetComponent<SpriteRenderer>().DOColor(new Color(156, 156, 156, 255), 1f);
        if (PreEmitter)
        {
            GameManager.setEmitter(PreEmitter);
            LineRenderer line = NewEmitter.GetComponentInChildren<LineRenderer>();
            line.enabled = false;
            NewEmitter.GetComponentInChildren<LaserController>().enabled = false;
            if (LineManager.lineRenderers.Contains(line)) LineManager.DeleteLineRenderer(line);
            triggerAble = true;

        }

        if (Obstacle)
        {
            Obstacle.transform.DOMove(StartPoisition, 2f);
        }
    }
}
