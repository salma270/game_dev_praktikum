using UnityEngine;
using UnityEngine.SceneManagement;

public class music : MonoBehaviour
{
    public static music instance;

    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        UpdateMusic(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusic(scene.name);
    }

    private void UpdateMusic(string sceneName)
    {
        if (IsMenuScene(sceneName))
        {
            if (audioSource.clip != menuMusic)
            {
                audioSource.clip = menuMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip != gameplayMusic)
            {
                audioSource.clip = gameplayMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    private bool IsMenuScene(string sceneName)
    {
        return sceneName == "mainmenu" || sceneName == "Level";
    }
}
