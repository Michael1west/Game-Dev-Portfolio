using UnityEngine;

public class DemonDetection : MonoBehaviour
{
    [Header("Suspicion")]
    [SerializeField] private float suspicion;
    [SerializeField] private float minSuspicion = 10f;
    [SerializeField] private float suspicionThreshold = 25f;
    [SerializeField] private bool seenPlayer = false;
    [SerializeField] private Vector3 lastKnownPosition;
    [SerializeField] private float suspicionDecayRate = 6f;

    [Header("Visual Detection")]
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float visualSuspicionRate = 12f;
    [SerializeField] private float losBreakReduction = 30f;

    [Header("Sound Detection")]
    [SerializeField] private float noiseRange = 20f;
    [SerializeField] private float crouchSuspicionRate = 10f;
    [SerializeField] private float walkSuspicionRate = 20f;
    [SerializeField] private float sprintSuspicionRate = 30f;

    private IThreatTarget target;

    private bool hasLOSToPlayer;
    private bool hadLOSToPlayer;
    private float timeSinceLOSLost;

    public bool HasLOSToPlayer => hasLOSToPlayer;
    public float TimeSinceLOSLost => timeSinceLOSLost;

    public Vector3 LastKnownPosition => lastKnownPosition;
    public bool SeenPlayer => seenPlayer;

    public float SuspicionThreshold => suspicionThreshold;
    public float Suspicion => suspicion;


    void Start()
    {
        target = FindFirstObjectByType<FirstPersonController>() as IThreatTarget;
    }

    void Update()
    {
        // STAGE 1 — Determine if we have LOS to the player THIS FRAME.
        hasLOSToPlayer = false;

        float effectiveDetectionRange = detectionRange + target.visibilityModifier;
        float distance = Vector3.Distance(transform.position, target.targetPosition);
        float angle = Vector3.Angle(transform.forward, target.targetPosition - transform.position);

        if (distance < effectiveDetectionRange && angle < fieldOfView)
        {
            if (Physics.Raycast(transform.position, target.targetPosition - transform.position, out RaycastHit hit, effectiveDetectionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    seenPlayer = true;
                    hasLOSToPlayer = true;
                }
            }
        }

        // STAGE 2 — Update the LOS-lost timer.

        if (hasLOSToPlayer)
        {
            timeSinceLOSLost = 0f;
            lastKnownPosition = target.targetPosition;
        }
        else
        {
            timeSinceLOSLost += Time.deltaTime;
        }

        // STAGE 3 - Suspicion

        // Visual

        if (hasLOSToPlayer)
        {
            suspicion += visualSuspicionRate * Time.deltaTime;
        }

        // Sound

        if (distance < noiseRange)
        {
            switch (target.currentMovementState)
            {
                case MovementState.Crouching:
                    suspicion += crouchSuspicionRate * Time.deltaTime;
                    break;
                case MovementState.Walking:
                    suspicion += walkSuspicionRate * Time.deltaTime;
                    break;
                case MovementState.Sprinting:
                    suspicion += sprintSuspicionRate * Time.deltaTime;
                    break;
            }
            lastKnownPosition = target.targetPosition;
        }

        // Decay
        
        if (!hasLOSToPlayer && distance >= noiseRange)
        {
            suspicion -= suspicionDecayRate * Time.deltaTime;
        }

        // LOS-break penalty

        if (hadLOSToPlayer && !hasLOSToPlayer)
        {
            suspicion -= losBreakReduction;
        }
        hadLOSToPlayer = hasLOSToPlayer;

        // Clamp

        if (seenPlayer)
        {
            suspicion = Mathf.Clamp(suspicion, minSuspicion, 100f);
        }
        else
        {
            suspicion = Mathf.Clamp(suspicion, 0f, 100f);

        } 
    }
}
