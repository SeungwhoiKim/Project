using UnityEngine;

public class SquirrelSpawner : MonoBehaviour
{
    public GameObject squirrelPrefab;

    public int squirrelCount = 30;

    public Vector2 spawnRangeX = new Vector2(-50f, 50f);
    public Vector2 spawnRangeZ = new Vector2(-50f, 50f);

    public Terrain terrain;

    void Start()
    {
        for (int i = 0; i < squirrelCount; i++)
        {
            float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
            float randomZ = Random.Range(spawnRangeZ.x, spawnRangeZ.y);

            Vector3 spawnPos = new Vector3(randomX, 0, randomZ);

            float terrainHeight = terrain.SampleHeight(spawnPos) + terrain.transform.position.y;
            spawnPos.y = terrainHeight;

            Instantiate(squirrelPrefab, spawnPos, Quaternion.identity);
        }
    }
}
