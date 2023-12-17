using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICooldown
{
    bool isAvailable { get; set; }
    float duration { get; set; }

    public void Reset();
}

public class TimeCooldown : ICooldown
{
    float last;
    public float duration { get; set; }
    public bool isAvailable { get { return (Time.time - last) > duration; } set { isAvailable = value; } }

    public void Reset()
    {
        last = Time.time;
    }

}

public class ChargeCooldown : ICooldown
{
    public bool isAvailable { get { return getCharges() > 0; } set { isAvailable = value; } }
    int maxCharges = 0;

    float last;

    int getCharges()
    {
        return (int)Mathf.Clamp(Mathf.Floor((Time.time - last) / duration), 0, maxCharges);
    }

    public ChargeCooldown(int max, int start=0)
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

public class PlayerBasicAttack : MonoBehaviour
{

    ICooldown cd = new ChargeCooldown(3, 3);

    // Start is called before the first frame update
    void Start()
    {
        cd.duration = 3;
        cd.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cd.isAvailable)
            {
                cd.Reset();
                Debug.Log("Woo");
            }
        }

    }
}
