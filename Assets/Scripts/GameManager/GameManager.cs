//using QZGameFramework.DebuggerSystem;
using UnityEngine;

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

    private void OnDestroy()
    {
        if (instance == this)
        {
            Destroy(this.gameObject);
        }
        instance = null;
    }
}