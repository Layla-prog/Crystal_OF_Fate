using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapAllyManager : MonoBehaviour
{
    public Camera miniMapCamera;
    public RectTransform markerParent;

    public GameObject craftsmanMarkerPrefab;
    public GameObject healerMarkerPrefab;
    public GameObject archerMarkerPrefab;
    public GameObject potionCrafterMarkerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");

        foreach (GameObject ally in allies)
        {
            NPC npcComponent = ally.GetComponent<NPC>();
            if (npcComponent == null) continue;

            GameObject markerPrefab = GetMarkerPrefabByType(npcComponent.npcType);
            if (markerPrefab != null)
            {
                GameObject marker = Instantiate(markerPrefab, markerParent);
                AllyMiniMapMarker tracker = marker.AddComponent<AllyMiniMapMarker>();
                tracker.target = ally.transform;
                tracker.markerUI = marker.GetComponent<RectTransform>();
                tracker.miniMapCamera = miniMapCamera;
            }
        }
    }

    GameObject GetMarkerPrefabByType(NPCType type)
    {
        switch (type)
        {
            case NPCType.Craftsman: return craftsmanMarkerPrefab;
            case NPCType.Archer: return archerMarkerPrefab;
            case NPCType.PotionCrafter: return potionCrafterMarkerPrefab;
            case NPCType.Healer: return healerMarkerPrefab;
            default: return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
