using UnityEngine;
#nullable enable

using EffectDictionary = System.Collections.Generic.Dictionary<int, (PlayerEffectTarget, EffectFactor, TimeCooldown?)>;
using UnityEngine.EventSystems;

public enum PlayerEffectTarget
{
    MOVEMENTSPEED, MAXHP, HP, ROLLSPEED, ROLLCOOLDOWN
}
public class PlayerEffects : MonoBehaviour
{

    public PlayerStats stats;
    EffectDictionary effects;
    static System.Random rnd = new System.Random();

    public void Add(int id, (PlayerEffectTarget, EffectFactor, TimeCooldown?) t)
    {
        t.Item3?.Reset();
        if (effects.ContainsKey(id)) { effects[id] = t; }
        else { effects.Add(id, t); }
        switch (t.Item1)
        {
            case PlayerEffectTarget.MOVEMENTSPEED:
                stats.movementSpeed = t.Item2 * stats.movementSpeed;
                break;
            case PlayerEffectTarget.MAXHP:
                stats.maxHP = (int)(t.Item2 * stats.maxHP);
                break;
            case PlayerEffectTarget.HP:
                stats.HP = (int)(t.Item2 * stats.HP);
                break;
            case PlayerEffectTarget.ROLLSPEED:
                stats.rollSpeed = t.Item2 * stats.rollSpeed;
                break;
            case PlayerEffectTarget.ROLLCOOLDOWN:
                stats.rollCooldown = t.Item2 * stats.rollCooldown;
                break;
        }
    }

    public void Remove(int id)
    {
        var t = effects[id];
        switch (t.Item1)
        {
            case PlayerEffectTarget.MOVEMENTSPEED:
                stats.movementSpeed = t.Item2 / stats.movementSpeed;
                break;
            case PlayerEffectTarget.MAXHP:
                stats.maxHP = (int)(t.Item2 / stats.maxHP);
                break;
            case PlayerEffectTarget.HP:
                stats.HP = (int)(t.Item2 / stats.HP);
                break;
            case PlayerEffectTarget.ROLLSPEED:
                stats.rollSpeed = t.Item2 / stats.rollSpeed;
                break;
            case PlayerEffectTarget.ROLLCOOLDOWN:
                stats.rollCooldown = t.Item2 / stats.rollCooldown;
                break;
        }
        effects.Remove(id);
    }
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        effects = new EffectDictionary();
    }
    void Update()
    {
        var temp = new EffectDictionary();
        foreach (var effect in temp)
        {
            if (effect.Value.Item3 == null) { continue; }
            else if (effect.Value.Item3.isAvailable)
            {
                Remove(effect.Key);
            }
        }
    }
    public int newId()
    {
        int ret;
        do { ret = rnd.Next(); } while (effects.ContainsKey(ret));
        return ret;
    }
}
public interface IPlayerController : IPlayerStatMessages, IPlayerRollable
{
    public PlayerEffects stats { get; set; }
    void IPlayerStatMessages.AddStatChange((PlayerEffectTarget, EffectFactor, TimeCooldown?) f)
    {
        stats.Add(stats.newId(), f);
    }
    public void HandleMovement();
    public void HandleInput();
}