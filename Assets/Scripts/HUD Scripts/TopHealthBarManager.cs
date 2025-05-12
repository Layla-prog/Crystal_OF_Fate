using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class TopHealthBarManager : MonoBehaviour
{
    public static TopHealthBarManager Instance;

    public GameObject barPrefab;
    public Transform container;

    private Dictionary<string, TopHealthBarUI> bars = new();

    void Awake()
    {
        Instance = this;
    }

    public void ShowHealthBar(string characterName, float percent)
    {
        if (!bars.ContainsKey(characterName))
        {
            GameObject go = Instantiate(barPrefab, container);
            TopHealthBarUI ui = go.GetComponent<TopHealthBarUI>();
            bars.Add(characterName, ui);
        }

        bars[characterName].UpdateHealth(characterName, percent);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
