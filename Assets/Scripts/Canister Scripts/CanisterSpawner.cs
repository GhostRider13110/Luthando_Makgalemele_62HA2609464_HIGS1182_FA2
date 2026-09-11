using UnityEngine;

public class CanisterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject canisterPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int canisterCount = 5;
    [SerializeField] private float spawnAreaSize = 15f;
    [SerializeField] private float spawnHeight = 0.5f;

    private void Start()
    {
        SpawnCanister();
    }

    private void SpawnCanister()
    {
        for (int i = 0; i < canisterCount; i++)
        {
            float randomX = Random.Range(-spawnAreaSize,spawnAreaSize);
            float randomZ = Random.Range(-spawnAreaSize,spawnAreaSize);

            Vector3 spawnPosition = new Vector3(
                randomX,
                spawnHeight,
                randomZ
            );

            Instantiate(
                canisterPrefab,
                spawnPosition,
                Quaternion.identity
            );
        }
    }
}
