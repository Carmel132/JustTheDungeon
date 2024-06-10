using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public TimeCooldown reloadSpeed = new();
    public TimeCooldown fireRate;
    public float damage;
    public float speed;
    public float bloom;
    public TimeCooldown chargeDuration;
    public bool reloadable = true;
    public bool barrelGlowEffect = true;
    public bool hasWeaponAnimations = false;
    public bool scrollableTo = true; // Can scroll to in WeaponManager (usually used for residual ability weapons)
    public bool destroyOnEmpty = false;
}
