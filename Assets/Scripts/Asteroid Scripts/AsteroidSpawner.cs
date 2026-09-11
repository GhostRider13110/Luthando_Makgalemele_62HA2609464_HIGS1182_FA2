using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;

    [Header("Spawn Area")]
    [SerializeField] private float spawnInterval = 2f; 
    [SerializeField] private float spawnHeight = 0.5f;
    [SerializeField] private float spawnDistance = 0.5f;

    [Header("Asteroid Settings")]
    [SerializeField] private float asteroidSpeed = 5f; 

    private void Start()
    {
       InvokeRepeating(nameof(SpawnAsteroids), 1f, spawnInterval);
    }

    private void SpawnAsteroids()
    {

        float angle = Random.Range(0f, 360f);

        float angleInRadians = angle * Mathf.Deg2Rad;

        Vector3 spawnPosition = new Vector3(
            Mathf.Cos(angleInRadians) * spawnDistance,
            spawnHeight,
            Mathf.Sin(angleInRadians) * spawnDistance
        );

        GameObject asteroidObject = Instantiate(
            asteroidPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Vector3 randomDirection = Random.insideUnitSphere;

        randomDirection.y = 0f; 

        Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();

        if ( asteroid != null )
        {
            asteroid.SetDirection(randomDirection);
        }
    }
}

