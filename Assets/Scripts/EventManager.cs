using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#nullable enable
public enum EventGroup
{
    Player, PlayerStats, Weapon
}

public class EventManager : MonoBehaviour, IPlayerMessages, IWeaponMessages, IPlayerStatMessages
{

    private Dictionary<EventGroup, List<GameObject>> register = new Dictionary<EventGroup, List<GameObject>>();

    /// <summary>
    /// Registers obj to receive eventGroup events
    /// </summary>
    /// <param name="eventGroup">The event group the object is registering to</param>
    /// <param name="obj">The gameobject being registered</param>
    public void registerEvent(EventGroup eventGroup, GameObject obj)
    {
        if (!register.ContainsKey(eventGroup)) { register.Add(eventGroup, new List<GameObject>()); }
        register[eventGroup].Add(obj);
    }
    /// <summary>
    /// Deregisters a given object
    /// </summary>
    /// <param name="eventGroup">Event group to deregister obj from</param>
    /// <param name="obj">Object getting deregistered</param>
    public void deregisterEvent(EventGroup eventGroup, GameObject obj)
    {
        register[eventGroup].Remove(obj);
    }

    //IPlayerMessages
    public void OnPlayerMainAttack(Transform player)
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerMainAttack(transform));
        }
    }
    public void OnPlayerActiveAbility(Transform player)
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerActiveAbility(transform));
        }
    }
    public void OnPlayerUltimateAbility(Transform player)
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerUltimateAbility(transform));
        }
    }
    public void OnPlayerHit(Transform player)
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerHit(transform));
        }
    }
    public void OnPlayerMove(Transform player)
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerMove(transform));
        }
    }
    public void OnPlayerRoll(Transform player)
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerRoll(transform));
        }
    }
    public void OnPlayerInteract(Transform player)
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerInteract(transform));
        }
    }
    public void OnPlayerKill(Transform player, Transform enemy)
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerKill(player, enemy));
        }
    }
    public void PlayerChargeUlt(float damage)
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.PlayerChargeUlt(damage));
        }
    }

    //IWeaponMessages
    public void AddStatChange((GunEffectManagerTarget, EffectFactor, TimeCooldown?) f)
    {
        foreach (var obj in register[EventGroup.Weapon])
        {
            ExecuteEvents.Execute<IWeaponMessages>(obj, null, (x, y) => x.AddStatChange(f));
        }
    }

    public void Reload(object? payload)
    {
        foreach (var obj in register[EventGroup.Weapon])
        {
            ExecuteEvents.Execute<IWeaponMessages>(obj, null, (x, y) => x.Reload(payload));
        }
    }

    //IPlayerStatMessages
    public void AddStatChange((PlayerEffectTarget, EffectFactor, TimeCooldown?) t)
    {
        foreach (var obj in register[EventGroup.PlayerStats])
        {
            ExecuteEvents.Execute<IPlayerStatMessages>(obj, null, (x, y) => x.AddStatChange(t));
        }
    }
}
