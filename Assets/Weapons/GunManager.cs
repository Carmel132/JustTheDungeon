using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
#nullable enable
public class EffectFactor
{
    float m;
    float b;
    public static float operator*(EffectFactor lhs, float rhs)
    {
        return lhs.m * rhs + lhs.b;
    }
    public static float operator/(EffectFactor lhs, float rhs)
    {
        return (rhs - lhs.b) / lhs.m;
    }
    public static TimeCooldown operator*(EffectFactor lhs, TimeCooldown rhs)
    {
        var ret = new TimeCooldown();
        ret.duration = rhs.duration * lhs.m + lhs.b;
        ret.Reset();
        return ret;
    }
    public static TimeCooldown operator /(EffectFactor lhs, TimeCooldown rhs)
    {
        var ret = new TimeCooldown();
        ret.duration = (rhs.duration - lhs.b) / lhs.m ;
        ret.Reset();
        return ret;
    }

    public EffectFactor(float m, float b)
    {
        this.m = m;
        this.b = b;
    }
}

public interface IGunStatMessages : IEventSystemHandler
{
    void AddStatChange((GunEffectManagerTarget, EffectFactor, TimeCooldown?) f) { }
}

public interface IWeapon : IAbility<Vector3>, IGunStatMessages
{
    //IAbility<Vector3>.OnActivation(Vector3 payload) == OnFire()
    GunEffectManager stats { get; set; }
    void Reload(in object payload) { }
    void IGunStatMessages.AddStatChange((GunEffectManagerTarget, EffectFactor, TimeCooldown?) f)
    {
        stats.Add(stats.newId(), f.Item1, f.Item2, f.Item3);
    }
}

public class GunManager : MonoBehaviour
{
    public (IWeapon?, IWeapon?, IWeapon?) weapons = (null, null, null);
    int current = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IWeapon? get(int idx)
    {
        switch (idx)
        {
            case 0:
                return weapons.Item1;
            case 1:
                return weapons.Item2;
            case 2:
                return weapons.Item3;
        }
        return null;
    }
}
