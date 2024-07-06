using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isMouseOver = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && isMouseOver)
        {
            Destroy(this.gameObject.transform.parent.gameObject);
            LineManager.DeleteLineRenderer(this.gameObject.transform.parent.gameObject.GetComponentInChildren<LineRenderer>());
        }
    }

    public void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
        isMouseOver = true;
        
    }

    public void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        isMouseOver = false;
    }
}
