using UnityEngine;

/// <summary>
/// Defines simple weapon and implements IWeaponMessages.AddStatChange and IWeaponMessages.Reload
/// </summary>
public interface IWeapon : IAbility<Vector3>, IWeaponMessages
{
    //IAbility<Vector3>.OnActivation(Vector3 payload) == OnFire()
    GunEffectManager stats { get; set; }
    BasicAmmoManager ammo { get; set; }
    void IWeaponMessages.AddStatChange((GunEffectManager.GunEffectManagerTarget, EffectFactor, TimeCooldown?) f)
    {
        stats.effectManager.Add(stats.effectManager.newId(), f);
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
/// Defines a weapons that requires charging
/// </summary>
public interface IChargeableWeapon
{
    TimeCooldown charging { get => charging; set { charging = value; charging.Finish(); } }
    bool doneCharging { get => !charging.isAvailable; }
    void StartCharging() { charging.Reset(); }
}
