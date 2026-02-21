using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredKey;
    private bool isOpen = false;

    public string GetPromptText()
    {
        if (isOpen)
        {
            return "";
        }
        return "Press E to open door";
    }

    public void Interact()
    {
        if (isOpen) return;

        Inventory inventory = FindFirstObjectByType<Inventory>();

        if (inventory.CurrentItem == requiredKey)
        {
            isOpen = true;
            Debug.Log("Door unlocked!");
        }
        else 
        {
            Debug.Log("Its locked. I need a key.");        
        }
    }
}
