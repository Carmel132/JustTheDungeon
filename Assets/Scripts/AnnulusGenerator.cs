using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnnulusGenerator : MonoBehaviour
{
    [Range(1, 500)]
    public int caps = 8;

    public float radius;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null ) lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = caps + 1;
        lineRenderer.SetPositions(GeneratePoints());
    }

    Vector3[] GeneratePoints()
    {
        Vector3[] points = new Vector3[caps + 1];
        float mult = Mathf.PI * 2 / caps;
        for (int i = 0; i <= caps; ++i)
        {
            points[i] = transform.position + radius * new Vector3(Mathf.Cos(mult * i), Mathf.Sin(mult * i), 0);
        }
        return points;
    }
}
