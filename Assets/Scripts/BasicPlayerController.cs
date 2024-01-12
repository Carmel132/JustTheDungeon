using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IAbility<T>
{
    void Update(T args) { }
    void Start(T args) { }
    // When the ability is called
    public void OnActivation(T payload) { }
};

public interface IPassiveAbility<T> : IAbility<T>
{

}

public interface IActiveAbility<T> : IAbility<T>
{
    ICooldown cd { get; set; }
}

public interface IUltimateAbility<T> : IAbility<T>
{
    float chargeRate { get; set; }
    float maxCharge { get; }
    float currentCharge { get; set; }

    void Charge(float damage)
    {
        currentCharge = Mathf.Max(currentCharge + chargeRate * damage, maxCharge);
    }
}

public interface IPlayerMessages : IEventSystemHandler
{
    void OnPlayerMainAttack(Transform player) { }
    void OnPlayerActiveAbility(Transform player) { }
    void OnPlayerUltimateAbility(Transform player) { }
    void OnPlayerHit(Transform player) { }
    void OnPlayerMove(Transform player) { }
    void OnPlayerRoll(Transform player) { }
    void OnPlayerInteract(Transform player) { }
    void PlayerChargeUlt(float damage) { }
}

public class PlayerStats
{
    public int Speed { get; set; }
    public static float SpeedMultiplier = 3 / 2;
    public int HP { get; set; }

    public PlayerStats(int speed, int hP)
    {
        Speed = speed;
        HP = hP;
    }
}


// TODO: Create ability information interfaces
public interface IPlayerAbilities<Payload> where Payload : IPlayerAbilityPayload
{
    public PlayerStats stats { get; set; }
    public IPassiveAbility<Payload> passive { get; set; }
    public IActiveAbility<Payload> active { get; set; }
    public IUltimateAbility<Payload> ultimate { get; set; }
}
// fuck abstraction (kill me)
public interface IPlayerAbilityPayload
{
    public Transform player { get; set; }
}

public class BasicPlayerController : MonoBehaviour
{
    public PlayerStats ps;
    public EventManager EM;

    private void Start()
    {
    }

    private void Update()
    {
        HandleMovement();
        HandleInput();
    }

    void HandleMovement()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.position += (Vector3)movement * ps.Speed * PlayerStats.SpeedMultiplier * Time.deltaTime;
        if (movement.magnitude != 0)
        {
            ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.OnPlayerMove(transform));
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.OnPlayerActiveAbility(transform));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.OnPlayerUltimateAbility(transform));
        }
    }
}
