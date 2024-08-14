using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TrapController : MonoBehaviour
{
    // Start is called before the first frame update
    public Material laserMaterial;
    private Color curColor;
    public AudioSource DeathAudio;
    private bool isRotating = false;

    void Start()
    {
        //if (laserMaterial) 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hitByLaser()
    {
        if (!isRotating)
        {
            DeathAudio.Play();
            //curColor = laserMaterial.GetColor("_Color");
            //laserMaterial = GameManager.curEmitter.GetComponentInChildren<LineRenderer>().gameObject.GetComponent<Renderer>().material;
        }
        StartCoroutine(DeadAndRestore());
    }

    private IEnumerator DeadAndRestore()
    {
        GameObject emitter = GameManager.curEmitter;
        if (laserMaterial != null)
        {
            laserMaterial.SetColor("_Color", new Color(1.56862748f, 0, 0, 1));
        }
        // #TODO ÆÁ±ÎÍæ¼Ò²Ù×÷
        yield return new WaitForSeconds(0.1f);
        isRotating = true;
        emitter.transform.DORotate(new Vector3(0, 0, GameManager.curRotation), 1.5f).OnComplete(() => {
            laserMaterial.SetColor("_Color", new Color(2.0942409f, 2.0942409f, 2.0942409f, 1));
            isRotating = false;
        }
        );
    }
}
