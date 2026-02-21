using System;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    private ItemData[] equipmentSlots;
    private ItemData currentItem;

    public ItemData CurrentItem => currentItem;
    public event Action OnInventoryChanged;

    void Awake()
    {
        equipmentSlots = new ItemData[4];
    }

    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i] == null)
            {
                equipmentSlots[i] = item;
                Debug.Log("Added " + item.itemName + " to slot " + i);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }

    public ItemData GetSlotItem(int index)
    {
        if (index >= 0 && index < equipmentSlots.Length)
        {
            return equipmentSlots[index];
        }
        return null;
    }

    public void EquipItem(int slot)
    {
        int index = slot - 1;

        if (equipmentSlots[index] == null)
        {
            Debug.Log("Slot " + slot + " is empty");
            return;
        }

        if (currentItem == equipmentSlots[index])
        {
            UnequipItem();
        }
        else
        {
            currentItem = equipmentSlots[index];
            Debug.Log("Equipped: " + currentItem.itemName);
            OnInventoryChanged?.Invoke();
        }
    }

    public void UnequipItem()
    {
        if (currentItem == null) return;

        Debug.Log("Unequipped: " + currentItem.itemName);
        currentItem = null;
        OnInventoryChanged?.Invoke();
    }

    public ItemData DropItem()
    {
        if (currentItem == null) return null;

        ItemData itemToDrop = currentItem;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if (equipmentSlots[i] == currentItem)
            {
                currentItem = null;
                equipmentSlots[i] = null;
                OnInventoryChanged?.Invoke();
            }
        }

        return itemToDrop;
    }
}