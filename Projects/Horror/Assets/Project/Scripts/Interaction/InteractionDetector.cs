using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private TMP_Text promptText;

    private PlayerInput playerInput;
    private IInteractable currentInteractable;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        
    }

    void Update()
    {
        currentInteractable = null;


        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hitInfo_, 3f))
        {
            IInteractable interactable = hitInfo_.collider.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                promptText.text = currentInteractable.GetPromptText();
            }
        }

        if (currentInteractable == null)
        {
            promptText.text = "";
        }

        if (playerInput.InteractPressed && currentInteractable != null)
        {
            currentInteractable.Interact();
        }

        
    }
}
