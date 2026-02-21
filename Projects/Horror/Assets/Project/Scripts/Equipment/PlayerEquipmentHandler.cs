using UnityEngine;

public class PlayerEquipmentHandler : MonoBehaviour
{
    [SerializeField] private Transform equipmentHolder;
    [SerializeField] private float throwStrength = 3f;

    private GameObject currentSpawnedEquipment;
    private PlayerInput playerInput;
    private Inventory inventory;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        bool equipmentChanged = false;

        equipmentChanged |= HandleEquipInput();
        HandleUseInput();

        if (HandleDropInput())
            return; // Drop handles its own visual update

        if (equipmentChanged)
            UpdateEquipmentVisual();
    }

    // Checks number keys 1-4 and equips the corresponding slot
    private bool HandleEquipInput()
    {
        if (playerInput.EquipSlot1Pressed) { inventory.EquipItem(1); return true; }
        if (playerInput.EquipSlot2Pressed) { inventory.EquipItem(2); return true; }
        if (playerInput.EquipSlot3Pressed) { inventory.EquipItem(3); return true; }
        if (playerInput.EquipSlot4Pressed) { inventory.EquipItem(4); return true; }
        return false;
    }

    // Tells the held item to perform its unique action (e.g. torch toggles light)
    private void HandleUseInput()
    {
        if (!playerInput.UseItemPressed || currentSpawnedEquipment == null) return;

        if (currentSpawnedEquipment.TryGetComponent(out IEquippable equippable))
            equippable.OnUse();
    }

    // Releases the held item into the world with physics and throw force
    private bool HandleDropInput()
    {
        if (!playerInput.DropItemPressed) return false;

        ItemData item = inventory.DropItem();
        if (item == null) return false;

        ReleaseItemIntoWorld();
        UpdateEquipmentVisual();
        return true;
    }

    // Detaches the held object from the player, adds physics, and throws it
    private void ReleaseItemIntoWorld()
    {
        // Detach from equipment holder so it exists independently in the scene
        currentSpawnedEquipment.transform.SetParent(null);

        // Notify the item it has been dropped (e.g. torch keeps its light state)
        if (currentSpawnedEquipment.TryGetComponent(out IEquippable equippable))
            equippable.OnDrop();

        // Add physics if not already present, then apply throw force
        Rigidbody rb = currentSpawnedEquipment.GetComponent<Rigidbody>();
        if (rb == null) rb = currentSpawnedEquipment.AddComponent<Rigidbody>();
        rb.AddForce((transform.forward + Vector3.up * 0.5f) * throwStrength, ForceMode.Impulse);

        currentSpawnedEquipment = null;
    }

    // Destroys the current held visual and spawns a new one for the equipped item
    private void UpdateEquipmentVisual()
    {
        DestroyHeldVisual();
        SpawnHeldVisual();
    }

    // Notifies old item it is being unequipped, then removes it from the scene
    private void DestroyHeldVisual()
    {
        if (currentSpawnedEquipment == null) return;

        if (currentSpawnedEquipment.TryGetComponent(out IEquippable equippable))
            equippable.OnUnequip();

        Destroy(currentSpawnedEquipment);
        currentSpawnedEquipment = null;
    }

    // Spawns the current item's prefab in the equipment holder and notifies it
    private void SpawnHeldVisual()
    {
        if (inventory.CurrentItem == null || inventory.CurrentItem.prefab == null) return;

        currentSpawnedEquipment = Instantiate(inventory.CurrentItem.prefab, equipmentHolder);

        if (currentSpawnedEquipment.TryGetComponent(out IEquippable equippable))
            equippable.OnEquip();
    }
}