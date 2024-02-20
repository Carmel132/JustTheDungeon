using System.Collections;
using System.Collections.Generic;
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

    bool isPlayerHoldingDownMouse = false;

    // Start is called before the first frame update
    void Start()
    {
    }
    // TODO: Migrate input to controller and implement events
    // Update is called once per frame
    void Update()
    {
        HandleInput();

        if (isPlayerHoldingDownMouse)
        {
            ability.OnActivation(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) { isPlayerHoldingDownMouse = true; }
        if (Input.GetMouseButtonUp(0)) { isPlayerHoldingDownMouse = false; }
    }
}
