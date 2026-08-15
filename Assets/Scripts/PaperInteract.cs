using UnityEngine;
using UnityEngine.InputSystem;

public class PaperInteract : MonoBehaviour
{
    [Header("File Nội Dung Tờ Giấy")]
    public TextAsset paperTextFile; // Kéo file txt tờ giấy vào

    private bool isNear = false;

    void Update()
    {
        if (isNear && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            FindAnyObjectByType<PlayerManager>().DisableGameplay();

            FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include).StartDialogueWithFile(paperTextFile);
        }
    }

    void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) isNear = true; }
    void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) isNear = false; }
}