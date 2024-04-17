using System.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using DigitalRuby.Tween;
/// <summary>
/// Defines a basic ability
/// </summary>
/// <typeparam name="T">payload structure</typeparam>
public interface IAbility<T>
{
    void Update(T args) { }
    void Start(T args) { }
    // When the ability is called
    public void OnActivation(T payload) { }
};

/// <summary>
/// Defines a passive ability (actives automatically)
/// </summary>
/// <typeparam name="T">payload structure</typeparam>
public interface IPassiveAbility<T> : IAbility<T>
{

}

/// <summary>
/// Defines an active ability (activated when player hits the active ability key)
/// </summary>
/// <typeparam name="T">payload structure</typeparam>
public interface IActiveAbility<T> : IAbility<T>
{
    ICooldown cd { get; set; }
}

/// <summary>
/// Defines an ultimate ability (activates when player hits the ultimate ability key) -- charging differs from active
/// </summary>
/// <typeparam name="T">payload structure</typeparam>
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

/// <summary>
/// Encapsulates a character's ability structure
/// </summary>
/// <typeparam name="Payload">Payload structure</typeparam>
public interface IPlayerAbilities<Payload> where Payload : IPlayerAbilityPayload
{
    public IPassiveAbility<Payload> passive { get; set; }
    public IActiveAbility<Payload> active { get; set; }
    public IUltimateAbility<Payload> ultimate { get; set; }
}

/// <summary>
/// Basic ability payload structure
/// </summary>
public interface IPlayerAbilityPayload
{
    public Transform player { get; set; }
}

/// <summary>
/// Defines rollability
/// </summary>
public interface IPlayerRollable
{
    public bool isRolling { get;set; }
    public TimeCooldown rollDuration { get; set; }
}

/// <summary>
/// Controls player movement and input excluding attacks
/// </summary>
public class PlayerController : MonoBehaviour, IPlayerController
{
    public EventManager EM;
    public PlayerEffects stats { get; set; }
    [field: SerializeField]
    public bool isRolling { get; set; } = false;
    [field: SerializeField]
    public TimeCooldown rollDuration { get; set; }
    public bool canMove = true;
    Vector2 rollDirection = Vector2.zero;

    private void Start()
    {
        Time.timeScale = 1f;

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
        if (!isRolling && canMove)
        {
            Vector2 movement = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            transform.position += PlayerStats.SpeedMultiplier * stats.stats.movementSpeed * Time.deltaTime * (Vector3)movement;
            if (movement.magnitude != 0)
            {
                ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.OnPlayerMove(transform));
            }
        }
        else if (isRolling)
        {
            transform.position += PlayerStats.SpeedMultiplier * stats.stats.rollSpeed * Time.deltaTime * (Vector3)rollDirection;
            //ExecuteEvents.Execute<IPlayerMessages>(EM.gameObject, null, (x, y) => x.OnPlayerRoll(transform));
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
            transform.GetChild(0).GetComponent<GunManager>().InterruptReload();

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
                Util.CallEvent<IPlayerMessages>((x, _) => x.OnPlayerRoll(transform));
                isRolling = true;
                StartRollAnimation();
                rollDuration.Reset();
                GetComponent<Collider2D>().enabled = false;
                //just to see invincibility frames
                GetComponent<SpriteRenderer>().color = Color.red;
                transform.GetChild(0).GetComponent<GunManager>().InterruptReload();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            IWeapon weapon = transform.GetChild(0).GetComponent<GunManager>().Current();
            if (weapon.stats.stats.reloadSpeed.isAvailable && weapon.stats.stats.reloadable && weapon.ammo.isReloadable)
            {
                ExecuteEvents.Execute<IWeaponMessages>(EM.gameObject, null, (x, y) => x.Reload(null));
            }
        }
    }

    public void StartRollAnimation()
    {
        Debug.Log(1);
        void updatePlayerRotation(ITween<float> t)
        {
            if (isRolling) transform.rotation = Quaternion.Euler(0, 0, t.CurrentValue);
        }

        float currentRotation = transform.rotation.z;
        float midPos = 180 * -Input.GetAxisRaw("Horizontal");
        float endPos = 360 * -Input.GetAxisRaw("Horizontal");

        gameObject.Tween("RollAnimation", currentRotation, midPos, rollDuration.duration * 0.3f, TweenScaleFunctions.QuadraticEaseIn, updatePlayerRotation)
            .ContinueWith(new FloatTween().Setup(midPos, endPos, rollDuration.duration * 0.7f, TweenScaleFunctions.QuadraticEaseOut, updatePlayerRotation));
    }
}
