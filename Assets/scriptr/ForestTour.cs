using UnityEngine;

public class ForestTour : MonoBehaviour
{
    public float duration = 15f;
    public float returnDuration = 2f;

    public GameObject treePrefab;
    public GameObject grassPrefab;

    private float timer;
    private float returnTimer;

    private OrbitCamera orbit;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Vector3 returnStartPosition;
    private Quaternion returnStartRotation;

    private bool returning;

    private Vector3 p0 = new Vector3(-30f, 7f, -30f);
    private Vector3 p1 = new Vector3(-20f, 12f, 25f);
    private Vector3 p2 = new Vector3(20f, 9f, -25f);
    private Vector3 p3 = new Vector3(30f, 7f, 30f);

    private const int pathSteps = 100;
    private float[] pathLengths;
    private float totalPathLength;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        orbit = GetComponent<OrbitCamera>();

        if (orbit != null)
            orbit.enabled = false;

        BuildPathTable();
        CreateForest();
    }

    void Update()
    {
        if (!returning)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / duration);

            transform.position = GetConstantSpeedPosition(progress);

            Vector3 nextPosition =
                GetConstantSpeedPosition(Mathf.Min(progress + 0.005f, 1f));

            Vector3 direction = nextPosition - transform.position;

            if (direction.sqrMagnitude > 0.001f)
                transform.rotation =
                    Quaternion.LookRotation(direction.normalized);

            if (timer >= duration)
            {
                returning = true;
                returnStartPosition = transform.position;
                returnStartRotation = transform.rotation;
            }

            return;
        }

        returnTimer += Time.deltaTime;

        float returnProgress =
            Mathf.Clamp01(returnTimer / returnDuration);

        float smoothProgress =
            Mathf.SmoothStep(0f, 1f, returnProgress);

        transform.position = Vector3.Lerp(
            returnStartPosition,
            originalPosition,
            smoothProgress
        );

        transform.rotation = Quaternion.Slerp(
            returnStartRotation,
            originalRotation,
            smoothProgress
        );

        if (returnProgress >= 1f)
        {
            if (orbit != null)
                orbit.enabled = true;

            enabled = false;
        }
    }

    void BuildPathTable()
    {
        pathLengths = new float[pathSteps + 1];
        pathLengths[0] = 0f;

        Vector3 previousPosition = Cubic(0f);
        totalPathLength = 0f;

        for (int i = 1; i <= pathSteps; i++)
        {
            float t = i / (float)pathSteps;
            Vector3 currentPosition = Cubic(t);

            totalPathLength += Vector3.Distance(
                previousPosition,
                currentPosition
            );

            pathLengths[i] = totalPathLength;
            previousPosition = currentPosition;
        }
    }

    Vector3 GetConstantSpeedPosition(float progress)
    {
        float targetDistance = progress * totalPathLength;

        for (int i = 1; i <= pathSteps; i++)
        {
            if (pathLengths[i] >= targetDistance)
            {
                float sectionLength =
                    pathLengths[i] - pathLengths[i - 1];

                float sectionProgress = sectionLength > 0f
                    ? (targetDistance - pathLengths[i - 1])
                      / sectionLength
                    : 0f;

                float previousT = (i - 1) / (float)pathSteps;
                float currentT = i / (float)pathSteps;

                float t = Mathf.Lerp(
                    previousT,
                    currentT,
                    sectionProgress
                );

                return Cubic(t);
            }
        }

        return Cubic(1f);
    }

    Vector3 Cubic(float t)
    {
        float u = 1f - t;

        return
            u * u * u * p0 +
            3f * u * u * t * p1 +
            3f * u * t * t * p2 +
            t * t * t * p3;
    }

    void CreateForest()
    {
        GameObject oldForest =
            GameObject.Find("GeneratedForest");

        if (oldForest != null)
            Destroy(oldForest);

        GameObject forest =
            new GameObject("GeneratedForest");

        TerrainData terrainData = new TerrainData();

        terrainData.heightmapResolution = 33;
        terrainData.size = new Vector3(80f, 5f, 80f);

        GameObject terrainObject =
            Terrain.CreateTerrainGameObject(terrainData);

        terrainObject.name = "ForestTerrain";

        terrainObject.transform.position =
            new Vector3(-40f, -0.1f, -40f);

        terrainObject.transform.SetParent(forest.transform);

        AddTerrainTexture(terrainData);

        Random.InitState(7);

        CreateTrees(forest.transform);
        CreateGrass(forest.transform);
    }

    void AddTerrainTexture(TerrainData terrainData)
    {
        Texture2D groundTexture =
            new Texture2D(64, 64);

        groundTexture.name = "ForestGroundTexture";
        groundTexture.wrapMode = TextureWrapMode.Repeat;
        groundTexture.filterMode = FilterMode.Bilinear;

        Color darkGreen =
            new Color(0.10f, 0.28f, 0.07f);

        Color lightGreen =
            new Color(0.24f, 0.48f, 0.12f);

        Color brown =
            new Color(0.28f, 0.20f, 0.09f);

        for (int y = 0; y < groundTexture.height; y++)
        {
            for (int x = 0; x < groundTexture.width; x++)
            {
                float noise = Mathf.PerlinNoise(
                    x * 0.12f,
                    y * 0.12f
                );

                Color color;

                if (noise < 0.32f)
                    color = brown;
                else
                    color = Color.Lerp(
                        darkGreen,
                        lightGreen,
                        noise
                    );

                groundTexture.SetPixel(x, y, color);
            }
        }

        groundTexture.Apply();

        TerrainLayer groundLayer = new TerrainLayer();

        groundLayer.name = "ForestGroundLayer";
        groundLayer.diffuseTexture = groundTexture;
        groundLayer.tileSize = new Vector2(8f, 8f);
        groundLayer.metallic = 0f;
        groundLayer.smoothness = 0f;

        terrainData.terrainLayers =
            new TerrainLayer[] { groundLayer };
    }

    void CreateTrees(Transform forest)
    {
        if (treePrefab == null)
        {
            Debug.LogWarning(
                "Ajoute Pine_Tree_19 dans Tree Prefab."
            );

            return;
        }

        for (int i = 0; i < 60; i++)
        {
            Vector3 position = FindForestPosition(5f);

            Quaternion rotation = Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            );

            GameObject tree = Instantiate(
                treePrefab,
                position,
                rotation,
                forest
            );

            float scale = Random.Range(0.8f, 1.35f);

            tree.transform.localScale *= scale;
        }
    }

    void CreateGrass(Transform forest)
    {
        if (grassPrefab == null)
        {
            Debug.LogWarning(
                "Ajoute Grass_2 dans Grass Prefab."
            );

            return;
        }

        for (int i = 0; i < 180; i++)
        {
            Vector3 position = FindForestPosition(2.5f);

            Quaternion rotation = Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            );

            GameObject grass = Instantiate(
                grassPrefab,
                position,
                rotation,
                forest
            );

            float scale = Random.Range(0.8f, 1.4f);

            grass.transform.localScale *= scale;
        }
    }

    Vector3 FindForestPosition(float pathDistance)
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            float x = Random.Range(-37f, 37f);
            float z = Random.Range(-37f, 37f);

            if (!IsNearCameraPath(x, z, pathDistance))
                return new Vector3(x, 0f, z);
        }

        return new Vector3(
            Random.Range(-37f, 37f),
            0f,
            Random.Range(-37f, 37f)
        );
    }

    bool IsNearCameraPath(
        float x,
        float z,
        float minimumDistance
    )
    {
        Vector2 position = new Vector2(x, z);

        for (int i = 0; i <= 30; i++)
        {
            float t = i / 30f;
            Vector3 pathPosition = Cubic(t);

            Vector2 pathPoint = new Vector2(
                pathPosition.x,
                pathPosition.z
            );

            if (Vector2.Distance(position, pathPoint)
                < minimumDistance)
            {
                return true;
            }
        }

        return false;
    }
}