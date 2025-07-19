using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWaypointMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float slowSpeed = 2f; // Speed when spacebar is held
    public float rotationSpeed = 2f;
    public float waitTime = 1f; // Time to wait at each waypoint
    
    private float currentMoveSpeed;
    
    [Header("Current Status")]
    public int currentWaypointIndex = 0;
    
    [Header("Audio Settings")]
    public AudioSource engineAudioSource;
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;
    
    private bool isMoving = true;
    private Vector3 lastPosition;
    
    void Start()
    {
        // Make sure we have waypoints
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to CarWaypointMovement!");
            return;
        }
        
        // Get or add AudioSource component
        if (engineAudioSource == null)
        {
            engineAudioSource = GetComponent<AudioSource>();
            if (engineAudioSource == null)
            {
                engineAudioSource = gameObject.AddComponent<AudioSource>();
                engineAudioSource.loop = true;
                engineAudioSource.playOnAwake = true;
            }
        }
        
        // Store initial position
        lastPosition = transform.position;
        
        // Set initial move speed
        currentMoveSpeed = moveSpeed;
        
        // Start moving to the first waypoint
        StartCoroutine(MoveToWaypoints());
    }
    
    IEnumerator MoveToWaypoints()
    {
        while (true)
        {
            // Move to current waypoint
            yield return StartCoroutine(MoveToWaypoint(waypoints[currentWaypointIndex]));
            
            // Wait at waypoint
            yield return new WaitForSeconds(waitTime);
            
            // Move to next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
    
    IEnumerator MoveToWaypoint(Transform targetWaypoint)
    {
        while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
        {
            // Calculate direction to target
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            
            // Rotate towards target
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                // Add 180 degree offset if car is facing backwards
                targetRotation *= Quaternion.Euler(0, 180, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            
            // Move towards target
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, currentMoveSpeed * Time.deltaTime);
            
            yield return null; // Wait for next frame
        }
    }
    
    void Update()
    {
        // Check if spacebar is held down for slow motion
        if (Input.GetKey(KeyCode.Space))
        {
            currentMoveSpeed = slowSpeed; // Slow down when spacebar held
        }
        else
        {
            currentMoveSpeed = moveSpeed; // Normal speed when spacebar released
        }
        
        // Update engine sound based on movement
        UpdateEngineSound();
    }
    
    void UpdateEngineSound()
    {
        if (engineAudioSource != null && engineAudioSource.clip != null)
        {
            // Calculate current speed
            float currentSpeed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
            
            // Normalize speed to pitch range (use currentMoveSpeed for proper scaling)
            float normalizedSpeed = Mathf.Clamp01(currentSpeed / currentMoveSpeed);
            float targetPitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);
            
            // Smoothly adjust pitch
            engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, targetPitch, Time.deltaTime * 2f);
            
            // Update last position
            lastPosition = transform.position;
        }
    }
}