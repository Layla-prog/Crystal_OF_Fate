using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    
    public Item Item;

    void Pickup() 
    {
        InventoryManger.Instance.Add(Item);
        Destroy(gameObject);


    }

    private void OnMouseDown()
    {
        
        Pickup();
    }


}
