using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Defines player messages
/// </summary>
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
    void OnPlayerKill(Transform player, Transform enemy) { }
}

/// <summary>
/// Defines weapon messages
/// </summary>
public interface IWeaponMessages : IEventSystemHandler
{
    void AddStatChange((GunEffectManagerTarget, EffectFactor, TimeCooldown?) f) { }
    void Reload(object? payload) { }
}

/// <summary>
/// Defines stat messages for player
/// </summary>
public interface IPlayerStatMessages : IEventSystemHandler
{
    void AddStatChange((PlayerEffectTarget, EffectFactor, TimeCooldown?) f) { }
}