using UnityEngine;
using UnityEngine.InputSystem;

public class KeyInteract : MonoBehaviour
{
    [Header("Liên Kết Cửa (Có thể mở nhiều cửa 1 lúc)")]
    public DoorInteract[] targetDoors;

    [Header("Kịch Bản (Tùy chọn)")]
    public TextAsset pickupDialogue;

    private bool isNear = false;

    void Update()
    {
        if (isNear && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            foreach (DoorInteract door in targetDoors)
            {
                if (door != null)
                {
                    door.UnlockDoor();
                }
            }

            if (pickupDialogue != null)
            {
                FindAnyObjectByType<PlayerManager>().DisableGameplay();
                FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include).StartDialogueWithFile(pickupDialogue);
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) isNear = true; }
    void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) isNear = false; }
}