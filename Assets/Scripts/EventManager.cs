using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#nullable enable
public enum EventGroup
{
    Player, PlayerStats, GunStats
}

public class EventManager : MonoBehaviour, IPlayerMessages, IGunStatMessages, IPlayerStatMessages
{

    private Dictionary<EventGroup, List<GameObject>> register = new Dictionary<EventGroup, List<GameObject>>();

    public void registerEvent(EventGroup eventGroup, GameObject obj)
    {
        if (!register.ContainsKey(eventGroup)) { register.Add(eventGroup, new List<GameObject>()); }
        register[eventGroup].Add(obj);
    }
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

    //IGunStatMessages
    public void AddStatChange((GunEffectManagerTarget, EffectFactor, TimeCooldown?) f)
    {
        foreach (var obj in register[EventGroup.GunStats])
        {
            ExecuteEvents.Execute<IGunStatMessages>(obj, null, (x, y) => x.AddStatChange(f));
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
