using UnityEngine;

[ExecuteAlways]
public class BezierCurve : MonoBehaviour
{
    public bool cubic = true;
    public int resolution = 40;
    public Transform[] points = new Transform[4];

    private LineRenderer line;

    void Reset()
    {
        line = GetComponent<LineRenderer>();

        if (line == null)
            line = gameObject.AddComponent<LineRenderer>();

        points[0] = CreatePoint("P0", new Vector3(-4, 0.2f, 0));
        points[1] = CreatePoint("P1", new Vector3(-2, 3, 0));
        points[2] = CreatePoint("P2", new Vector3(2, 3, 0));
        points[3] = CreatePoint("P3", new Vector3(4, 0.2f, 0));

        line.startWidth = 0.12f;
        line.endWidth = 0.12f;
        line.startColor = Color.cyan;
        line.endColor = Color.cyan;
    }

    Transform CreatePoint(string pointName, Vector3 position)
    {
        GameObject point = new GameObject(pointName);
        point.transform.SetParent(transform);
        point.transform.localPosition = position;
        return point.transform;
    }

    void Update()
    {
        if (line == null)
            line = GetComponent<LineRenderer>();

        if (points.Length < 4 || points[0] == null)
            return;

        line.positionCount = resolution + 1;

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            line.SetPosition(i, cubic ? Cubic(t) : Quadratic(t));
        }
    }

    Vector3 Quadratic(float t)
    {
        float u = 1f - t;

        return u * u * points[0].position
             + 2f * u * t * points[1].position
             + t * t * points[2].position;
    }

    Vector3 Cubic(float t)
    {
        float u = 1f - t;

        return u * u * u * points[0].position
             + 3f * u * u * t * points[1].position
             + 3f * u * t * t * points[2].position
             + t * t * t * points[3].position;
    }

    void OnDrawGizmos()
    {
        if (points == null)
            return;

        Gizmos.color = Color.yellow;

        foreach (Transform point in points)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.18f);
        }
    }
}