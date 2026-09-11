using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Camera Position")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -7f);

    [Header("Camera Smoothing")]
    [SerializeField] private float smoothTime = 0.15f; 

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) 
        {
            Debug.LogError("CameraFollow target is NULL. Assign the Player ");
            return;
        } 

        // This Calculates where the camera should be
        Vector3 targetPosition = target.position + offset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
            );  // This Smoothly moves the camera toward that position 

        transform.LookAt( target );  //Keeps the camera looking at the player
    }
}
