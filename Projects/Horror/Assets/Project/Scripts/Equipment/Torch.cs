using UnityEngine;

public class Torch : MonoBehaviour, IEquippable
{
    private Light torchLight;
    private bool torchOn = false;

    void Start()
    {
        torchLight = GetComponentInChildren<Light>();
    }

    public void OnEquip() 
    {
    
    }
    public void OnUnequip()
    {
        if (torchLight != null)
        {
            torchOn = false;
            torchLight.enabled = torchOn;
        }
    }
    public void OnDrop()
    {
        
    }
    public void OnUse()
    {
        if (torchLight != null)
        {
            torchOn = !torchOn;
            torchLight.enabled = torchOn;
        }

    }
}
