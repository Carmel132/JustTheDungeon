using UnityEngine;

/// <summary>
/// Handles user-to-player-to-gun manager linking
/// </summary>
public class PlayerBasicAttack : MonoBehaviour
{
    GunManager gunManager;
    bool isPlayerHoldingDownMouse = false;
    bool didPlayerReleaseMouse = false;
    bool didPlayerClickMouse = false;
    void Start()
    {
        gunManager = GetComponentInChildren<GunManager>();
    }
    // TODO: implement shoot-roll cancelling
    void Update()
    {
        if (GetComponent<BasicPlayerController>().isRolling) { return; }
        HandleInput();
        AttackInputManagers.IAttackInputManager attackInputManager = gunManager.Current().attackInputManager;
        if (isPlayerHoldingDownMouse)
        {
            attackInputManager.OnHold(gunManager);
        }
        if (didPlayerClickMouse)
        {
            attackInputManager.OnClick(gunManager);        }
        if (didPlayerReleaseMouse)
        {
            attackInputManager.OnRelease(gunManager);
        }
    }

    void HandleInput()
    {
        didPlayerClickMouse = false;
        didPlayerReleaseMouse = false;
        if (Input.GetMouseButtonDown(0)) 
        {
            didPlayerClickMouse = true;
            isPlayerHoldingDownMouse = true; 
        }
        if (Input.GetMouseButtonUp(0)) 
        { 
            didPlayerReleaseMouse = true;
            isPlayerHoldingDownMouse = false; 
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            gunManager.NextWeapon();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            gunManager.PrevWeapon();
        }
    }
}
