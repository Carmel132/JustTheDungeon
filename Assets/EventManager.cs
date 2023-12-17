using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.EventSystems;

public enum EventGroup
{
    Player
}

public class EventManager : MonoBehaviour, IPlayerMessages
{

    private Dictionary<EventGroup, List<GameObject>> register = new Dictionary<EventGroup, List<GameObject>>();

    public void OnPlayerMainAttack(Transform player) 
    {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerMainAttack(transform));
        }
    }
    public void OnPlayerActiveAbility(Transform player) {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerActiveAbility(transform));
        }
    }
    public void OnPlayerUltimateAbility(Transform player) {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerUltimateAbility(transform));
        }
    }
    public void OnPlayerHit(Transform player) {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerHit(transform));
        }
    }
    public void OnPlayerMove(Transform player) {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerMove(transform));
        }
    }
    public void OnPlayerRoll(Transform player) {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerRoll(transform));
        }
    }
    public void OnPlayerInteract(Transform player) {
        foreach (var obj in register[EventGroup.Player])
        {
            ExecuteEvents.Execute<IPlayerMessages>(obj, null, (x, y) => x.OnPlayerInteract(transform));
        }
    }
    public void registerEvent(EventGroup eventGroup, GameObject obj)
    {
        if (!register.ContainsKey(eventGroup)){ register.Add(eventGroup, new List<GameObject>()); }
        register[eventGroup].Add(obj);
    }
    public void deregisterEvent(EventGroup eventGroup, GameObject obj)
    {
        register[eventGroup].Remove(obj);
    }
}
