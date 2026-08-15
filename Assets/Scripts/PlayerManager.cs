using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Characters")]
    public PlayerController ajaw;
    public PlayerController dodoco;

    [Header("Camera")]
    public CinemachineCamera cam;

    private bool isAjawActive = true; // Mặc định vào game Ajaw chạy trước
    public bool isGameplayDisabled = false;

    void Start()
    {
        if (CemeteryHouseInteract.currentLevel == 0)
        {
            if (ajaw != null) ajaw.gameObject.SetActive(false);
            if (dodoco != null) dodoco.gameObject.SetActive(false);
            DisableGameplay();
        }
        else
        {
            if (ajaw != null) ajaw.gameObject.SetActive(true);
            if (dodoco != null) dodoco.gameObject.SetActive(true);
            EnableGameplay();
        }

        if (cam != null && ajaw != null) cam.Target.TrackingTarget = ajaw.transform;
    }

    void Update()
    {
        if (isGameplayDisabled) return;

        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            isAjawActive = !isAjawActive;

            if (isAjawActive)
            {
                ajaw.isControlled = true;
                dodoco.isControlled = false;
                cam.Target.TrackingTarget = ajaw.transform;
            }
            else
            {
                ajaw.isControlled = false;
                dodoco.isControlled = true;
                cam.Target.TrackingTarget = dodoco.transform;
            }
        }
    }

    public void DisableGameplay()
    {
        isGameplayDisabled = true;

        if (ajaw != null) 
        {
            ajaw.isControlled = false;
            ajaw.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
        if (dodoco != null) 
        {
            dodoco.isControlled = false;
            dodoco.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }

    public void EnableGameplay()
    {
        isGameplayDisabled = false;

        if (isAjawActive)
        {
            if (ajaw != null) ajaw.isControlled = true;
        }
        else
        {
            if (dodoco != null) dodoco.isControlled = true;
        }
    }

    public void StartGameAfterDialogue()
    {
        if (ajaw != null) ajaw.gameObject.SetActive(true); 
        if (dodoco != null) dodoco.gameObject.SetActive(true); 
        
        EnableGameplay(); 
    }
}