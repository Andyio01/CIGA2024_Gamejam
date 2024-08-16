using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // 拖拽你的Panel到这里
    private bool isPaused = false;

    private void Start()
    {
        
         Resume();
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);  // 隐藏暂停页面
        Time.timeScale = 1f;           // 恢复游戏
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);   // 显示暂停页面
        Time.timeScale = 0f;           // 暂停游戏
        isPaused = true;
    }

    
}