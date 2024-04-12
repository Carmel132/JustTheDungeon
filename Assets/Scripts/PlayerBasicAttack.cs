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
        HandleInput();
        //if (!GetComponent<BasicPlayerController>().isRolling && gunManager.Current() is IChargeableWeapon weapon)
        //{
        //    if (didPlayerClickMouse)
        //    {
        //        weapon.StartCharging();
        //    }
        //    else if (didPlayerReleaseMouse)
        //    {
        //        gunManager.Current().OnActivation(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    }
        //}
        //else if (isPlayerHoldingDownMouse && !GetComponent<BasicPlayerController>().isRolling)
        //{
        //    gunManager.Current().OnActivation(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    if (gunManager.Current().ammo.Current == 0) { isPlayerHoldingDownMouse = false; }
        //}
        if (!GetComponent<BasicPlayerController>().isRolling)
        {
            AttackInputManagers.IAttackInputManager attackInputManager = gunManager.Current().attackInputManager;
            if (isPlayerHoldingDownMouse)
            {
                attackInputManager.OnHold(gunManager);
            }
            if (didPlayerClickMouse)
            {
                attackInputManager.OnClick(gunManager);
            }
            if (didPlayerReleaseMouse)
            {
                attackInputManager.OnRelease(gunManager);
            }
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
