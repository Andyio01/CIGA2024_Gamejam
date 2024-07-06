using QZGameFramework.UIManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏启动脚本
/// </summary>
public class GameMain : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.ShowWindow<BeginWindow>();
    }

    private void Start()
    {
        Debug.Log("这是游戏启动脚本，启动初始化逻辑在这个脚本里编写，也可在GameManager里编写");
    }

    private void Update()
    {
    }
}