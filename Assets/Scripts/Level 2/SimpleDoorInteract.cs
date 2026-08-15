using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleDoorInteract : MonoBehaviour
{
    [Header("Cài Đặt Dịch Chuyển")]
    [Tooltip("Kéo cánh cửa đích (nơi muốn bay tới) vào đây")]
    public Transform destination;

    [Header("Âm Thanh (SFX)")]
    public AudioSource audioSource;
    public AudioClip openDoorSFX;

    private bool isNear = false;
    private Transform playerToTeleport;

    void Update()
    {
        if (isNear && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (destination != null && playerToTeleport != null)
            {
                if (audioSource != null && openDoorSFX != null)
                {
                    audioSource.PlayOneShot(openDoorSFX);
                }

                playerToTeleport.position = destination.position;
                Debug.Log(playerToTeleport.name + " đã chui qua cửa!");
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