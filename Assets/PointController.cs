using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    public bool isHit = true;
    private bool isMouseOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
    //     // 判断鼠标右键是否被按下
    //     if (Input.GetMouseButtonDown(1)) // 1 是鼠标右键
    //     {
    //         // 从摄像机到鼠标位置发射一条射线
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         RaycastHit hit;

    //         // 进行射线投射，检测射线是否击中了物体
    //         if (Physics.Raycast(ray, out hit))
    //         {
    //             // 检查射线是否击中了当前对象
    //             if (hit.collider.gameObject == this.gameObject)
    //             {
    //                 Debug.Log("Hit Point");
    //                 // 在此处调用你的事件处理函数
    //                 Destroy(this.gameObject);
    //             }
    //         }
    //     }
        // if(!GetComponentInChildren<LineRenderer>().enabled)
        // {
        // }
        if (!isHit)
        {
            // 没有前置光线时关闭LineRender
            if(gameObject.transform.tag != "Blocker") GetComponentInChildren<LineRenderer>().enabled = false;
            // gameObject.GetComponent<LaserController>().SetLength(0.0f);
            StartCoroutine(WaitAndDestroy(1f));
            Debug.Log("No Hit");
        }
        else{
            // 有前置光线时开启LineRender
            if(gameObject.transform.tag != "Blocker") GetComponentInChildren<LineRenderer>().enabled = true;
            // gameObject.GetComponent<LaserController>().SetLength(0.0f);
            StopAllCoroutines();
            Debug.Log("Hit");
        }
        isHit = false;

        
    }
    // }
    // // 被射线击中时才激活自身的LineRender，获取其方向并旋转
    // // private void OnCollisionEnter(Collision other) {
    // //     Debug.Log("Hited By:" + other.gameObject.name);
    // // }
    public void hitByLaser(LineRenderer line)
    {
        isHit = true;
        // GetComponentInChildren<LineRenderer>().enabled = true;
        // // 获取射线方向
        // Vector3 direction = GetComponentInChildren<LineRenderer>().GetPosition(1) - GetComponentInChildren<LineRenderer>().GetPosition(0);
        // 旋转自身
        // transform.right = direction;
        this.GetComponentInChildren<MouseHover>().SetCurrentLine(line);
    }

    IEnumerator WaitAndDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
        // LineManager.DeleteLineRenderer(this.gameObject.GetComponentInChildren<LineRenderer>());

    }
    
}
