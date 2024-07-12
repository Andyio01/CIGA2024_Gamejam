using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reciever : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 NextCameraPoisition;
    private bool IsActived = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraMove()
    {
        IsActived = true;
        Debug.Log("Camera Move");
        if(IsActived) Camera.main.transform.position = NextCameraPoisition;
    }
}
