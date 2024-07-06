using UnityEngine;

public class TestMove : MonoBehaviour
{
    public float speed = 1.0f;

    void Update()
    {
        // 控制点的移动
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}