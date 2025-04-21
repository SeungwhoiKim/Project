using UnityEngine;

public class PlayerOcclusionHighlighter : MonoBehaviour
{
    [Header("Matterial Setting")]
    public Material normalMaterial;
    public Material highlightMaterial;

    [Header("Occlusion Check Setting")]
    public float checkInterval = 0.1f;
    [Range(0f, 1f)]
    public float occlusionThreshold = 0.5f;

    private Renderer rend;
    private float timer = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogWarning("PlayerOcclusionHighlighter: Need Renderer Component.");
        }
        if (normalMaterial != null)
            rend.material = normalMaterial;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            CheckOcclusion();
        }
    }

    void CheckOcclusion()
    {
        if (Camera.main == null || rend == null)
            return;

        Camera cam = Camera.main;
        Vector3 camPos = cam.transform.position;

        Bounds bounds = rend.bounds;
        Vector3[] samplePoints = new Vector3[5];
        samplePoints[0] = bounds.center;
        samplePoints[1] = bounds.min;
        samplePoints[2] = bounds.max;
        samplePoints[3] = new Vector3(bounds.min.x, bounds.center.y, bounds.max.z);
        samplePoints[4] = new Vector3(bounds.max.x, bounds.center.y, bounds.min.z);

        int occludedCount = 0;
        int totalSamples = samplePoints.Length;

        foreach (Vector3 point in samplePoints)
        {
            Vector3 direction = point - camPos;
            float distance = direction.magnitude;
            Ray ray = new Ray(camPos, direction.normalized);

            if (Physics.Raycast(ray, out RaycastHit hit, distance))
            {
                if (hit.transform.root != transform && !hit.transform.CompareTag("Player"))
                {
                    occludedCount++;
                }
            }
            else
            {
            }
        }

        float occlusionRatio = (float)occludedCount / totalSamples;

        if (occlusionRatio >= occlusionThreshold)
        {
            if (rend.material != highlightMaterial)
            {
                rend.material = highlightMaterial;
            }
        }
        else
        {
            if (rend.material != normalMaterial)
            {
                rend.material = normalMaterial;
            }
        }
    }
}
