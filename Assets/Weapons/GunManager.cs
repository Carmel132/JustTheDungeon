using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor.Animations;
using System.Net.Http.Headers;
#nullable enable
public class EffectFactor
{
    float m;
    float b;
    public static float operator *(EffectFactor lhs, float rhs)
    {
        return lhs.m * rhs + lhs.b;
    }
    public static float operator /(EffectFactor lhs, float rhs)
    {
        return (rhs - lhs.b) / lhs.m;
    }
    public static TimeCooldown operator *(EffectFactor lhs, TimeCooldown rhs)
    {
        var ret = new TimeCooldown();
        ret.duration = rhs.duration * lhs.m + lhs.b;
        ret.Reset();
        return ret;
    }
    public static TimeCooldown operator /(EffectFactor lhs, TimeCooldown rhs)
    {
        var ret = new TimeCooldown();
        ret.duration = (rhs.duration - lhs.b) / lhs.m;
        ret.Reset();
        return ret;
    }

    public EffectFactor(float m, float b)
    {
        this.m = m;
        this.b = b;
    }
}

public interface IWeaponMessages : IEventSystemHandler
{
    void AddStatChange((GunEffectManagerTarget, EffectFactor, TimeCooldown?) f) { }
    void Reload(object? payload) { }
}

public interface IWeapon : IAbility<Vector3>, IWeaponMessages
{
    //IAbility<Vector3>.OnActivation(Vector3 payload) == OnFire()
    GunEffectManager stats { get; set; }
    BasicAmmoManager ammo { get; set; }
    void IWeaponMessages.AddStatChange((GunEffectManagerTarget, EffectFactor, TimeCooldown?) f)
    {
        stats.Add(stats.newId(), f.Item1, f.Item2, f.Item3);
    }

    void IWeaponMessages.Reload(object? payload)
    {
        if (stats.stats.reloadSpeed.isAvailable && stats.stats.reloadable && ammo.isReloadable)
        {
            stats.stats.reloadSpeed.Reset();
            //ammo.Reload();
        }
    }
}

public class GunManager : MonoBehaviour, IWeaponMessages
{
    public List<IWeapon> weapons;
    int current = 0;
    public bool locked = false;

    public Animator ReloadIndicator;
    public TimeCooldown cd = new(0);
    void Start()
    {
        UpdateWeaponList();

        Util.GetEventManager().registerEvent(EventGroup.Weapon, gameObject);
    }
    void Update()
    {
        if (transform.childCount != weapons.Count)
        {
            UpdateWeaponList();
        }
        DisableNonCurrent();

        if (ReloadIndicator.GetBool("isReloading") && cd.isAvailable)
        {
            StopReloadIndicator();
        }
    }
    void OnGUI()
    {
        ReloadIndicator.transform.position = transform.position + new Vector3(0, 2.5f, 0);
    }
    void DisableNonCurrent()
    {
        for (int i = 0; i < weapons.Count; ++i)
        {
            weapons[i].stats.gameObject.SetActive(i == current);
        }
    }
    public void LastWeapon()
    {
        if (locked) return;
        current = weapons.Count - 1;
    }
    public void FirstWeapon()
    {
        if (locked) return;
        current = 0;
    }
    public void NextWeapon()
    {
        if (locked) return;
        current = (current + 1) % weapons.Count;
    }
    public void PrevWeapon()
    {
        if (locked) return;
        current = (current == 0 ? weapons.Count : current) - 1;
    }
    public IWeapon? Current()
    {
        return weapons[current];
    }

    public void Reload(object? payload)
    {
        if (ReloadIndicator.GetBool("isReloading") || !Current().ammo.isReloadable) { return; }


        Debug.Log(1);
        ReloadIndicator.SetBool("isReloading", true);
        float reloadDuration = Current().stats.stats.reloadSpeed.duration;
        ReloadIndicator.SetFloat("SpeedMultiplier", 1.07f/(reloadDuration));

        cd.duration = reloadDuration;
        cd.Reset();
    }

    void StopReloadIndicator()
    {
        if (ReloadIndicator.GetBool("isReloading"))
        {
            ReloadIndicator.SetBool("isReloading", false);
            Current().ammo.Reload();
        }
    }

    public void InterruptReload()
    {
        ReloadIndicator.SetBool("isReloading", false);
    }

    void UpdateWeaponList()
    {
        weapons = new();
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (Util.GetComponentThatImplements<IWeapon>(transform.GetChild(i).gameObject) is IWeapon w)
            {
                weapons.Add(w);
            }
        }
    }
    //TODO: Interrupt reload on ability activation
}
