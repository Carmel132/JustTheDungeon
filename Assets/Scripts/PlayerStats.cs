using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float movementSpeed;
    public int maxHP;
    public int HP;
    public float rollSpeed;
    public TimeCooldown rollCooldown;

    public void Start()
    {
        HP = maxHP;
    }

    public static float SpeedMultiplier = 3 / 2;
}
