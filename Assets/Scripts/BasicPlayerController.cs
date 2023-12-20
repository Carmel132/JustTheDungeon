using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IAbility<T>
{
    void Update(T args) { }
    void Start(T args) { }
};

public interface IPassiveAbility<T> : IAbility<T>
{

}

public interface IActiveAbility<T> : IAbility<T>
{
    float chargeRate { get; set; }
    float maxCharge { get; }
    float currentCharge { get; set; }
}

public interface IUltimateAbility<T> : IActiveAbility<T>
{
    
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
}

// TODO: Create ability information interfaces
public struct PlayerStats
{
    public uint Speed;
    public static float SpeedMultiplier = 3/2;
    public uint HP;
    public IPassiveAbility<Transform> passive;
    public IActiveAbility<Transform> active;
    public IUltimateAbility<Transform> ultimate;
}

public class BasicPlayerController : MonoBehaviour
{
    public PlayerStats ps;
    public EventManager EM;
    void Start()
    {
        ps.Speed = 3;
    }

    void Update()
    {
        HandleMovement();
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
}
