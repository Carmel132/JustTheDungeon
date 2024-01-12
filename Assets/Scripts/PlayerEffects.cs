using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerEffects : MonoBehaviour
{
    Dictionary<Type, IEffect> Effects = new Dictionary<Type, IEffect>();

    public void Add(IEffect effect)
    {
        if (Effects.ContainsKey(effect.GetType()))
        {
            Effects[effect.GetType()].Add(transform, effect);   
        }
        else
        {
            Effects.Add(effect.GetType(), effect);
            Effects[effect.GetType()].Start(transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in Effects.Where(item => item.Value.remove).ToList())
        {
            item.Value.OnRemove(transform);
            Effects.Remove(item.Key);
        }
    }
}
