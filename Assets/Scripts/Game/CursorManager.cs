using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    public Vector2 GetMousePossition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetCursorIcon(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
        Debug.Log("指针设置为：" + texture.name);
    }
}