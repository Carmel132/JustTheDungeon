using System;
using static PlayerEffects;

public struct EffectManager<Target>
{


    public EffectManager(Action<(Target, EffectFactor, TimeCooldown?)> _ImplementEffect, Action<(Target, EffectFactor, TimeCooldown?)> _NullifyEffect)
    {
        effects = new();
        ImplementEffect = _ImplementEffect;
        NullifyEffect = _NullifyEffect;
    }

    System.Collections.Generic.Dictionary<int, (Target, EffectFactor, TimeCooldown?)> effects;
    public void Add(int id, (Target, EffectFactor, TimeCooldown?) effect)
    {
        effect.Item3?.Reset();
        if (effects.ContainsKey(id)) { effects[id] = effect; }
        else { effects.Add(id, effect); }
        ImplementEffect(effect);
    }
    public void Remove(int id)
    {
        var effect = effects[id];
        NullifyEffect(effect);
        effects.Remove(id);
    }

    public int newId()
    {
        int ret;
        do { ret = Util.rnd.Next(); } while (effects.ContainsKey(ret));
        return ret;
    }

    public Action<(Target, EffectFactor, TimeCooldown?)> ImplementEffect;
    public Action<(Target, EffectFactor, TimeCooldown?)> NullifyEffect;

    public void EffectManagerStart()
    {
        System.Collections.Generic.Dictionary<int, (Target, EffectFactor, TimeCooldown?)> effects = new();
    }
    public void EffectManagerUpdate()
    {
        var temp = new System.Collections.Generic.Dictionary<int, (Target, EffectFactor, TimeCooldown?)>();
        foreach (var effect in temp)
        {
            if (effect.Value.Item3 == null) { continue; }
            else if (effect.Value.Item3.isAvailable)
            {
                Remove(effect.Key);
            }
        }
    }
}

public interface IEffectManager<Target>
{
    public EffectManager<Target> effectManager { get; set; }
    public void ImplementEffect((Target, EffectFactor, TimeCooldown?) t);
    public void NullifyEffect((Target, EffectFactor, TimeCooldown?) t);
}