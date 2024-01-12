using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float Damage;
    public HashSet<IEffect> Effects = new HashSet<IEffect>();
}
