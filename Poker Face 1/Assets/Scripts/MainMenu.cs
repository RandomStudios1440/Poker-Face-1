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
        SceneManager.LoadScene(2);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Options()
    {
        SceneManager.LoadScene(3);
    }
}