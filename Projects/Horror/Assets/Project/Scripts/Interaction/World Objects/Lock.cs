using UnityEngine;

public class Lock : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredItem;
    private ILockable lockable;

    void Start()
    {
        lockable = GetComponentInParent<ILockable>();
        lockable.SetLocked(true);
    }

    void OnDestroy()
    {
        lockable.SetLocked(false);
    }

    public string GetPromptText()
    {
        return "Locked";
    }

    public void Interact()
    {
        Inventory inventory = FindFirstObjectByType<Inventory>();

        if (inventory.HasItem(requiredItem))
        {
            Destroy(this);
        }
        else
        {
            Debug.Log("Missing: " + requiredItem.name);
        }

    }
}
