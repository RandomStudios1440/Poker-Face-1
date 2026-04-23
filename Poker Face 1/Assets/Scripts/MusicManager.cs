using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private bool shuffleOnStart = true;
    [SerializeField] private bool loopPlaylist = true;
    [SerializeField] private string[] scenesToMuteMusic;

    [Header("Secondary Track")]
    [SerializeField] private AudioSource secondaryAudioSource;
    [SerializeField] private AudioClip secondaryTrack;
    [SerializeField] private string[] scenesForSecondaryTrack = { "Store", "Merchant", "Inventory" };

    private bool wasInSecondaryScene = false;

    private int currentTrackIndex = 0;
    private int[] shuffledIndices;

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
            return;
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;

        float savedVolume = PlayerPrefs.GetFloat("musicVolume", 100f);
        AudioListener.volume = savedVolume / 100f;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckIfShouldPlayMusic(scene.name);
    }

    private bool IsSecondaryScene(string sceneName)
    {
        foreach (string s in scenesForSecondaryTrack)
            if (sceneName == s) return true;
        return false;
    }

    private void CheckIfShouldPlayMusic(string sceneName)
    {
        bool shouldMute = false;
        foreach (string mutedScene in scenesToMuteMusic)
        {
            if (sceneName == mutedScene)
            {
                shouldMute = true;
                break;
            }
        }

        if (shouldMute)
        {
            audioSource.Pause();
            if (secondaryAudioSource != null) secondaryAudioSource.Stop();
            wasInSecondaryScene = false;
            return;
        }

        if (IsSecondaryScene(sceneName))
        {
            audioSource.Pause();
            if (secondaryAudioSource != null && secondaryTrack != null)
            {
                secondaryAudioSource.loop = true;
                if (!wasInSecondaryScene || secondaryAudioSource.clip != secondaryTrack)
                {
                    secondaryAudioSource.clip = secondaryTrack;
                    secondaryAudioSource.Play();
                }
                else if (!secondaryAudioSource.isPlaying)
                {
                    secondaryAudioSource.Play();
                }
                // if already playing, do nothing — let it continue
            }
            wasInSecondaryScene = true;
        }
        else
        {
            // leaving a secondary scene — stop secondary, resume main
            if (wasInSecondaryScene && secondaryAudioSource != null)
                secondaryAudioSource.Stop();

            wasInSecondaryScene = false;

            if (!audioSource.isPlaying)
            {
                audioSource.time = 0;
                audioSource.Play();
            }
        }
    }

    private void Start()
    {
        if (musicTracks.Length > 0)
        {
            if (shuffleOnStart)
                ShufflePlaylist();
            PlayNextTrack();
        }
    }

    private void Update()
    {
        if (!wasInSecondaryScene && !audioSource.isPlaying && musicTracks.Length > 0)
        {
            PlayNextTrack();
        }
    }

    private void ShufflePlaylist()
    {
        shuffledIndices = new int[musicTracks.Length];
        for (int i = 0; i < shuffledIndices.Length; i++)
            shuffledIndices[i] = i;

        for (int i = shuffledIndices.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = shuffledIndices[i];
            shuffledIndices[i] = shuffledIndices[randomIndex];
            shuffledIndices[randomIndex] = temp;
        }

        currentTrackIndex = 0;
    }

    private void PlayNextTrack()
    {
        if (musicTracks.Length == 0) return;

        int trackIndex = shuffleOnStart ? shuffledIndices[currentTrackIndex] : currentTrackIndex;
        audioSource.clip = musicTracks[trackIndex];
        audioSource.Play();

        currentTrackIndex++;

        if (currentTrackIndex >= musicTracks.Length)
        {
            if (loopPlaylist)
            {
                if (shuffleOnStart)
                    ShufflePlaylist();
                else
                    currentTrackIndex = 0;
            }
        }
    }

    public void PlayRandomTrack()
    {
        if (musicTracks.Length == 0) return;

        int randomIndex = Random.Range(0, musicTracks.Length);
        audioSource.clip = musicTracks[randomIndex];
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.UnPause();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
