using UnityEngine;



public class PlayerBasicAttack : MonoBehaviour
{
    public IAbility<Vector3> ability;
    GunManager gunManager;
    bool isPlayerHoldingDownMouse = false;

    void Start()
    {
        gunManager = GetComponentInChildren<GunManager>();
    }

    // TODO: Migrate input to controller and implement events
    void Update()
    {
        HandleInput();
        if (isPlayerHoldingDownMouse && !GetComponent<BasicPlayerController>().isRolling)
        {
            gunManager.Current().OnActivation(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (gunManager.Current().ammo.Current == 0) { isPlayerHoldingDownMouse = false; }
        }

    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) { isPlayerHoldingDownMouse = true; }
        if (Input.GetMouseButtonUp(0)) { isPlayerHoldingDownMouse = false; }

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
