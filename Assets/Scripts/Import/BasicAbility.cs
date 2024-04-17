using System;
using UnityEngine;

/// <summary>
/// Defines a cooldown
/// </summary>
public interface ICooldown
{
    bool isAvailable { get; set; }
    public float duration { get; set; }

    public void Reset();
}

/// <summary>
/// Single timer cooldown
/// </summary>
[System.Serializable]
public class TimeCooldown : ICooldown
{
    float last;
    [field: SerializeField]
    public float duration { get; set; }
    public bool isAvailable { get { return (Time.time - last) > duration; } set { isAvailable = value; } }

    public void Reset()
    {
        last = Time.time;
    }

    public void Finish()
    {
        last -= duration;
    }

    public TimeCooldown(float? _d = null)
    {
        if (_d is float v) { duration = v; }
    }

    public float percentDone { get => Mathf.Clamp01((Time.time - last) / duration); }
}

/// <summary>
/// Charge based timer
/// </summary>
[System.Serializable]
public class ChargeCooldown : ICooldown
{
    public bool isAvailable { get { return getCharges() > 0; } set { isAvailable = value; } }
    int maxCharges = 0;

    float last;

    int getCharges()
    {
        return (int)Mathf.Clamp(Mathf.Floor((Time.time - last) / duration), 0, maxCharges);
    }

    public ChargeCooldown(int max, int start = 0)
    {
        maxCharges = max;
        last -= start * duration;
    }

    public float duration { get; set; }

    public void Reset()
    {
        if (isAvailable)
        {
            if (getCharges() == maxCharges)
            {
                last = Time.time - duration * (maxCharges - 1);
            }
            else
            {
                last += duration;
            }

        }
        else
        {
            last = Time.time;
        }
    }
}