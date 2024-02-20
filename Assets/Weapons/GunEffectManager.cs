using System.Collections.Generic;
using UnityEngine;
#nullable enable
public enum GunEffectManagerTarget
{
    RELOADSPEED, FIRERATE, DAMAGE, SPEED, BLOOM
}

public class GunEffectManager : MonoBehaviour
{


    public WeaponStats stats;
    Dictionary<int, (GunEffectManagerTarget, EffectFactor, TimeCooldown?)> effects = new Dictionary<int, (GunEffectManagerTarget, EffectFactor, TimeCooldown?)>();
    static System.Random rnd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<WeaponStats>();
    }

    // Update is called once per frame
    void Update()
    {
        var temp = new Dictionary<int, (GunEffectManagerTarget, EffectFactor, TimeCooldown?)>(effects);
        foreach (var effect in temp)
        {
            if (effect.Value.Item3 == null) { continue; }
            else if (effect.Value.Item3.isAvailable)
            {
                Remove(effect.Key);
            }
        }
    }

    public void Add(int id, GunEffectManagerTarget target, EffectFactor f, TimeCooldown? t)
    {
        t?.Reset();
        if (effects.ContainsKey(id)) { effects[id] = (target, f, t); }
        else { effects.Add(id, (target, f, t)); }
        switch (target)
        {
            case GunEffectManagerTarget.RELOADSPEED:
                stats.reloadSpeed = f * stats.reloadSpeed;
                break;
            case GunEffectManagerTarget.FIRERATE:
                stats.fireRate = f * stats.fireRate;
                break;
            case GunEffectManagerTarget.DAMAGE:
                stats.damage = f * stats.damage;
                break;
            case GunEffectManagerTarget.SPEED:
                stats.speed = f * stats.speed;
                break;
            case GunEffectManagerTarget.BLOOM:
                stats.bloom = f * stats.bloom;
                break;
        }
    }

    public void Remove(int id)
    {
        var f = effects[id];
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
        effects.Remove(id);
    }

    public int newId()
    {
        int ret;
        do { ret = rnd.Next(); } while (effects.ContainsKey(ret));
        return ret;
    }
}
