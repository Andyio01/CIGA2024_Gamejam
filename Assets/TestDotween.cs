using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TestDotween : MonoBehaviour
{
    public LoopType loopType;
    public int loops;
    // Start is called before the first frame update
    void Start()
    {
        // transform.DOPunchPosition(new Vector3(1, 1, 0), 1, 10, 0.5f);
        // / 目标对象是当前的GameObject
        Transform target = transform;

        // 移动到目标位置，并设置循环
        target.DOMove(new Vector3(0, 5, 0), 2)
              .SetLoops(loops, loopType); // 循环4次，每次递增

    }

    // Update is called once per frame
    void Update()
    {
    }
}
