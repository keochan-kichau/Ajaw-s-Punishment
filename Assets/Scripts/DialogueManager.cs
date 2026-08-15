using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [HideInInspector] public bool isEndingLevel = false;
    [HideInInspector] public string sceneToLoad = "";

    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image avatarImage;

    [Header("Data Files")]
    public TextAsset FirstDialogueFile;

    [System.Serializable]
    public struct EmotionData
    {
        public string emotionID;
        public Sprite emotionSprite;
    }

    [Header("Character Emotions Database")]
    public EmotionData[] emotionDatabase;
    private string[] lines;
    private int currentLineIndex = 0;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.03f;

    private bool isTyping = false;
    private string currentSentence = "";
    private Coroutine typingCoroutine;

    [System.Serializable]
    public struct DialogueLine
    {
        public string characterName;
        public Sprite characterAvatar;
        [TextArea(3, 10)]
        public string sentence;
    }

    [Header("Dialogue Script")]
    public DialogueLine[] dialogueLines;

    [Header("Scene Settings")]
    public bool isOpeningDialogue = true;

    [Header("Chuyển Map Sau Thoại")]
    [Tooltip("Tick vào đây nếu muốn đọc xong thoại là nhảy thẳng sang Scene khác")]
    public bool loadSceneAfterDialogue = true;
    public string nextSceneName = "Level3";

    void Start()
    {
        if (CemeteryHouseInteract.currentLevel == 0)
        {
            gameObject.SetActive(true);
            if (isOpeningDialogue && FirstDialogueFile != null)
            {
                lines = FirstDialogueFile.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
                DisplayNextLine();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        bool isMouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool isSpacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;

        if (isMouseClicked || isSpacePressed)
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    void DisplayNextLine()
    {
        if (currentLineIndex >= lines.Length)
        {
            EndDialogue();
            return;
        }

        string line = lines[currentLineIndex];
        string[] parts = line.Split('|');

        if (parts.Length == 3)
        {
            string charName = parts[0].Trim();
            string emotionID = parts[1].Trim();
            string sentence = parts[2].Trim();

            currentSentence = sentence;
            nameText.text = charName;

            Sprite foundSprite = null;
            foreach (EmotionData emo in emotionDatabase)
            {
                if (emo.emotionID == emotionID)
                {
                    foundSprite = emo.emotionSprite;
                    break;
                }
            }

            if (foundSprite != null)
            {
                avatarImage.sprite = foundSprite;
                avatarImage.gameObject.SetActive(true);
            }
            else
            {
                avatarImage.gameObject.SetActive(false);
            }

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
        }
        else
        {
            Debug.LogWarning("Dòng thoại bị lỗi cấu trúc (thiếu dấu |): " + line);
        }

        currentLineIndex++;
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        gameObject.SetActive(false);

        if (loadSceneAfterDialogue)
        {
            Debug.Log("Kết thúc thoại! Chuyển sang map tiếp theo: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        PlayerManager pm = FindAnyObjectByType<PlayerManager>();

        if (isEndingLevel)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else if (pm != null)
        {
            if (isOpeningDialogue)
            {
                pm.StartGameAfterDialogue();
                isOpeningDialogue = false;
            }
            else
            {
                pm.EnableGameplay();
            }
        }
    }

    public void StartDialogueWithFile(TextAsset newTextFile, bool endLevel = false, string nextScene = "")
    {
        isEndingLevel = endLevel;
        sceneToLoad = nextScene;

        gameObject.SetActive(true);
        lines = newTextFile.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        currentLineIndex = 0;
        DisplayNextLine();
    }
}