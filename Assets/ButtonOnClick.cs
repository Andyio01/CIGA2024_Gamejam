using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadBeginScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("BeginScene");
    }
    public void LoadLevel01()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level01");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
