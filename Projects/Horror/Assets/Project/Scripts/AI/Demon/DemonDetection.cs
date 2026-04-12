using UnityEngine;

public class DemonDetection : MonoBehaviour
{
    [Header("Suspicion")]
    [SerializeField] private float suspicion;
    [SerializeField] private float minSuspicion = 10f;
    [SerializeField] private float alertThreshold = 25f;
    [SerializeField] private float huntThreshold = 60f;
    [SerializeField] private bool seenPlayer = false;
    [SerializeField] private Vector3 lastKnownPosition;
    [SerializeField] private float suspicionDecayRate = 6f;
    

    [Header("Visual Detection")]
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float visualSuspicionRate = 12f;
    [SerializeField] private float losBreakReduction = 10f;
    private bool hasLineOfSight;
    private bool hadLineOfSight; 

    [Header("Sound Detection")]
    [SerializeField] private float noiseRange = 20f;
    [SerializeField] private float crouchSuspicionRate = 10f;
    [SerializeField] private float walkSuspicionRate = 20f;
    [SerializeField] private float sprintSuspicionRate = 30f;
    

    private bool hasBeenAlerted = false;
    private bool hasSearched = false;
    private bool hasRoamed = false;

    public bool HasBeenAlerted => hasBeenAlerted;
    public bool HasSearched => hasSearched;
    public bool HasRoamed => hasRoamed;

    private IThreatTarget target;

    void Start()
    {
        target = FindFirstObjectByType<FirstPersonController>() as IThreatTarget;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.targetPosition);
        float angle = Vector3.Angle(transform.forward, target.targetPosition - transform.position);

        if (distance < detectionRange && angle < fieldOfView)
        {
            if (Physics.Raycast(transform.position, target.targetPosition - transform.position, out RaycastHit hit, detectionRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    seenPlayer = true;
                    hasLineOfSight = true;
                    lastKnownPosition = target.targetPosition;
                    suspicion += visualSuspicionRate * Time.deltaTime;

                    if (suspicion >= alertThreshold)
                    {
                        hasBeenAlerted = true;
                    }
                }
                else
                {
                    hasLineOfSight = false;
                }
            }
        }
        else
        {
            suspicion -= suspicionDecayRate * Time.deltaTime;
            hasLineOfSight = false;
        }

        if (hadLineOfSight && !hasLineOfSight)
        {
            suspicion -= losBreakReduction;
        }
        hadLineOfSight = hasLineOfSight;



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
        }
        


        if (seenPlayer)
        {
            suspicion = Mathf.Clamp(suspicion, minSuspicion, 100f);
        }
        else
        {
            suspicion = Mathf.Clamp(suspicion, 0f, 100f);

        } 
    }

    public void ResetAlertMemory()
    {
        hasBeenAlerted = false;
    }

    public void SetHasSearched()
    {
        hasSearched = true;
    }

    public void ResetSearchMemory()
    {
        hasSearched = false;
    }

    public void SetHasRoamed()
    {
        hasRoamed = true;
    }

    public void ResetRoamMemory()
    {
        hasRoamed = false;
    }

    public bool IsAlert() => suspicion >= alertThreshold;
    public bool IsHunting() => suspicion >= huntThreshold;
    public Vector3 LastKnownPosition => lastKnownPosition;
    public bool SeenPlayer => seenPlayer;
}
