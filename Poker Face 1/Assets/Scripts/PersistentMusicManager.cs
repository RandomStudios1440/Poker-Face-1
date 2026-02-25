using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusicManager : MonoBehaviour
{
    public static PersistentMusicManager Instance;
    
    public AudioClip musicTrack;
    private AudioSource audioSource;
    
    public string[] scenesWithoutMusic;
    
    private bool wasPlayingInPreviousScene = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
            
            audioSource.clip = musicTrack;
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void Start()
    {
        CheckCurrentScene();
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckCurrentScene();
    }
    
    void CheckCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        bool shouldPlayMusic = !IsSceneForbidden(currentScene);
        
        if (shouldPlayMusic)
        {
            if (!wasPlayingInPreviousScene)
            {
                audioSource.Stop();
                audioSource.Play();
            }
            else if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            wasPlayingInPreviousScene = true;
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            wasPlayingInPreviousScene = false;
        }
    }
    
    bool IsSceneForbidden(string sceneName)
    {
        foreach (string forbiddenScene in scenesWithoutMusic)
        {
            if (forbiddenScene == sceneName)
                return true;
        }
        return false;
    }
    
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
    
    public void StopMusic()
    {
        audioSource.Stop();
        wasPlayingInPreviousScene = false;
    }
}
