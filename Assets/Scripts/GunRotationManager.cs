using UnityEngine;

/// <summary>
/// Has gun pointing towards cursor
/// </summary>
public class GunRotationManager : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main; // Main camera
    }

    void Update()
    {
        var m = cam.ScreenToWorldPoint(Input.mousePosition); // Gets the mouse position in world coordinates

        var theta = (Vector2.SignedAngle(Vector2.right, (Vector2)(transform.position - m)) + 2.5f * 360) % 360; // Calculates angle between position vector and mouse vector
        transform.rotation = Quaternion.Euler(0, 0, theta); // Sets rotation of game object to the angle
    }
}
