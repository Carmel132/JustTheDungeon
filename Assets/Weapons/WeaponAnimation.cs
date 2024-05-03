using System.Linq;
using UnityEngine;


/// <summary>
/// Manages weapon sprite animations
/// </summary>
public class WeaponAnimation : MonoBehaviour
{
    private enum STATE
    {
        RELEASE, CLICK, HOLD
    }
    public Animator animator;


    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetInteger("State", (int)STATE.RELEASE);
    }

    public void OnClick()
    {
        animator.SetInteger("State", (int)STATE.CLICK);
    }
    public void OnHold()
    {
        animator.SetInteger("State", (int)STATE.HOLD);
    }
    public void OnRelease()
    {
        animator.SetInteger("State", (int)STATE.RELEASE);
    }
}
