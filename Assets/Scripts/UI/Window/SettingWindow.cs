/* ------------------------------------
/* Title: SettingWindow类
/* Creation Time: 2024/7/6 12:16:23
/* Author: Rock
/* Description: This is the class used to bind the Window prefab.
/* 描述: 这是用于绑定 Window 预制体的类。
/* 注意: 如需重新生成此文件，请务必对此 Window 的预制体进行新的修改后，并重新生成对应的 DataComponent 类后再重新生成此文件，否则现有编写的代码将会被全部覆盖为默认代码！！！
--------------------------------------- */

using UnityEngine;
using UnityEngine.UI;
using QZGameFramework.UIManager;
using System;
using static UnityEngine.Rendering.DebugUI;

public class SettingWindow : WindowBase
{
    // UI面板的组件类
    private SettingWindowDataComponent dataCompt;

    private MusicData musicData;

    #region 生命周期函数

    /// <summary>
    /// 在物体显示时执行一次，与Mono Awake一致
    /// </summary>
    public override void OnAwake()
    {
        dataCompt = gameObject.GetComponent<SettingWindowDataComponent>();
        dataCompt.InitUIComponent(this);
        base.OnAwake();
        musicData = GameDataMgr.Instance.musicData;
        SetMusicData(musicData);
    }

    /// <summary>
    /// 在物体显示时执行一次，与Mono OnEnable一致
    /// </summary>
    public override void OnShow()
    {
        base.OnShow();
        dataCompt.MusicSlider.onValueChanged.AddListener(OnMusicChange);
        dataCompt.SoundSlider.onValueChanged.AddListener(OnSoundChange);
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

    private void OnMusicChange(float value)
    {
        GameDataMgr.Instance.musicData.musicVolume = value;
        GameDataMgr.Instance.SaveMusicData();
    }

    private void OnSoundChange(float value)
    {
        GameDataMgr.Instance.musicData.soundVolume = value;
        GameDataMgr.Instance.SaveMusicData();
    }

    public void SetMusicData(MusicData musicData)
    {
        dataCompt.MusicToggle.isOn = musicData.musicOn;
        dataCompt.MusicSlider.value = musicData.musicVolume;
        dataCompt.SoundSlider.value = musicData.soundVolume;
        dataCompt.SoundToggle.isOn = musicData.soundOn;
    }

    #endregion

    #region UI组件事件

    public void OnSoundToggleChange(bool state, Toggle toggle)
    {
        GameDataMgr.Instance.musicData.soundOn = state;
        GameDataMgr.Instance.SaveMusicData();
    }

    public void OnMusicToggleChange(bool state, Toggle toggle)
    {
        GameDataMgr.Instance.musicData.musicOn = state;
        GameDataMgr.Instance.SaveMusicData();
    }

    public void OnCloseButtonClick()
    {
        HideWindow();
    }

    #endregion
}