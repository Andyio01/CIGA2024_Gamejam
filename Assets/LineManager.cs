using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    private static LineRenderer[] lineRenderers; // 所有LineRenderer的数组
    public float threshold = 0.1f; // 悬浮检测的阈值
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
