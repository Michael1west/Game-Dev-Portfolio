using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    [SerializeField] private float slideSpeed = 14f;
    [SerializeField] private float slideDuration = 0.8f;
    [SerializeField] private float slideSpeedThreshold = 9f;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private float slideHeight = 0.5f;
    [SerializeField] private float slideCenter = 0.25f;
    [SerializeField] private float slideEndSpeed = 10f;
    [SerializeField] private float slideDeceleration = 15f;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Vector3 slideDirection;
    private float slideCounter;
    private float originalHeight;
    private float originalCenter;
    private float currentSlideSpeed;

    public bool isSliding { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        originalHeight = playerCollider.height;
        originalCenter = playerCollider.center.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.SlidePressed && playerInput.SprintHeld && rb.linearVelocity.magnitude >= slideSpeedThreshold)
        {
            isSliding = true;
            Vector3 horizontalVelocity = rb.linearVelocity;
            horizontalVelocity.y = 0f;
            slideDirection = horizontalVelocity.normalized;
            slideCounter = slideDuration;
            playerCollider.height = slideHeight;
            playerCollider.center = new Vector3(0f, slideCenter, 0f);
            currentSlideSpeed = slideSpeed;
        }

    }

    void FixedUpdate()
    {
        if (isSliding)
        {
            currentSlideSpeed = Mathf.MoveTowards(currentSlideSpeed, slideEndSpeed, slideDeceleration * Time.fixedDeltaTime);

            Vector3 slideVelocity = slideDirection * currentSlideSpeed;
            rb.linearVelocity = new Vector3(slideVelocity.x, rb.linearVelocity.y, slideVelocity.z);
            slideCounter -= Time.fixedDeltaTime;
            

            if (slideCounter <= 0)
            {
                isSliding = false;
                playerCollider.height = originalHeight;
                playerCollider.center = new Vector3(0f, originalCenter, 0f);
                
            }
            

        }
    }
}
