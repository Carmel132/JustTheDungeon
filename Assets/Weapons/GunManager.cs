using System.Collections.Generic;
using UnityEngine;
#nullable enable

/// <summary>
/// General class relating allowing for simple temporary effect management
/// </summary>
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
        TimeCooldown ret = new();
        ret.duration = rhs.duration * lhs.m + lhs.b;
        ret.Reset();
        return ret;
    }
    public static TimeCooldown operator /(EffectFactor lhs, TimeCooldown rhs)
    {
        TimeCooldown ret = new();
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

/// <summary>
/// Defines simple weapon and implements IWeaponMessages.AddStatChange and IWeaponMessages.Reload
/// </summary>
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

/// <summary>
/// Class that manages a player's weapons and allows for weapon selection
/// </summary>
public class GunManager : MonoBehaviour, IWeaponMessages
{
    /// <summary>
    /// List of weapons player has
    /// </summary>
    public List<IWeapon> weapons;
    /// <summary>
    /// Index of currently selecting weapon
    /// </summary>
    int current = 0;
    /// <summary>
    /// Whether the GunManager is allowed to switch weapons
    /// </summary>
    public bool locked = false;

    /// <summary>
    /// Animator of the reloading indicator
    /// </summary>
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
    /// <summary>
    /// Disables all non-selected weapon game objects
    /// </summary>
    void DisableNonCurrent()
    {
        for (int i = 0; i < weapons.Count; ++i)
        {
            weapons[i].stats.gameObject.SetActive(i == current);
        }
    }
    /// <summary>
    /// Selects the last weapon
    /// </summary>
    public void LastWeapon()
    {
        if (locked) return;
        current = weapons.Count - 1;
    }
    /// <summary>
    /// Selects the first weapon
    /// </summary>
    public void FirstWeapon()
    {
        if (locked) return;
        current = 0;
    }
    /// <summary>
    /// Selects the next weapon
    /// </summary>
    public void NextWeapon()
    {
        if (locked) return;
        current = (current + 1) % weapons.Count;
    }
    /// <summary>
    /// Selects the previous weapon
    /// </summary>
    public void PrevWeapon()
    {
        if (locked) return;
        current = (current == 0 ? weapons.Count : current) - 1;
    }
    /// <summary>
    /// Get the currently selected weapon
    /// </summary>
    /// <returns>IWeapon? of selected weapon</returns>
    public IWeapon? Current()
    {
        return weapons[current];
    }

    /// <summary>
    /// Waits for reload message and initiates the reload indicator
    /// </summary>
    /// <param name="payload"></param>
    public void Reload(object? payload)
    {
        if (ReloadIndicator.GetBool("isReloading") || !Current().ammo.isReloadable) { return; }


        Debug.Log(1);
        ReloadIndicator.SetBool("isReloading", true);
        float reloadDuration = Current().stats.stats.reloadSpeed.duration;
        ReloadIndicator.SetFloat("SpeedMultiplier", 1.07f/reloadDuration);

        cd.duration = reloadDuration;
        cd.Reset();
    }

    /// <summary>
    /// Disables the reload indicator animation
    /// </summary>
    void StopReloadIndicator()
    {
        if (ReloadIndicator.GetBool("isReloading"))
        {
            ReloadIndicator.SetBool("isReloading", false);
            Current().ammo.Reload();
        }
    }

    /// <summary>
    /// Interrupts the reload indicator animation
    /// </summary>
    public void InterruptReload()
    {
        ReloadIndicator.SetBool("isReloading", false);
    }

    /// <summary>
    /// Iterates through child objects for weapons and adds them to weapons list
    /// </summary>
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
