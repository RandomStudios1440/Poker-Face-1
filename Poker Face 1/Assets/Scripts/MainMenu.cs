using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(1);
        else
            SceneManager.LoadScene(1);
    }
    public void Store()
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(2);
        else
            SceneManager.LoadScene(2);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Menu()
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(0);
        else
            SceneManager.LoadScene(0);
    }
    public void Options()
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(3);
        else
            SceneManager.LoadScene(3);
    }
    public void Play()
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(4);
        else
            SceneManager.LoadScene(4);
    }
    public void Begin()
    {
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.LoadScene(5);
        else
            SceneManager.LoadScene(5);

    }
}   