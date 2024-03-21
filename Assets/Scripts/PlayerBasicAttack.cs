using UnityEngine;



public class PlayerBasicAttack : MonoBehaviour
{
    /* GameObject proj;
    public float speed;
    public float lifetime;
    public float bloom;
    public Transform projectileParent;
    public Transform start;
    IBasicAbility<Vector3, IGunBasicAbilityInfo> ability = new GunBasicAbility();*/
    public IAbility<Vector3> ability;
    GunManager gunManager;
    bool isPlayerHoldingDownMouse = false;
    bool reloadClick = false;

    // Start is called before the first frame update
    void Start()
    {
        gunManager = GetComponentInChildren<GunManager>();
    }
    // TODO: Migrate input to controller and implement events
    // Update is called once per frame
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
