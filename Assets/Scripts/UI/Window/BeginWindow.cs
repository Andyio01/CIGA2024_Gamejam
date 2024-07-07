/* ------------------------------------
/* Title: BeginWindow类
/* Creation Time: 2024/7/6 10:01:20
/* Author: hhz
/* Description: This is the class used to bind the Window prefab.
/* 描述: 这是用于绑定 Window 预制体的类。
/* 注意: 如需重新生成此文件，请务必对此 Window 的预制体进行新的修改后，并重新生成对应的 DataComponent 类后再重新生成此文件，否则现有编写的代码将会被全部覆盖为默认代码！！！
--------------------------------------- */

using UnityEngine;
using UnityEngine.UI;
using QZGameFramework.UIManager;
using QZGameFramework.GFSceneManager;
using QZGameFramework.PackageMgr.ResourcesManager;
using QZGameFramework.PersistenceDataMgr;

public class BeginWindow : WindowBase
{
    // UI面板的组件类
    private BeginWindowDataComponent dataCompt;

    private MusicData musicData;

    #region 生命周期函数

    /// <summary>
    /// 在物体显示时执行一次，与Mono Awake一致
    /// </summary>
    public override void OnAwake()
    {
        dataCompt = gameObject.GetComponent<BeginWindowDataComponent>();
        dataCompt.InitUIComponent(this);
        base.OnAwake();
        dataCompt.CheckmarkImage.raycastTarget = true;
        musicData = GameDataMgr.Instance.musicData;
        dataCompt.MusicToggle.isOn = musicData.musicOn;
    }

    /// <summary>
    /// 在物体显示时执行一次，与Mono OnEnable一致
    /// </summary>
    public override void OnShow()
    {
        base.OnShow();
    }

    /// <summary>
    /// 在物体隐藏时执行一次，与Mono OnDisable 一致
    /// </summary>
    public override void OnHide()
    {
        base.OnHide();
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

    public void OnMusicToggleChange(bool state, Toggle toggle)
    {
        if (state)
        {
            dataCompt.CheckmarkImage.sprite = ResourcesMgr.Instance.LoadRes<Sprite>("UI/BeginWindow/music");
        }
        else
        {
            dataCompt.CheckmarkImage.sprite = ResourcesMgr.Instance.LoadRes<Sprite>("UI/BeginWindow/music0");
        }
        dataCompt.CheckmarkImage.SetNativeSize();
        GameDataMgr.Instance.musicData.musicOn = state;
        GameDataMgr.Instance.SaveMusicData();
    }

    public void OnQuitGameButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnStartGameButtonClick()
    {
        SceneMgr.LoadScene(BinaryDataMgr.Instance.GetTable<GlobalContainer>().GetData((int)E_Global.LEVEL_01).stringValue);
        HideWindow();
        //UIManager.Instance.ShowWindow<GameMainWindow>();
    }

    #endregion
}