using System.Collections;
using System.Collections.Generic;
// using System.Collections.Generic;
using UnityEngine;

public class TestLineTrail : MonoBehaviour
{
    public float speed = 1.0f;
    public float pointSpacing = 0.1f;

    private LineRenderer lineRenderer;
    private List<Vector3> points;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        points = new List<Vector3>();
        AddPoint();
    }

    void Update()
    {
        // 控制点的移动
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // 判断是否需要添加新的点
        if (Vector3.Distance(points[points.Count - 1], transform.position) > pointSpacing)
        {
            AddPoint();
        }

        // 更新 Line Renderer 的点
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    void AddPoint()
    {
        points.Add(transform.position);
    }
}
