using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CemeteryHouseInteract : MonoBehaviour
{
    [Header("Cài đặt Lối Thoát Số 8")]
    [Tooltip("Không cần tick thủ công. AnomalyManager sẽ tự tick ô này!")]
    public bool hasAnomaly = false;

    [Header("Kịch Bản Chiến Thắng")]
    public TextAsset winDialogue; 
    [Tooltip("Gõ chính xác tên Scene Main Menu để game tự văng ra ngoài sảnh sau khi win")]
    public string mainMenuSceneName = "Main Menu"; 

    [Header("Hình ảnh Số Level")]
    [Tooltip("Kéo object chứa component SpriteRenderer hiển thị số vào đây")]
    public SpriteRenderer numberDisplay;
    [Tooltip("Kéo 9 tấm ảnh số (từ 0 đến 8) vào đây theo ĐÚNG THỨ TỰ")]
    public Sprite[] numberSprites;

    public static int currentLevel = 0;

    private List<PlayerController> playersNear = new List<PlayerController>();

    void Start()
    {
        UpdateLevelImage();
    }

    void UpdateLevelImage()
    {
        if (numberDisplay != null && numberSprites != null && numberSprites.Length > 0)
        {
            int spriteIndex = Mathf.Clamp(currentLevel, 0, numberSprites.Length - 1);
            numberDisplay.sprite = numberSprites[spriteIndex];
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        bool canInteract = false;
        foreach (PlayerController pc in playersNear)
        {
            if (pc != null && pc.isControlled)
            {
                canInteract = true; 
                break;
            }
        }

        if (!canInteract) return;

        if (currentLevel == 0)
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                Debug.Log("Bắt đầu vào Level 1!");
                currentLevel = 1;
                ReloadMap();
            }
            return;
        }

        bool pressedF = Keyboard.current.fKey.wasPressedThisFrame;
        bool pressedJ = Keyboard.current.jKey.wasPressedThisFrame;

        if (pressedF || pressedJ)
        {
            bool isCorrect = false;

            if (pressedF && !hasAnomaly) isCorrect = true;
            if (pressedJ && hasAnomaly) isCorrect = true;

            if (isCorrect)
            {
                currentLevel++;

                if (currentLevel > 8)
                {
                    WinGame();
                }
                else
                {
                    ReloadMap();
                }
            }
            else
            {
                currentLevel = 1;
                ReloadMap();
            }
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetLevelOnPlay()
    {
        currentLevel = 0;
    }

    void ReloadMap()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void WinGame()
    {
        PlayerManager pm = FindAnyObjectByType<PlayerManager>();
        if (pm != null) pm.DisableGameplay();

        DialogueManager dm = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
        if (dm != null && winDialogue != null)
        {
            dm.StartDialogueWithFile(winDialogue, true, mainMenuSceneName);
        }

        PlayerPrefs.SetInt("SavedLevel", 0);
        PlayerPrefs.Save();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null && !playersNear.Contains(pc))
            {
                playersNear.Add(pc); 
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null && playersNear.Contains(pc))
            {
                playersNear.Remove(pc); 
            }
        }
    }
}