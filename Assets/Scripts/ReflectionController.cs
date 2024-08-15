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
    public bool isHit = false;
    public Transform ReflectionPoint;
    public Transform RefractionPoint;
    public float refractiveIndex1 = 1.0f; // 空气的折射率
    public float refractiveIndex2 = 1.5f; // 目标介质的折射率
    // 该点暂时未被击中
    private bool waitForHit = false;
    // 入射光线是从上方还是下方射入
    private bool underWater = false;
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
            ReflectionPoint.GetComponentInChildren<LineRenderer>().enabled = false;
            RefractionPoint.GetComponentInChildren<LineRenderer>().enabled = false;
            // if(LineManager.lineRenderers.Contains(this.transform.GetComponentInChildren<LineRenderer>())) LineManager.DeleteLineRenderer(this.transform.GetComponentInChildren<LineRenderer>());
            // Debug.Log("No Hit");
            foreach(LineRenderer line in this.transform.GetComponentsInChildren<LineRenderer>())
            {
                if (LineManager.lineRenderers.Contains(line)) LineManager.DeleteLineRenderer(line);
            }
            foreach(LaserController laser in this.transform.GetComponentsInChildren<LaserController>())
            {
                if(laser.curHitted){
                    if(laser.curHitted.GetComponent<ReflectionController>())
                    {
                            laser.curHitted.GetComponent<ReflectionController>().unHitted();
                    }
                    else if(laser.curHitted.GetComponent<PointController>())
                    {
                            laser.curHitted.GetComponent<PointController>().unHitted();
                    }
                    else if (laser.curHitted.GetComponent<ButtonController>())
                    {
                            laser.curHitted.GetComponent<ButtonController>().unHitted();
                    }
                    laser.curHitted = null;
                }
            }
        }
        else{
            // 有前置光线时开启LineRender

            // 如果是镜子表面，开启反射光线并关闭折射光线
            if (planeType == PlaneType.Mirror){
                RefractionPoint.GetComponentInChildren<LineRenderer>().enabled = false;
                if (LineManager.lineRenderers.Contains(RefractionPoint.GetComponentInChildren<LineRenderer>())) LineManager.DeleteLineRenderer(RefractionPoint.GetComponentInChildren<LineRenderer>());
                ReflectionPoint.GetComponentInChildren<LineRenderer>().enabled = true;
            }
            // 如果是水面，根据入射光线判断
            else if (planeType == PlaneType.water){
                // 光线从水面上方射入，开启折射和反射
                if (!underWater)
                {
                    ReflectionPoint.GetComponentInChildren<LineRenderer>().enabled = true;
                    RefractionPoint.GetComponentInChildren<LineRenderer>().enabled = true;
                }
                // 光线从水下射入，只折射
                else
                {
                    ReflectionPoint.GetComponentInChildren<LineRenderer>().enabled = false;
                    RefractionPoint.GetComponentInChildren<LineRenderer>().enabled = true;
                }
            }
            
            // GetComponentInChildren<LineRenderer>().enabled = true;
            // LineRenderer[] line = GetComponentsInChildren<LineRenderer>();
            // for (int i = 0; i < line.Length; i++)
            // {
            //     line[i].enabled = true;
            // }
            waitForHit = false;
            // gameObject.GetComponent<LaserController>().SetLength(0.0f);
            // LineManager.AddLineRenderer(this.transform.GetComponentInChildren<LineRenderer>());
            // Debug.Log("Hit");
        }
        // isHit = false;

        
    }
    // }
    // // 被射线击中时才激活自身的LineRender，获取其方向并旋转
    // // private void OnCollisionEnter(Collision other) {
    // //     Debug.Log("Hited By:" + other.gameObject.name);
    // // }
    public void hitByLaser(Vector2 refractDirection, Vector2 reflectDirection, Vector2 hitPoistion, float dotProduct)
    {
        if(!isHit)
        {
            isHit = true;
            // GetComponentInChildren<LineRenderer>().enabled = true;
            // // 获取射线方向
            // Vector3 direction = GetComponentInChildren<LineRenderer>().GetPosition(1) - GetComponentInChildren<LineRenderer>().GetPosition(0);
            // 旋转自身

            // 如果是镜子表面，执行反射
            if (planeType == PlaneType.Mirror){
                ReflectionPoint.position = hitPoistion;
                ReflectionPoint.right = reflectDirection;
            }
            // 如果是水面且光线从上方来，执行反射和折射
            else if (planeType == PlaneType.water && dotProduct < 0){
                ReflectionPoint.position = hitPoistion;
                ReflectionPoint.right = reflectDirection;
                // 折射
                RefractionPoint.position = hitPoistion;
                RefractionPoint.right = refractDirection;
            }
            // 如果从水面下方来，只折射
            else if (planeType == PlaneType.water && dotProduct >= 0)
            {
                
                RefractionPoint.position = hitPoistion;
                RefractionPoint.right = refractDirection;
            }

            // if(!LineManager.lineRenderers.Contains(this.transform.GetComponentInChildren<LineRenderer>())) 
            // LineManager.AddLineRenderer(this.transform.GetComponentInChildren<LineRenderer>());
            foreach (LineRenderer line in this.transform.GetComponentsInChildren<LineRenderer>())
            {
                if (!LineManager.lineRenderers.Contains(line)) LineManager.AddLineRenderer(line);
            }
        }
        else
        {
             if (planeType == PlaneType.Mirror){
                ReflectionPoint.position = hitPoistion;
                ReflectionPoint.right = reflectDirection;
            }
            // 如果是水面，执行反射和折射
            else if (planeType == PlaneType.water){
                ReflectionPoint.position = hitPoistion;
                ReflectionPoint.right = reflectDirection;
                // 折射
                RefractionPoint.position = hitPoistion;
                RefractionPoint.right = refractDirection;


            }
            return;
        }
    }

    public void unHitted()
    {
        isHit = false;
        
    }
}
