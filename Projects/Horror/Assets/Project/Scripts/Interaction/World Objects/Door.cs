using UnityEngine;

public class Door : MonoBehaviour, IInteractable, ILockable
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float openAngle = 90f;

    private bool isLocked;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;



    void Awake()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0,openAngle,0);
    }

    void Update()
    {

        if (isOpen)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, openRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, closedRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Interact()
    {
        if (isLocked) return;
        isOpen = !isOpen;   
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }

    public string GetPromptText()
    {
        if (isOpen)
        {
            return "Close door";
        }
        else return "Open door";

    }
}
