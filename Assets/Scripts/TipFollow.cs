using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;  // 如果你使用TextMeshPro

public class TipFollow : MonoBehaviour
{
    public RectTransform tooltipUI;
    public Text tooltipText;  // 如果使用TextMeshPro，或者使用UnityEngine.UI.Text
    public Vector2 offset = new Vector2(10, 10);  // 设置Tooltip偏移量
    public GameObject BlockerPrefab;
    public GameObject BlockerPreview;
    public GameObject DiffractionPrefab;
    public GameObject DiffractionPreview;
    private bool isBlocker = true;
    public AudioSource ChangePointSound;
    void Start()
    {
        if(isBlocker)
        {
            LineManager.ChangeCurrentPointer(BlockerPrefab, BlockerPreview);
        }
        else
        {
            LineManager.ChangeCurrentPointer(DiffractionPrefab, DiffractionPreview);
        
        }
    }
    void FixedUpdate()
    {
        Vector2 pos = CursorManager.Instance.GetMousePossition();

        // 应用偏移，使Tooltip位于鼠标的右上方
        tooltipUI.position = pos + offset;

        // 设置Tooltip文本，可以根据需求进行动态更改
        tooltipText.text = isBlocker ? GameManager.BlockerNum.ToString() : GameManager.DiffractionNum.ToString();
    }
    void Update(){
        // 监听鼠标滚轮输入
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        // 向前滚动切换Blocker
        if (scroll > 0f)
        {
            if(!ChangePointSound.isPlaying) ChangePointSound.Play();
            isBlocker = true;
            LineManager.ChangeCurrentPointer(BlockerPrefab, BlockerPreview);
            Debug.Log("滚轮向前, 当前prefab为: " + LineManager.currentPointer.name);
            tooltipText.color = Color.red;
        }
        else if (scroll < 0f)
        {
            if (!ChangePointSound.isPlaying) ChangePointSound.Play();
            isBlocker = false;
            LineManager.ChangeCurrentPointer(DiffractionPrefab, DiffractionPreview);
            Debug.Log("滚轮向后, 当前prefab为: " + LineManager.currentPointer.name);
            tooltipText.color = Color.blue;
        }
        
    }

    public void ChangeToBlocker()
    {
        Debug.Log("Change to Blocker");
        LineManager.ChangeCurrentPointer(BlockerPrefab, BlockerPreview);
    }

    public void ChangeToDiffraction()
    {
        Debug.Log("Change to Diffraction");
        LineManager.ChangeCurrentPointer(DiffractionPrefab, DiffractionPreview);
    }
}