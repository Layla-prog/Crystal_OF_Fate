using UnityEngine;
using UnityEngine.UI;

public class InventoryItemControler : MonoBehaviour
{

    Item item;

    public Button RemoveButton;

    public void RemoveItem ()
    {

        InventoryManger.Instance.Remove(item);
        Destroy(gameObject);
    }


    public void AddItem(Item newItem) 
    {
        item = newItem;

    }

}
