using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMouseClick : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer Line;
    public GameObject StartPoint;
    public GameObject BlockerPrefab;
    public GameObject DiffractionPrefab;
    public float threshold = 0.001f;
    public enum pointerType {Blocker, Diffraction};
    private GameObject currentPointer;
    private Vector2 lastMousePosition;
    private GameObject currentPrefabInstance; // 当前场景中的Prefab实例
    [SerializeField]
    private pointerType selectedOption;
    void Start()
    {
        switch(selectedOption)
        {
            case pointerType.Blocker:
                currentPointer = BlockerPrefab;
                break;
            case pointerType.Diffraction:
                currentPointer = DiffractionPrefab;
                break;
            default:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
         // 检测鼠标点击
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        for (int i = 0; i < Line.positionCount - 1; i++)
        {
            Vector2 start = Line.GetPosition(i);
            Vector2 end = Line.GetPosition(i + 1);
            if (PointToLineDistance(mousePos, start, end) < threshold)
            {
                Vector2 currentMousePosition = mousePos;
                Debug.Log("鼠标悬浮在线上");
                // TODO: 在鼠标悬浮的位置生成一个虚影
                if(currentMousePosition != lastMousePosition)
                {
                    if (currentPrefabInstance != null)
                    {
                        Destroy(currentPrefabInstance); // 如果存在旧的Prefab实例，则销毁它
                    }   
                    currentPrefabInstance = Instantiate(currentPointer, ClosestPointOnLineSegment(currentMousePosition, start, end), Quaternion.identity);

                }
                if(Input.GetMouseButtonDown(0)){
                Instantiate(currentPointer, ClosestPointOnLineSegment(mousePos, start, end), Quaternion.identity);
                
                break;
                }
            }
        }

            
    }

    // 计算点到线段的最短距离
    float PointToLineDistance(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        float lineLength = (lineEnd - lineStart).magnitude;
        if (lineLength == 0) return (point - lineStart).magnitude;

        float t = Mathf.Max(0, Mathf.Min(1, Vector2.Dot(point - lineStart, lineEnd - lineStart) / (lineLength * lineLength)));
        Vector2 projection = lineStart + t * (lineEnd - lineStart);
        return (point - projection).magnitude;
    }

    // 计算点到线段的最近点
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
}
