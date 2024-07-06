using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public LineRenderer Line;
    public static LineRenderer[] lineRenderers; // 所有LineRenderer的数组
    public float threshold = 0.1f; // 悬浮检测的阈值
    private Vector2 lastMousePosition;
    private GameObject currentPrefabInstance; // 当前场景中的Prefab实例
    public static GameObject currentPreview;
    public static GameObject currentPointer;

    // 测试用
    public GameObject testPointer;
    public GameObject testPreview;

    // Start is called before the first frame update
    void Start()
    {
        AddLineRenderer(Line);
        ChangeCurrentPointer(testPointer, testPreview);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (var lineRenderer in lineRenderers)
        {
            if(lineRenderer == null)
            {
                continue;
            }
            if (IsMouseOverLine(lineRenderer, mousePosition, threshold))
            {
                Debug.Log("鼠标当前悬浮在直线 " + lineRenderer.gameObject.name + " 上");
                
            }
        }
    }

    public static void AddLineRenderer(LineRenderer lineRenderer)
    {
        if (lineRenderers == null)
        {
            lineRenderers = new LineRenderer[1];
            lineRenderers[0] = lineRenderer;
        }
        else
        {
            LineRenderer[] newLineRenderers = new LineRenderer[lineRenderers.Length + 1];
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                newLineRenderers[i] = lineRenderers[i];
            }
            newLineRenderers[lineRenderers.Length] = lineRenderer;
            lineRenderers = newLineRenderers;
        }
    }

    public static void DeleteLineRenderer(LineRenderer lineRenderer)
    {
        if (lineRenderers == null)
        {
            return;
        }
        else
        {
            LineRenderer[] newLineRenderers = new LineRenderer[lineRenderers.Length - 1];
            int j = 0;
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                if (lineRenderers[i] != lineRenderer)
                {
                    newLineRenderers[j] = lineRenderers[i];
                    j++;
                }
            }
            lineRenderers = newLineRenderers;
        }
    }

     bool IsMouseOverLine(LineRenderer line, Vector2 mousePos, float threshold)
    {
        for (int i = 0; i < line.positionCount - 1; i++)
        {
            Vector2 start = line.GetPosition(i);
            Vector2 end = line.GetPosition(i + 1);
            if (PointToLineDistance(mousePos, start, end) < threshold)
            {
                Vector2 currentMousePosition = mousePos;
                // Vector2 currentMousePosition = mousePos;
                // Debug.Log("鼠标悬浮在线上");
                if(currentMousePosition != lastMousePosition)
                {
                    if (currentPrefabInstance != null)
                    {
                        Destroy(currentPrefabInstance); // 如果存在旧的Prefab实例，则销毁它
                    }   
                    currentPrefabInstance = Instantiate(currentPreview, ClosestPointOnLineSegment(currentMousePosition, start, end), Quaternion.identity);

                }
                // 按左键添加实例点，并更改光线的起始点和终点
                if(Input.GetMouseButtonDown(0)){
                    // 计算方向
                    Vector3 direction = (end - start).normalized;
                    GameObject newPonit = Instantiate(currentPointer, ClosestPointOnLineSegment(mousePos, start, end), Quaternion.identity);
                    newPonit.transform.right = direction;
                    LineRenderer newLine = newPonit.GetComponentInChildren<LineRenderer>();
                    // newLine.SetPosition(0, newPonit.transform.position + new Vector3(1f, 0, 0));
                    // Debug.Log("newPoint.position: " + newPonit.transform.position);
                    // Debug.Log("newLine.position: " + newLine.transform.position);
                    // newLine.SetPosition(1, end);

                    AddLineRenderer(newLine);

                }
                return true;
            }
        }
        return false;
    }

    float PointToLineDistance(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        Vector2 lineVec = lineEnd - lineStart;
        Vector2 pointVec = point - lineStart;
        Vector2 lineUnitVec = lineVec.normalized;
        float projectedLength = Vector2.Dot(pointVec, lineUnitVec);
        Vector2 closestPoint = lineStart + lineUnitVec * projectedLength;
        float distanceFromLine = (point - closestPoint).magnitude;
        if (projectedLength < 0 || projectedLength > lineVec.magnitude)
        {
            return Mathf.Min((point - lineStart).magnitude, (point - lineEnd).magnitude);
        }
        return distanceFromLine;
    }
    // 计算点到线段的最近点，即点吸附在直线上的位置
    Vector2 ClosestPointOnLineSegment(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        Vector2 AP = point - lineStart; // 点到线段起点的向量
        Vector2 AB = lineEnd - lineStart; // 线段的向量
        float magnitudeAB = AB.magnitude; // 线段长度
        float ABAPproduct = Vector2.Dot(AP, AB); // 向量点乘
        float distance = ABAPproduct / (magnitudeAB * magnitudeAB); // 归一化点乘结果，得到投影点的位置

        if (distance < 0)
            return lineStart;
        else if (distance > 1)
            return lineEnd;
        else
            return lineStart + AB * distance; // 投影点
    }
    // 更改当前的point和pointpreview
    public static void ChangeCurrentPointer(GameObject pointer, GameObject preview)
    {
        currentPointer = pointer;
        currentPreview = preview;
    }
}
