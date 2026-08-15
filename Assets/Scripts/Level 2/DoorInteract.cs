using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteract : MonoBehaviour
{
    [Header("Cài Đặt Dịch Chuyển")]
    public Transform destination;
    public bool isUnlocked = false;

    [Header("Cửa Liên Kết (Tùy chọn)")]
    [Tooltip("Kéo cánh cửa đầu ra vào đây. Khi cửa này mở, cửa kia cũng tự động mở theo.")]
    public DoorInteract pairedDoor;

    [Header("Kịch Bản (Tùy chọn)")]
    public TextAsset lockedDialogue;

    [Header("Âm Thanh (SFX)")]
    public AudioSource audioSource;
    public AudioClip openDoorSFX;  
    public AudioClip lockedDoorSFX;  

    private bool isNear = false;
    private Transform playerToTeleport;

    public void UnlockDoor()
    {
        isUnlocked = true;

        if (pairedDoor != null && !pairedDoor.isUnlocked)
        {
            pairedDoor.UnlockDoor();
        }
    }

    void Update()
    {
        if (isNear && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (isUnlocked)
            {
                if (playerToTeleport != null)
                {
                    if (audioSource != null && openDoorSFX != null)
                    {
                        audioSource.PlayOneShot(openDoorSFX);
                    }

                    playerToTeleport.position = destination.position;
                }
            }
            else
            {
                if (audioSource != null && lockedDoorSFX != null)
                {
                    audioSource.PlayOneShot(lockedDoorSFX);
                }

                if (lockedDialogue != null)
                {
                    FindAnyObjectByType<PlayerManager>().DisableGameplay();
                    FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include).StartDialogueWithFile(lockedDialogue);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
            playerToTeleport = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform == playerToTeleport)
        {
            isNear = false;
            playerToTeleport = null;
        }
    }
}