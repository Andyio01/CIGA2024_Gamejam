using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
// using System.Numerics;

public class ReflectionController : MonoBehaviour
{
    public enum PlaneType{
        Mirror,
        water

    }

    [SerializeField]
    private PlaneType planeType;
    private bool isHit = true;
    public Transform EmissionPoint;
    // 该点暂时未被击中
    private bool waitForHit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!isHit)
        {
            // 没有被击中时关闭LineRender
            GetComponentInChildren<LineRenderer>().enabled = false;
            if(LineManager.lineRenderers.Contains(this.transform.GetComponentInChildren<LineRenderer>())) LineManager.DeleteLineRenderer(this.transform.GetComponentInChildren<LineRenderer>());
            // Debug.Log("No Hit");
        }
        else{
            // 有前置光线时开启LineRender
            GetComponentInChildren<LineRenderer>().enabled = true;
            waitForHit = false;
            // gameObject.GetComponent<LaserController>().SetLength(0.0f);
            // LineManager.AddLineRenderer(this.transform.GetComponentInChildren<LineRenderer>());
            // Debug.Log("Hit");
        }
        isHit = false;

        
    }
    // }
    // // 被射线击中时才激活自身的LineRender，获取其方向并旋转
    // // private void OnCollisionEnter(Collision other) {
    // //     Debug.Log("Hited By:" + other.gameObject.name);
    // // }
    public void hitByLaser(Vector2 direction, Vector2 hitPoistion)
    {
        
        isHit = true;
        // GetComponentInChildren<LineRenderer>().enabled = true;
        // // 获取射线方向
        // Vector3 direction = GetComponentInChildren<LineRenderer>().GetPosition(1) - GetComponentInChildren<LineRenderer>().GetPosition(0);
        // 旋转自身
        EmissionPoint.position = hitPoistion;
        EmissionPoint.right = direction;
        if(!LineManager.lineRenderers.Contains(this.transform.GetComponentInChildren<LineRenderer>())) 
        LineManager.AddLineRenderer(this.transform.GetComponentInChildren<LineRenderer>());
    }

}
