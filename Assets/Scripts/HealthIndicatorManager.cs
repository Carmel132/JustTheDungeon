using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthIndicatorManager : MonoBehaviour
{
    [SerializeField]
    private PlayerStats stats;
    public Material mat;
    public TMP_Text text;

    void Start()
    {
        mat.SetFloat("_Health", 1);
    }

    void Update()
    {
        mat.SetFloat("_Health", (float)stats.HP / (float)stats.maxHP);
        text.text = $"{stats.HP}/{stats.maxHP}";
    }
}
