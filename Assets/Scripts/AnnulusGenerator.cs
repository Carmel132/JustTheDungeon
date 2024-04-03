using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AnnulusGenerator : MonoBehaviour
{
    [Range(1, 500)]
    public int caps = 8;

    public float radius;
    LineRenderer lineRenderer;
    Vector3[] points;
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector3[caps];
        TryGetComponent(out lineRenderer);
        if (lineRenderer == null ) lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = caps + 1;
        GeneratePoints();
        lineRenderer.SetPositions(ShiftedPoints());
    }

    Vector3[] ShiftedPoints()
    {
        return points.ToList().Select(p =>radius* p + transform.position).ToArray();
    }

    void GeneratePoints()
    {
        if (caps + 1 == points.Length) { return; }
        points = new Vector3[caps + 1];
        float multiplier = Mathf.PI * 2 / caps;
        for (int i = 0; i <= caps; ++i)
        {
            points[i] = new Vector3(Mathf.Cos(multiplier * i), Mathf.Sin(multiplier * i), 0);
        }
    }
}
