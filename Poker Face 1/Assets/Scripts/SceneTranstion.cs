using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(TransitionToScene(sceneIndex));
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        // Fade out
        yield return StartCoroutine(Fade(1f));

        // Load scene
        SceneManager.LoadScene(sceneName);

        // Fade in
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator TransitionToScene(int sceneIndex)
    {
        // Fade out
        yield return StartCoroutine(Fade(1f));

        // Load scene
        SceneManager.LoadScene(sceneIndex);

        // Fade in
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}