//using QZGameFramework.DebuggerSystem;
using QZGameFramework.GFInputManager;
using QZGameFramework.GFSceneManager;
using QZGameFramework.UIManager;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//using UnityEngine.InputSystem;

/// <summary>
/// 游戏一开始就会创建的全局唯一的游戏管理器
/// 如有需要初始化的脚本、数据等，可以在这里进行初始化
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private static bool isInit = false; // 是否已经初始化过

    private bool isShowGameMainWindow;
    public bool IsShowGameMainWindow { get => isShowGameMainWindow; set => isShowGameMainWindow = value; }
    public static int BlockerNum;
    public static int DiffractionNum;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitalizeGameManager()
    {
        GameObject obj;
        // 已经初始化过 则不再进行初始化
        if (isInit)
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
                DontDestroyOnLoad(instance.gameObject);
            }

            Debug.LogError("GameManager Has Initialized.");
            return;
        }

        obj = new GameObject("GameManager");
        instance = obj.AddComponent<GameManager>();
        DontDestroyOnLoad(obj);

        SingletonManager.Initialize();

        isInit = true;
    }

    private void Awake()
    {
    }

    private void OnEnable()
    {
        InputMgr.Instance.Enable();
        InputMgr.Instance.RegisterCommand(KeyCode.Escape, KeyPressType.Down, ShowGameMainWindow);
    }

    private void ShowGameMainWindow(KeyCode keycode)
    {
        if (SceneManager.GetActiveScene().name == "BeginScene")
        {
            return;
        }
        IsShowGameMainWindow = !IsShowGameMainWindow;
        if (IsShowGameMainWindow)
        {
            UIManager.Instance.ShowWindow<GameMainWindow>();
        }
        else
        {
            UIManager.Instance.HideWindow<GameMainWindow>();
        }
    }

    private void OnDisable()
    {
        InputMgr.Instance.RemoveCommand(KeyCode.Escape, KeyPressType.Down, ShowGameMainWindow);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            Destroy(this.gameObject);
        }
        InputMgr.Instance.Disable();
        instance = null;
    }
}