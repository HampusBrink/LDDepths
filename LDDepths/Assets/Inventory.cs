using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    bool hasDynamite = false;
    bool hasFuze = false;
    bool hasLighter = false;
    
    public UIManager uiManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            if (other.TryGetComponent(out Collectible collectible))
            {
                switch (collectible.collectibleType)
                {
                    case Collectible.CollectibleType.Dynamite: 
                        hasDynamite = true;
                        Debug.Log("Collected Dynamite");
                        break;
                    case Collectible.CollectibleType.Fuze:
                        hasFuze = true;
                        Debug.Log("Collected Fuze");
                        break;
                    case Collectible.CollectibleType.Lighter:
                        hasLighter = true;
                        Debug.Log("Collected Lighter");
                        break;
                    default:
                        Debug.Log("I HAVE NOTING");
                        break;
                }
                collectible.CollectibleCollect();
                uiManager.UpdateUnlockUI(collectible.collectibleType);
            }
            else
            {
                Debug.Log("Collectible doesn't have Component");
            }
        }
        else if (other.CompareTag("Escape"))
        {
            if (hasDynamite && hasFuze && hasLighter)
            {
                Debug.Log("You escaped!");
            }
            else
            {
                Debug.Log("You don't have all the items to escape.");
            }
        }
    }
}
