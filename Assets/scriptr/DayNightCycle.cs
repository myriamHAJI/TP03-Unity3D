using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float cycleDuration = 120f;

    private Light sun;
    private float timeOfDay = 8f;

    void Awake()
    {
        sun = GetComponent<Light>();
    }

    void Update()
    {
        timeOfDay = (timeOfDay + Time.deltaTime * 24f / cycleDuration) % 24f;

        float angle = timeOfDay * 15f - 90f;
        transform.rotation = Quaternion.Euler(angle, 0f, 0f);

        float daylight = Mathf.Max(0f, Mathf.Sin(angle * Mathf.Deg2Rad));
        sun.intensity = daylight * 1.5f;
    }
}