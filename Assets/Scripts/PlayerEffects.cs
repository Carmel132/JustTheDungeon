using System;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerEffects;
using static UnityEngine.GraphicsBuffer;
#nullable enable

public class PlayerEffects : MonoBehaviour, IEffectManager<PlayerEffectTarget>
{
    public enum PlayerEffectTarget : int
    {
        MOVEMENTSPEED, MAXHP, HP, ROLLSPEED, ROLLCOOLDOWN
    }

    public PlayerStats stats;
    public EffectManager<PlayerEffectTarget> effectManager { get; set; }

    public void ImplementEffect((PlayerEffectTarget, EffectFactor, TimeCooldown?) t)
    {
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
    
    public void NullifyEffect((PlayerEffectTarget, EffectFactor, TimeCooldown?) t)
    {
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
    }
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        effectManager = new(ImplementEffect, NullifyEffect);
        effectManager.EffectManagerStart();
    }
    private void Update()
    {
        effectManager.EffectManagerUpdate();
    }
}
public interface IPlayerController : IPlayerStatMessages, IPlayerRollable
{
    public PlayerEffects stats { get; set; }
    void IPlayerStatMessages.AddStatChange((PlayerEffectTarget, EffectFactor, TimeCooldown?) f)
    {
        stats.effectManager.Add(stats.effectManager.newId(), f);
    }
    public void HandleMovement();
    public void HandleInput();
}