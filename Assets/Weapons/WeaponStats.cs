using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public TimeCooldown reloadSpeed = new TimeCooldown();
    public TimeCooldown fireRate;
    public float damage;
    public float speed;
    public float bloom;
    public bool reloadable = true;
}
