using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    public string GetPromptText()
    {
        return "Press E to pick up " + itemData.itemName;
    }

    public void Interact()
    {
        Inventory inventory = FindFirstObjectByType<Inventory>();
        if (inventory.AddItem(itemData))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full");
        }
    }
}