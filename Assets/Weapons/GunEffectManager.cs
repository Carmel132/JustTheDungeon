using System.Collections.Generic;
using UnityEngine;
using static GunEffectManager;
#nullable enable


public class GunEffectManager : MonoBehaviour, IEffectManager<GunEffectManagerTarget>
{
    public enum GunEffectManagerTarget : int
    {
        RELOADSPEED, FIRERATE, DAMAGE, SPEED, BLOOM
    }

    public WeaponStats stats;
    public EffectManager<GunEffectManagerTarget> effectManager { get; set; }

    void Start()
    {
        stats = GetComponent<WeaponStats>();
        effectManager = new(ImplementEffect, NullifyEffect);
        effectManager.EffectManagerStart();
    }
    void Update()
    {
        effectManager.EffectManagerUpdate();
    }

    public void ImplementEffect((GunEffectManagerTarget, EffectFactor, TimeCooldown?) f)
    {
        switch (f.Item1)
        {
            case GunEffectManagerTarget.RELOADSPEED:
                stats.reloadSpeed = f.Item2 * stats.reloadSpeed;
                break;
            case GunEffectManagerTarget.FIRERATE:
                stats.fireRate = f.Item2 * stats.fireRate;
                break;
            case GunEffectManagerTarget.DAMAGE:
                stats.damage = f.Item2 * stats.damage;
                break;
            case GunEffectManagerTarget.SPEED:
                stats.speed = f.Item2 * stats.speed;
                break;
            case GunEffectManagerTarget.BLOOM:
                stats.bloom = f.Item2 * stats.bloom;
                break;
        }
    }

    public void NullifyEffect((GunEffectManagerTarget, EffectFactor, TimeCooldown?) f)
    {
        switch (f.Item1)
        {
            case GunEffectManagerTarget.RELOADSPEED:
                stats.reloadSpeed = f.Item2 / stats.reloadSpeed;
                break;
            case GunEffectManagerTarget.FIRERATE:
                stats.fireRate = f.Item2 / stats.fireRate;
                break;
            case GunEffectManagerTarget.DAMAGE:
                stats.damage = f.Item2 / stats.damage;
                break;
            case GunEffectManagerTarget.SPEED:
                stats.speed = f.Item2 / stats.speed;
                break;
            case GunEffectManagerTarget.BLOOM:
                stats.bloom = f.Item2 / stats.bloom;
                break;
        }
    }
}
