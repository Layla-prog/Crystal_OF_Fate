using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour
{
  
  public static InventoryManger Instance;
  public List<Item> Items = new List<Item>();

  public Transform ItemContent;
  public GameObject InventoryItem;

    public Toggle EnableRemove;

    public InventoryItemControler[] InventoryItems;

    private void Awake()
    {
         Instance = this;
    }

    public void Add(Item item) 
    {
        Items.Add(item);

    }


    public void Remove(Item item) 
    {
        Items.Remove(item);

    }


    public void ListItems () 
    {

        //clean content before open

        foreach (Transform item in ItemContent)
        {

            Destroy(item.gameObject);
        }

        foreach (var item in Items) 
        {
            GameObject obj = Instantiate(InventoryItem , ItemContent);
            var ItemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var ItemIcon = obj.transform.Find("Image").GetComponent<Image>();
            
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

            ItemName.GetComponentInChildren<Text>().text = item.ItemName;
            ItemIcon.sprite = item.Image;

            Debug.LogError("itemname" +item.ItemName);
            
            if(EnableRemove.isOn)
            removeButton.gameObject.SetActive(true);
        }

    }

    public void EnableItemsRemove () 
    
    {
        if(EnableRemove.isOn){

            foreach (Transform item in ItemContent)
            
            {

                item.Find("RemoveButton").gameObject.SetActive(true);

            }
        }else
            {
                 foreach (Transform item in ItemContent)
            
            {

                item.Find("RemoveButton").gameObject.SetActive(false);

            }
            }
        }


        public void SetInventoryItems() 
        {
            InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemControler>();
        }


    }
