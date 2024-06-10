using UnityEngine;

/// <summary>
/// Handles user-to-player-to-gun manager linking
/// </summary>
public class PlayerAttackManager : MonoBehaviour
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
        if (GetComponent<PlayerController>().isRolling) { return; }
        HandleInput();
        if (!gunManager.Current().stats.stats.reloadSpeed.isAvailable) { return; }
        AttackInputManagers.IAttackInputManager attackInputManager = gunManager.Current().attackInputManager;
        WeaponAnimation? weaponAnimation = gunManager.Current().weaponAnimation;
        if (isPlayerHoldingDownMouse)
        {
            attackInputManager.OnHold(gunManager);
            weaponAnimation?.OnHold();
        }
        if (didPlayerClickMouse)
        {
            attackInputManager.OnClick(gunManager);
            weaponAnimation?.OnClick();
        }
        if (didPlayerReleaseMouse)
        {
            attackInputManager.OnRelease(gunManager);
            weaponAnimation?.OnRelease();
            Debug.Log("Hi");
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
