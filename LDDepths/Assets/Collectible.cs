using UnityEngine;

public class Collectible : MonoBehaviour
{
    
    public CollectibleType collectibleType;
    
    
    
    public enum CollectibleType
    {
        Dynamite,
        Fuze,
        Lighter,
        None
    }

    public CollectibleType GetCollectibleType()
    {
        return collectibleType;
    }

    public void CollectibleCollect()
    {
        Destroy(gameObject);
    }
}
