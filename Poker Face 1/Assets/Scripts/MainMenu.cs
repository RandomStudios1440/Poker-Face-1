using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Store()
    {
        SceneTransition.Instance.LoadScene(2);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Menu()
    {
        SceneTransition.Instance.LoadScene(0);
    }
    public void Options()
    {
        SceneTransition.Instance.LoadScene(3);
    }
    public void Play()
    {
        SceneTransition.Instance.LoadScene(4);
    }
    public void Begin()
    {
        SceneTransition.Instance.LoadScene(5);
    }
}