using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The car to follow
    
    [Header("Camera Position")]
    public Vector3 offset = new Vector3(0, 5, -8); // Position relative to car
    public float followSpeed = 5f; // How fast camera follows
    public float rotationSpeed = 2f; // How fast camera rotates
    
    [Header("Look Settings")]
    public bool lookAtTarget = true; // Should camera always look at car?
    public Vector3 lookOffset = new Vector3(0, 0, 0); // Offset for look position
    
    void LateUpdate()
    {
        if (target == null)
            return;
            
        // Calculate desired position
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        
        // Smoothly move to desired position - use higher speed for better sync
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        
        // Look at the target if enabled
        if (lookAtTarget)
        {
            Vector3 lookPosition = target.position + lookOffset;
            Vector3 direction = lookPosition - transform.position;
            
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
    
    void FixedUpdate()
    {
        // Additional smooth following for physics-based movement
        if (target == null)
            return;
            
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Slerp(transform.position, desiredPosition, followSpeed * Time.fixedDeltaTime * 0.5f);
    }
}