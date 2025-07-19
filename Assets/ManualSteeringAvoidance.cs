using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualSteeringAvoidance : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform[] waypoints;
    public float moveSpeed = 12f;
    public float slowSpeed = 3f;
    public float rotationSpeed = 2f;
    public float waitTime = 1f;
    
    [Header("Manual Steering")]
    public float steeringSpeed = 80f;
    public bool showObstacleWarning = true;
    
    [Header("Obstacle Detection")]
    public float obstacleDetectionDistance = 5f;
    public LayerMask obstacleLayer = -1;
    
    [Header("Collision System")]
    public AudioClip crashSound;
    public bool gameOver = false;
    
    [Header("Audio Settings")]
    public AudioSource engineAudioSource;
    public float minPitch = 0.8f;
    public float maxPitch = 2.0f;
    
    private int currentWaypointIndex = 0;
    private float currentMoveSpeed;
    private Vector3 lastPosition;
    private bool obstacleAhead = false;
    private bool showRetryUI = false;
    private Rigidbody carRigidbody;
    
    void Start()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned!");
            return;
        }
        
        // Setup audio
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
        
        lastPosition = transform.position;
        currentMoveSpeed = moveSpeed;
        
        // Get Rigidbody component
        carRigidbody = GetComponent<Rigidbody>();
        if (carRigidbody == null)
        {
            Debug.LogError("No Rigidbody found! Please add Rigidbody to CarContainer");
        }
        
        StartCoroutine(MoveToWaypoints());
    }
    
    IEnumerator MoveToWaypoints()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToWaypoint(waypoints[currentWaypointIndex]));
            yield return new WaitForSeconds(waitTime);
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
    
    IEnumerator MoveToWaypoint(Transform targetWaypoint)
    {
        while (Vector3.Distance(transform.position, targetWaypoint.position) > 2f && !gameOver)
        {
            // Check for obstacles ahead
            CheckForObstacles();
            
            // Calculate direction to target waypoint
            Vector3 directionToTarget = (targetWaypoint.position - transform.position).normalized;
            
            // Check for manual steering input
            float steeringInput = 0f;
            bool canSteer = Input.GetKey(KeyCode.Space); // Must hold spacebar to steer
            
            if (canSteer)
            {
                if (Input.GetKey(KeyCode.A))
                    steeringInput = -1f; // Turn left
                if (Input.GetKey(KeyCode.D))
                    steeringInput = 1f;  // Turn right
            }
            
            if (steeringInput != 0 && canSteer)
            {
                // Manual steering - player is avoiding obstacles (spacebar + A/D)
                transform.Rotate(0, steeringInput * steeringSpeed * Time.deltaTime, 0);
                // Use Rigidbody for physics-based movement
                if (carRigidbody != null)
                {
                    carRigidbody.MovePosition(transform.position + transform.forward * slowSpeed * Time.deltaTime);
                }
            }
            else
            {
                // Default behavior - move straight toward waypoint (will hit obstacles!)
                // Rotate towards target waypoint
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    targetRotation *= Quaternion.Euler(0, 180, 0); // Fix car orientation
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                
                // Use Rigidbody for physics-based movement
                if (carRigidbody != null)
                {
                    Vector3 targetPosition = Vector3.MoveTowards(transform.position, targetWaypoint.position, currentMoveSpeed * Time.deltaTime);
                    carRigidbody.MovePosition(targetPosition);
                }
            }
            
            yield return null;
        }
    }
    
    void CheckForObstacles()
    {
        // Cast a ray forward to detect obstacles
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, obstacleDetectionDistance, obstacleLayer))
        {
            obstacleAhead = true;
            if (showObstacleWarning)
            {
                Debug.Log("Obstacle ahead! Press Spacebar + A or D to avoid!");
            }
        }
        else
        {
            obstacleAhead = false;
        }
    }
    
    // Detect collision with obstacles
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name); // Debug message
        
        if (collision.gameObject.name.Contains("Obstacle"))
        {
            CrashIntoObstacle();
        }
    }
    
    void CrashIntoObstacle()
    {
        gameOver = true;
        showRetryUI = true;
        
        // Stop all movement
        StopAllCoroutines();
        
        // Play crash sound
        if (crashSound != null && engineAudioSource != null)
        {
            engineAudioSource.PlayOneShot(crashSound);
        }
        
        Debug.Log("CRASHED! Game Over!");
    }
    
    void Update()
    {
        // Check for retry input
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R key pressed - attempting restart"); // Debug message
            RestartGame();
        }
        
        if (!gameOver)
        {
            // Spacebar always slows down the car (original behavior)
            if (Input.GetKey(KeyCode.Space))
            {
                currentMoveSpeed = slowSpeed;
            }
            else
            {
                currentMoveSpeed = moveSpeed;
            }
            
            UpdateEngineSound();
        }
    }
    
    void RestartGame()
    {
        Debug.Log("Restarting game..."); // Debug message
        
        gameOver = false;
        showRetryUI = false;
        
        // Reset car position to first waypoint
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            transform.rotation = Quaternion.identity; // Reset rotation
            
            if (carRigidbody != null)
            {
                carRigidbody.velocity = Vector3.zero; // Stop any movement
                carRigidbody.angularVelocity = Vector3.zero; // Stop any rotation
            }
            currentWaypointIndex = 0;
        }
        
        // Restart movement coroutine
        StopAllCoroutines(); // Stop any existing coroutines first
        StartCoroutine(MoveToWaypoints());
        
        Debug.Log("Game restarted!"); // Debug message
    }
    
    void UpdateEngineSound()
    {
        if (engineAudioSource != null && engineAudioSource.clip != null)
        {
            float currentSpeed = Vector3.Distance(transform.position, lastPosition) / Time.deltaTime;
            float normalizedSpeed = Mathf.Clamp01(currentSpeed / currentMoveSpeed);
            float targetPitch = Mathf.Lerp(minPitch, maxPitch, normalizedSpeed);
            
            engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, targetPitch, Time.deltaTime * 2f);
            lastPosition = transform.position;
        }
    }
    
    // Visual debugging - shows obstacle detection ray
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Draw detection ray
            Gizmos.color = obstacleAhead ? Color.red : Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * obstacleDetectionDistance);
        }
    }
    
    // Display UI warning when obstacle is detected
    void OnGUI()
    {
        if (showRetryUI)
        {
            // Game Over Screen
            GUI.color = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 60, 200, 40), "CRASHED!");
            GUI.color = Color.white;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 20, 200, 30), "You hit an obstacle!");
            
            // Retry Button
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 20, 100, 30), "Retry (R)"))
            {
                RestartGame();
            }
        }
        else if (obstacleAhead && showObstacleWarning && !gameOver)
        {
            GUI.color = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 150, 50, 300, 30), "OBSTACLE AHEAD!");
            GUI.color = Color.white;
            GUI.Label(new Rect(Screen.width / 2 - 150, 80, 300, 30), "Hold SPACEBAR + A (Left) or D (Right) to avoid");
        }
    }
}