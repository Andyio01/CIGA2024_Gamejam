/* ------------------------------------
/* Title: GameMainWindow类
/* Creation Time: 2024/7/6 18:03:36
/* Author: Rock
/* Description: This is the class used to bind the Window prefab.
/* 描述: 这是用于绑定 Window 预制体的类。
/* 注意: 如需重新生成此文件，请务必对此 Window 的预制体进行新的修改后，并重新生成对应的 DataComponent 类后再重新生成此文件，否则现有编写的代码将会被全部覆盖为默认代码！！！
--------------------------------------- */

using UnityEngine;
using UnityEngine.UI;
using QZGameFramework.UIManager;
using QZGameFramework.GFSceneManager;

public class GameMainWindow : WindowBase
{
    // UI面板的组件类
    private GameMainWindowDataComponent dataCompt;

    #region 生命周期函数

    /// <summary>
    /// 在物体显示时执行一次，与Mono Awake一致
    /// </summary>
    public override void OnAwake()
    {
        dataCompt = gameObject.GetComponent<GameMainWindowDataComponent>();
        dataCompt.InitUIComponent(this);
        base.OnAwake();
    }

    /// <summary>
    /// 在物体显示时执行一次，与Mono OnEnable一致
    /// </summary>
    public override void OnShow()
    {
        base.OnShow();
        GameManager.Instance.IsShowGameMainWindow = true;
    }

    /// <summary>
    /// 在物体隐藏时执行一次，与Mono OnDisable 一致
    /// </summary>
    public override void OnHide()
    {
        base.OnHide();
        GameManager.Instance.IsShowGameMainWindow = false;
    }

    /// <summary>
    ///  在物体销毁时执行一次，与Mono OnDestroy一致
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    #endregion

    #region Custom API Function

    #endregion

    #region UI组件事件

    public void OnCloseButtonClick()
    {
        HideWindow();
    }

    public void OnContinueButtonClick()
    {
        HideWindow();
    }

    public void OnSettingButtonClick()
    {
        //UIManager.Instance.ShowWindow<SettingWindow>();
    }

    public void OnMainMenuButtonClick()
    {
        HideWindow();

        SceneMgr.LoadScene("BeginScene");
    }

    #endregion
}