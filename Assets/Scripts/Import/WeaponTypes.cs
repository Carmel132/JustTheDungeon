using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Defines simple weapon and implements IWeaponMessages.AddStatChange and IWeaponMessages.Reload
/// </summary>
public interface IWeapon : IAbility<Vector3>, IWeaponMessages
{
    //IAbility<Vector3>.OnActivation(Vector3 payload) == OnFire()
    GunEffectManager stats { get; set; }
    BasicAmmoManager ammo { get; set; }
    AttackInputManagers.IAttackInputManager attackInputManager { get ; set; }
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

public static class AttackInputManagers
{
    public interface IAttackInputManager
    {
        void OnClick(GunManager gm);
        void OnHold(GunManager gm);
        void OnRelease(GunManager gm);
    }
    public class AutomaticAttack : IAttackInputManager
    {
        public void OnClick(GunManager gm)
        {
            if (gm.Current().ammo.Current == 0) { gm.Current().OnActivation(Camera.main.ScreenToWorldPoint(Input.mousePosition)); }
        }

        public void OnHold(GunManager gm)
        {
            if (gm.Current().ammo.Current == 0) { return; }
            gm.Current().OnActivation(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            
        }

        public void OnRelease(GunManager gm)
        {

        }
    }

    public class ChargeAttack : IAttackInputManager
    {
        public void OnClick(GunManager gm)
        {
            if (gm.Current() is IChargeableWeapon weapon)
            {
                weapon.StartCharging();
            }
        }

        public void OnHold(GunManager gm)
        {
        }

        public void OnRelease(GunManager gm)
        {
            gm.Current().OnActivation(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

}