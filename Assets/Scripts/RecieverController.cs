using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Rendering.PostProcessing;



public class RecieverController : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 NewCameraPoisition;
    public GameObject NewEmitter;
    private bool isHitted = false;
    public bool END;

    public bool isEnding = false;
    public GameObject globalVolume;
    public Image EndPanel;
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
        if (END) 
        {
            GameEnd();
            return;
        }
        Camera.main.transform.DOMove(NewCameraPoisition, 2f);
        if (NewEmitter)
        {
            GameManager.setEmitter(NewEmitter);
            LineRenderer line = NewEmitter.GetComponentInChildren<LineRenderer>();
            NewEmitter.GetComponentInChildren<LaserController>().enabled = true;
            if (!LineManager.lineRenderers.Contains(line)) LineManager.AddLineRenderer(line);
        }

    }

    public void GameEnd()
    {
        if (isEnding) return;
        else
        {
            isEnding = true;
            Debug.Log("Game End");
            // PostProcessProfile profile = globalVolume.GetComponent<PostProcessProfile>();
            // Bloom bloom = globalVolume.GetComponent<Bloom>();
            // bloom.threshold.value = Mathf.Lerp(bloom.threshold.value , 0f, 3f);
            EndPanel.DOFade(1, 2f).OnComplete(() =>
            {
                SceneManager.LoadScene("EndScene");
            });
        }
        // SceneManager.LoadScene("EndScene");
    }
}
