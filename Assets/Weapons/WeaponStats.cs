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
}
