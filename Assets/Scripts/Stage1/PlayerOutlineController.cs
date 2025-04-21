using UnityEngine;

public class PlayerOutlineController : MonoBehaviour
{
    public Material outlineMaterial;
    public float outlineScaleMultiplier = 1.05f;

    private GameObject outlineObject;

    void Start()
    {
        CreateOutline();
    }

    void CreateOutline()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        if (mf == null || mr == null)
        {
            Debug.LogWarning("Player object needs MeshFilter and MeshRenderer.");
            return;
        }

        outlineObject = new GameObject("PlayerOutline");
        outlineObject.transform.SetParent(transform, false);
        outlineObject.transform.localPosition = Vector3.zero;
        outlineObject.transform.localRotation = Quaternion.identity;
        outlineObject.transform.localScale = Vector3.one * outlineScaleMultiplier;

        MeshFilter outlineMF = outlineObject.AddComponent<MeshFilter>();
        outlineMF.mesh = mf.mesh;

        MeshRenderer outlineMR = outlineObject.AddComponent<MeshRenderer>();
        outlineMR.material = outlineMaterial;

        outlineMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        outlineMR.receiveShadows = false;
    }
}
