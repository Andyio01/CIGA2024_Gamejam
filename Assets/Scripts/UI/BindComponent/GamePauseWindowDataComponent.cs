/* ------------------------------------
/* Title: GamePauseWindow组件类
/* Creation Time: 2024/7/7 11:02:19
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
	public class GamePauseWindowDataComponent : MonoBehaviour
	{
		[ReadOnly] public Button CloseButton;
		[ReadOnly] public Button ContinueButton;
		[ReadOnly] public Button SettingButton;
		[ReadOnly] public Button MainMenuButton;

		public void InitUIComponent(WindowBase target)
		{
			// 组件事件绑定
			GamePauseWindow mWindow = target as GamePauseWindow;
			target.AddButtonClickListener(CloseButton,mWindow.OnCloseButtonClick);
			target.AddButtonClickListener(ContinueButton,mWindow.OnContinueButtonClick);
			target.AddButtonClickListener(SettingButton,mWindow.OnSettingButtonClick);
			target.AddButtonClickListener(MainMenuButton,mWindow.OnMainMenuButtonClick);
		}
	}
}
