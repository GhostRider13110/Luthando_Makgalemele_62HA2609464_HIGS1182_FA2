using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 moveDirection; 

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("You Have Been Hit");

            GameManager.Instance.PlayerDied();
        }
    }

    public void DestroyAsteroid()
    {
        Destroy(gameObject);
    }

}
