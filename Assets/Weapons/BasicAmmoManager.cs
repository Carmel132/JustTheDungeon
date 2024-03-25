using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class BasicAmmoManager : MonoBehaviour
{

    [SerializeField]
    int currentLoadedBullets;
    [SerializeField]
    int bulletsCarried;
    [SerializeField]
    int magSize;
    [SerializeField]
    int decrement;

    public int Current { get => currentLoadedBullets; set { currentLoadedBullets = value; } }
    public int Max { get => bulletsCarried; }

    public BasicAmmoManager(int _bulletsCarried, int _magSize, int _decrement, int start = -1)
    {
        bulletsCarried = _bulletsCarried;
        magSize = _magSize;
        decrement = _decrement;
        currentLoadedBullets = start < 0 ? _magSize : start;
    }

    public void Reload()
    {
        if (bulletsCarried <= 0) return;
        int bulletsNeeded = magSize - currentLoadedBullets;
        if (bulletsNeeded <= 0) return;
        int bulletsLoadedFromCarried = Mathf.Min(bulletsNeeded, bulletsCarried);
        currentLoadedBullets += bulletsLoadedFromCarried;
        bulletsCarried -= bulletsLoadedFromCarried;
    }

    public bool isReloadable { get { return bulletsCarried > 0 && magSize - currentLoadedBullets > 0; } }


    public void OnActivation()
    {
        currentLoadedBullets = Mathf.Max(currentLoadedBullets - decrement, 0);
    }
}
