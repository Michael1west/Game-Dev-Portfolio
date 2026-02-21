using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text[] slotTexts;
    private Inventory inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        inventory.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    void OnDestroy()
    {
        inventory.OnInventoryChanged -= RefreshUI;
    }

    private void RefreshUI()
    {
        for (int i = 0; i < slotTexts.Length; i++)
        {
            ItemData item = inventory.GetSlotItem(i);

            if (item != null)
            {
                
                slotTexts[i].text = item.itemName;
            }
            else
            {
                slotTexts[i].text = (i + 1).ToString();
            }
        }
    }
}
