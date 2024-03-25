using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoAmountIndicator : MonoBehaviour
{
    [SerializeField]
    GunManager gm;

    TMP_Text text;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.text = $"{gm.Current().ammo.Current}/{gm.Current().ammo.Max}";
    }
}
