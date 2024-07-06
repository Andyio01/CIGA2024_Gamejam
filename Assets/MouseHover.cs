using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isMouseOver = false;
    private bool Dragging = false;
    public float distance;  // 端点之间的距离
    private LineRenderer currentLine;
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
        // if(Input.GetMouseButtonDown(0) && isMouseOver)
        // {
        //     Debug.Log("current line: " + currentLine.transform.parent.transform.parent.gameObject.name);
        // }

        if (Dragging)
        {
            // 获取旋转中心
            Transform pivot = currentLine.transform.parent.parent.gameObject.transform;

            distance = Vector2.Distance(pivot.position, this.gameObject.transform.position);
            Debug.Log("Distance: " + distance);
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)pivot.position).normalized;
            transform.position = pivot.position + (Vector3)(direction * distance);  // 维持半径不变
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

 
    void OnMouseDrag()
    {
        Dragging = true;
        Debug.Log("Dragging");
    }

    public void SetCurrentLine(LineRenderer line)
    {
        this.currentLine = line;
    }
}
