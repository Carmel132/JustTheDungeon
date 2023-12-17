using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IAbility
{
    void Update<T>(T[] args);
    void Start<T>(T[] args);
};

public interface IPassiveAbility : IAbility
{

}

public interface IActiveAbility : IAbility
{
    double chargeRate { get; set; }
    double maxCharge { get; }
    double currentCharge { get; set; }
}

public interface IUltimateAbility : IActiveAbility
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

public struct PlayerStats
{
    public uint Speed;
    public static float SpeedMultiplier = 3/2;
    public uint HP;
    public IPassiveAbility passive;
    public IActiveAbility active;
    public IUltimateAbility ultimate;
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
