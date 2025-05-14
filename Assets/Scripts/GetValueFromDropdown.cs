using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class GetValueFromDropdown : MonoBehaviour
{

    [SerializeField] private TMP_Dropdown dropdown;

    public void GetmDropdownValue () {

        int pickedEntryIndex = dropdown.value;

        Debug.Log(pickedEntryIndex);


    }
    
}
