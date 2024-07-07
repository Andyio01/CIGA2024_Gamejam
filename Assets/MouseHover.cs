using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MouseHover : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isMouseOver = false;
    private bool Dragging = false;
    public float distance;  // 端点之间的距离
    private LineRenderer currentLine;
    public bool IsBlocker;
    public Texture2D DefaultCursor;
    public Texture2D DragCursor;
    public Texture2D InteractableCursor;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && isMouseOver)
        {
            if(this.IsBlocker)
            {
                GameManager.BlockerNum++;
                Debug.Log("当前BlockerNum: " + GameManager.BlockerNum);
            }
            else
            {
                GameManager.DiffractionNum++;
                Debug.Log("当前DiffractionNum: " + GameManager.DiffractionNum);
                LineManager.DeleteLineRenderer(currentLine);

            }
            Destroy(this.gameObject.transform.parent.gameObject);
            // LineManager.DeleteLineRenderer();
        }
        // if(Input.GetMouseButtonDown(0) && isMouseOver)
        // {
        //     Debug.Log("current line: " + currentLine.transform.parent.transform.parent.gameObject.name);
        // }
        if (Input.GetMouseButtonUp(0))
        {
            Dragging = false;
            // this.gameObject.transform.parent.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            this.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        }
        if (Dragging && IsBlocker)
        {
            Transform pivot = currentLine.transform.parent.parent.gameObject.transform;
            Vector2 mousePosition = CursorManager.Instance.GetMousePossition();
            Vector2 direction = (mousePosition - (Vector2)pivot.position).normalized;
            pivot.gameObject.GetComponent<LaserController>().EmissionPoint.transform.position = pivot.position + (Vector3)(direction * 0.25f);
            // pivot.right = direction;

        }
    }
    private void FixedUpdate() {
        
    
        if (Dragging && IsBlocker)
        {
            // 获取旋转中心
            Transform pivot = currentLine.transform.parent.parent.gameObject.transform;
            Debug.Log("Pivot: " + pivot.position);
            distance = Vector2.Distance(pivot.position, this.gameObject.transform.parent.position);
            Debug.Log("Distance: " + distance);
            Vector2 mousePosition = CursorManager.Instance.GetMousePossition();
            Vector2 direction = (mousePosition - (Vector2)pivot.position).normalized;
            transform.parent.position = pivot.position + (Vector3)(direction * distance);  // 维持半径不变
            pivot.right = direction;
        }
    }
        
    

    public void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
        this.gameObject.transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.5f);
        isMouseOver = true;
        
    }

    public void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
        this.gameObject.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        isMouseOver = false;
    }

 
    void OnMouseDrag()
    {
        Dragging = true;
        this.gameObject.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f);

        Debug.Log("Dragging");
    }

    public void SetCurrentLine(LineRenderer line)
    {
        this.currentLine = line;
    }
}
