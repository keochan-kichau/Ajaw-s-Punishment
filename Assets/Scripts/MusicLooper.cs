using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicLooper : MonoBehaviour
{
    public static MusicLooper instance;

    [Header("Danh sách nhạc (Kéo thả nhiều bài vào đây)")]
    public AudioClip[] bgmList;

    [Header("Thời gian chờ (Giây)")]
    private float initialDelay = 2f;
    private float delayBetweenTracks = 10f;

    [Header("Cài đặt Âm lượng")]
    [Range(0f, 1f)]
    public float musicVolume = 0.3f;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.volume = musicVolume;

        audioSource.loop = false;
        audioSource.playOnAwake = false;

        if (bgmList.Length > 0)
        {
            StartCoroutine(PlayMusicWithDelay());
        }
        else
        {
            Debug.LogWarning("Chưa có bài nhạc nào trong danh sách BGM List!");
        }
    }

    IEnumerator PlayMusicWithDelay()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            int randomIndex = Random.Range(0, bgmList.Length);
            audioSource.clip = bgmList[randomIndex];

            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);

            yield return new WaitForSeconds(delayBetweenTracks);
        }
    }
}