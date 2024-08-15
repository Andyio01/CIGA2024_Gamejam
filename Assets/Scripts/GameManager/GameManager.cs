//using QZGameFramework.DebuggerSystem;
using QZGameFramework.GFInputManager;
using QZGameFramework.GFSceneManager;
using QZGameFramework.UIManager;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.Rendering;
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

    private bool isShowGamePauseWindow;
    public bool IsShowGamePauseWindow { get => isShowGamePauseWindow; set => isShowGamePauseWindow = value; }
    public static int BlockerNum;
    public static int DiffractionNum;
    public GameObject initialEmitter;
    public static GameObject curEmitter;
    public static float curRotation;

    public Texture2D DefaultCursor;
    public Texture2D DragCursor;
    public Texture2D InteractableCursor;
    // private static bool isEnding = false;
    // public GameObject globalVolume;
    // public GameObject EndPanel;

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
        isShowGamePauseWindow = !isShowGamePauseWindow;
        if (isShowGamePauseWindow)
        {
            UIManager.Instance.ShowWindow<GamePauseWindow>();
        }
        else
        {
            UIManager.Instance.HideWindow<GamePauseWindow>();
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
    private void Start()
    {
        curEmitter = initialEmitter;
    }

    public static void setEmitter(GameObject Emitter)
    {
        curEmitter = Emitter;
        curRotation = Emitter.transform.rotation.z;
    }
    public static GameObject getEmitter()
    {
        return curEmitter;
    }

    // public static void GameEnd()
    // {
    //     if (isEnding) return;
    //     else
    //     {
    //         isEnding = true;
    //         // PostProcessProfile profile = instance.GetComponent<PostProcessProfile>();
    //         // Bloom bloom = profile.GetSetting<Bloom>();
    //         // bloom.threshold.value = Mathf.Lerp(instance.globalVolume.GetComponent<Bloom>().threshold.value, 0f, 1.5f);
    //         instance.EndPanel.GetComponent<Image>().DOColor(new Color(255, 255, 255, 255), 1.5f).OnComplete(() =>
    //         {
    //             SceneManager.LoadScene("EndScene");
    //         });
    //     }
    //     // SceneManager.LoadScene("EndScene");
    // }
}