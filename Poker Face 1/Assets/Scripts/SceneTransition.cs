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
        if (fadeImage != null)
        {
            fadeImage.color = fadeColor;
        }
        yield return StartCoroutine(Fade(1f, duration));
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(Fade(0f, duration));
    }

    private IEnumerator TransitionToScene(int sceneIndex, float duration, Color fadeColor)
    {
        if (fadeImage != null)
        {
            fadeImage.color = fadeColor;
        }
        yield return StartCoroutine(Fade(1f, duration));
        SceneManager.LoadScene(sceneIndex);
        yield return StartCoroutine(Fade(0f, duration));
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        if (fadeCanvasGroup == null) yield break;

        fadeCanvasGroup.blocksRaycasts = targetAlpha > 0;

        if (targetAlpha > 0)
        {
            fadeCanvasGroup.alpha = 1f;
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            float elapsed = 0f;
            float fadeDur = duration * 0.3f;
            while (elapsed < fadeDur)
            {
                elapsed += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDur);
                yield return null;
            }
        }

        fadeCanvasGroup.alpha = targetAlpha;
        fadeCanvasGroup.blocksRaycasts = targetAlpha > 0;
    }
}
