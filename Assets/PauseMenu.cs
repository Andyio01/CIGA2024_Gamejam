using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // ��ק���Panel������
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
        pauseMenuUI.SetActive(false);  // ������ͣҳ��
        Time.timeScale = 1f;           // �ָ���Ϸ
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);   // ��ʾ��ͣҳ��
        Time.timeScale = 0f;           // ��ͣ��Ϸ
        isPaused = true;
    }

    
}