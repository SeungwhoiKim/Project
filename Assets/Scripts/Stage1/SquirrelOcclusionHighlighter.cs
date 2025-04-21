using UnityEngine;

public class SquirrelOcclusionHighlighter : MonoBehaviour
{
    [Header("Materials")]
    public Material normalMaterial;
    public Material highlightMaterial;

    [Header("Occlusion Settings")]
    public float checkInterval = 0.2f;
    [Range(0f, 1f)] public float occlusionThreshold = 0.6f;

    private Renderer rend;
    private float timer = 0f;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        if (rend == null)
        {
            Debug.LogError("SquirrelOcclusionHighlighter: Can't find Renderer!");
            enabled = false;
            return;
        }
        if (normalMaterial == null) normalMaterial = rend.material;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;
        CheckOcclusion();
    }

    void CheckOcclusion()
    {
        if (Camera.main == null) return;

        Vector3 camPos = Camera.main.transform.position;
        Bounds b = rend.bounds;

        Vector3[] pts = new Vector3[]
        {
            b.center,
            b.min,
            b.max,
            new Vector3(b.min.x, b.center.y, b.max.z),
            new Vector3(b.max.x, b.center.y, b.min.z)
        };

        int occluded = 0;
        foreach (var pt in pts)
        {
            Vector3 dir = pt - camPos;
            float dist = dir.magnitude;
            Ray ray = new Ray(camPos, dir.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, dist))
            {
                if (hit.transform != transform && !hit.collider.transform.IsChildOf(transform))
                    occluded++;
            }
        }

        float ratio = (float)occluded / pts.Length;
        Material toUse = (ratio >= occlusionThreshold && highlightMaterial != null)
                         ? highlightMaterial : normalMaterial;
        if (rend.material != toUse)
            rend.material = toUse;
    }
}
