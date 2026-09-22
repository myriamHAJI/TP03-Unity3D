using UnityEngine;

public class WorldHealthBar : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 2.3f, 0f);

    private CharacterStats stats;
    private Transform barRoot;
    private Transform fill;

    void Start()
    {
        stats = GetComponent<CharacterStats>();
        CreateBar();
    }

    void LateUpdate()
    {
        if (stats == null || fill == null)
            return;

        float ratio = (float)stats.CurrentHealth / stats.maxHealth;

        fill.localScale = new Vector3(1.2f * ratio, 0.12f, 0.06f);
        fill.localPosition = new Vector3(-0.6f + 0.6f * ratio, 0f, -0.06f);

        if (Camera.main != null)
            barRoot.rotation = Camera.main.transform.rotation;
    }

    void CreateBar()
    {
        GameObject root = new GameObject("HealthBar");
        root.transform.SetParent(transform);
        root.transform.localPosition = offset;
        barRoot = root.transform;

        Transform background = CreatePart("Background", Color.red);
        background.SetParent(barRoot);
        background.localPosition = Vector3.zero;
        background.localScale = new Vector3(1.25f, 0.16f, 0.05f);

        fill = CreatePart("Fill", Color.green);
        fill.SetParent(barRoot);
    }

    Transform CreatePart(string objectName, Color color)
    {
        GameObject part = GameObject.CreatePrimitive(PrimitiveType.Cube);
        part.name = objectName;

        Destroy(part.GetComponent<Collider>());
        part.GetComponent<Renderer>().material.color = color;

        return part.transform;
    }
}