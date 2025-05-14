using System.IO;
using System.IO.Enumeration;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item" ,menuName = "Item/Create New Item")]
public class Item : ScriptableObject 
{

    public int id;
    public string ItemName;
    public int Value;
    public Sprite Image;

}
