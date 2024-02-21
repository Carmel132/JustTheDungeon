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

// TODO: Create ability information interfaces
public interface IPlayerAbilities<Payload> where Payload : IPlayerAbilityPayload
{
    public IPassiveAbility<Payload> passive { get; set; }
    public IActiveAbility<Payload> active { get; set; }
    public IUltimateAbility<Payload> ultimate { get; set; }
}
// fuck abstraction (kill me)
public interface IPlayerAbilityPayload
{
    public Transform player { get; set; }
}


public interface IPlayerRollable
{
    public bool isRolling { get;set; }
    public TimeCooldown rollDuration { get; set; }
}

public class BasicPlayerController : MonoBehaviour, IPlayerController
{
    public EventManager EM;
    public PlayerEffects stats { get; set; }
    [field: SerializeField]
    public bool isRolling { get; set; } = false;
    [field: SerializeField]
    public TimeCooldown rollDuration { get; set; }

    Vector2 rollDirection = Vector2.zero;

    private void Start()
    {
        stats = GetComponent<PlayerEffects>();
        EM.registerEvent(EventGroup.PlayerStats, gameObject);
    }

    private void Update()
    {
        HandleMovement();
        HandleInput();
    }

    public void HandleMovement()
    {
        if (!isRolling)
        {
            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            transform.position += (Vector3)movement * stats.stats.movementSpeed * PlayerStats.SpeedMultiplier * Time.deltaTime;
            if (movement.magnitude != 0)
            {
                ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.OnPlayerMove(transform));
            }
        }
        else
        {
            transform.position += (Vector3)rollDirection * stats.stats.rollSpeed * PlayerStats.SpeedMultiplier * Time.deltaTime;
            ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.OnPlayerRoll(transform));
            if (rollDuration.isAvailable) 
            { 
                isRolling = false;
                stats.stats.rollCooldown.Reset();
                GetComponent<Collider2D>().enabled = true;
                //just to see invincibility frames
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(1) && !isRolling)
        {
            ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.OnPlayerActiveAbility(transform));
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling)
        {
            ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.OnPlayerUltimateAbility(transform));
        }
        if (Input.GetKeyDown(KeyCode.Space) && stats.stats.rollCooldown.isAvailable && !isRolling)
        {
            rollDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            if (rollDirection.magnitude != 0)
            {
                isRolling = true;
                rollDuration.Reset();
                GetComponent<Collider2D>().enabled = false;
                //just to see invincibility frames
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            
        }
    }
}
