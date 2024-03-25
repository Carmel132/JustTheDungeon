using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrentAmmoSpriteManager : MonoBehaviour
{
    [SerializeField]
    GunManager gm;
    Image renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Image>();
        renderer.enabled = true;
        renderer.preserveAspect = true;
    }

    // Update is called once per frame
    void Update()
    {
        renderer.sprite = gm.Current().stats.gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        renderer.color = gm.Current().stats.gameObject.GetComponentInChildren<SpriteRenderer>().color;
        transform.localScale = gm.Current().stats.gameObject.GetComponentInChildren<SpriteRenderer>().transform.localScale;
    }
}
