using UnityEngine;

public class GunRotationManager : MonoBehaviour
{
    private Vector2 mouse;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var m = cam.ScreenToWorldPoint(Input.mousePosition);
        var dmouse = Vector2.zero;
        if ((Vector2)m != mouse)
        {
            dmouse = (Vector2)m - mouse;
        }
        mouse = m;

        var theta = (Vector2.SignedAngle(Vector2.right, (Vector2)transform.position - mouse) + 2.5f * 360) % 360; // Fuck angle periodicity
        transform.rotation = Quaternion.Euler(0, 0, theta);
        //transform.parent.LookAt(mouse);
    }
}
