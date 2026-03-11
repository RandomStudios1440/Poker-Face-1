using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private UnityEngine.UI.Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (fadeCanvasGroup != null)
            {
                DontDestroyOnLoad(fadeCanvasGroup.transform.root.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName, fadeDuration, Color.black));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(TransitionToScene(sceneIndex, fadeDuration, Color.black));
    }

    public void LoadScene(string sceneName, float customDuration)
    {
        StartCoroutine(TransitionToScene(sceneName, customDuration, Color.black));
    }

    public void LoadScene(int sceneIndex, float customDuration)
    {
        StartCoroutine(TransitionToScene(sceneIndex, customDuration, Color.black));
    }

    public void LoadScene(string sceneName, float customDuration, Color fadeColor)
    {
        StartCoroutine(TransitionToScene(sceneName, customDuration, fadeColor));
    }

    public void LoadScene(int sceneIndex, float customDuration, Color fadeColor)
    {
        StartCoroutine(TransitionToScene(sceneIndex, customDuration, fadeColor));
    }

    private IEnumerator TransitionToScene(string sceneName, float duration, Color fadeColor)
    {
        fadeImage.color = fadeColor;
        yield return StartCoroutine(Fade(1f, duration));
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(Fade(0f, duration));
    }

    private IEnumerator TransitionToScene(int sceneIndex, float duration, Color fadeColor)
    {
        fadeImage.color = fadeColor;
        yield return StartCoroutine(Fade(1f, duration));
        SceneManager.LoadScene(sceneIndex);
        yield return StartCoroutine(Fade(0f, duration));
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsed = 0f;

        fadeCanvasGroup.blocksRaycasts = targetAlpha > 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
        fadeCanvasGroup.blocksRaycasts = targetAlpha > 0;
    }
}
