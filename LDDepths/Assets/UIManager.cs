using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Sprite dynamiteUnlockedSprite;
    public Sprite fuzeUnlockedSprite;
    public Sprite lighterUnlockedSprite;
    
    public Image dynamiteImage;
    public Image fuzeImage;
    public Image lighterImage;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUnlockUI(Collectible.CollectibleType collectible)
    {
        switch (collectible)
        {
            case Collectible.CollectibleType.Dynamite: 
                dynamiteImage.sprite = dynamiteUnlockedSprite;
                Debug.Log("Collected Dynamite");
                break;
            case Collectible.CollectibleType.Fuze:
                fuzeImage.sprite = fuzeUnlockedSprite;
                Debug.Log("Collected Fuze");
                break;
            case Collectible.CollectibleType.Lighter:
                lighterImage.sprite = lighterUnlockedSprite;
                Debug.Log("Collected Lighter");
                break;
            default:
                Debug.Log("I HAVE NOTING 2");
                break;
        }
    }
}
