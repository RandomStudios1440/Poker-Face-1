using Unity.VisualScripting;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    public static bool PausedGame = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausedGame) 
            {
                Resume();
            } else
            {
                Pause();
            }
               
        }
    }
        void Resume ()
    {
        pauseMenuUI.SetActive(false);
        PausedGame = false;
    }  
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        PausedGame = true;
    }
}
