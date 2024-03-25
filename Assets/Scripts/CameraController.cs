using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //TODO: Update camera for room locking (see docs)

    [SerializeField]
    Transform Player;
    Vector3 Offset = new(0, 0, -10);

    void Update()
    {
        transform.position = Player.transform.position + Offset;
    }
}
