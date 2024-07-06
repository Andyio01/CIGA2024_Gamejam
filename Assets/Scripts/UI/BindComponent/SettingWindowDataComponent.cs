/* ------------------------------------
/* Title: SettingWindow组件类
/* Creation Time: 2024/7/6 12:28:25
/* Author: Rock
/* Description: It is used to mount the corresponding Window object and automatically obtain UI components.
/* 描述: 用于挂载在对应的Window物体上，自动获取UI组件。
/* 此文件为自动生成，请尽量不要修改，重新生成将会覆盖原有修改！！！
--------------------------------------- */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QZGameFramework.UIManager
{
	[DisallowMultipleComponent]
	public class SettingWindowDataComponent : MonoBehaviour
	{
		[ReadOnly] public Button CloseButton;
		[ReadOnly] public Toggle MusicToggle;
		[ReadOnly] public Slider MusicSlider;
		[ReadOnly] public Toggle SoundToggle;
		[ReadOnly] public Slider SoundSlider;

		public void InitUIComponent(WindowBase target)
		{
			// 组件事件绑定
			SettingWindow mWindow = target as SettingWindow;
			target.AddButtonClickListener(CloseButton,mWindow.OnCloseButtonClick);
			target.AddToggleClickListener(MusicToggle,mWindow.OnMusicToggleChange);
			target.AddToggleClickListener(SoundToggle,mWindow.OnSoundToggleChange);
		}
	}
}
