using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapAllyManager : MonoBehaviour
{
    public Camera miniMapCamera;
    public RectTransform markerParentSmall;
    public RectTransform markerParentFull;

    public GameObject playerMarkerPrefab;
    public GameObject craftsmanMarkerPrefab;
    public GameObject healerMarkerPrefab;
    public GameObject archerMarkerPrefab;
    public GameObject potionCrafterMarkerPrefab;

    private GameObject playerMarker;

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
                // Instantiate marker for small minimap
                GameObject markerSmall = Instantiate(markerPrefab, markerParentSmall);
                AllyMiniMapMarker trackerSmall = markerSmall.AddComponent<AllyMiniMapMarker>();
                trackerSmall.target = ally.transform;
                trackerSmall.markerUI = markerSmall.GetComponent<RectTransform>();
                trackerSmall.miniMapCamera = miniMapCamera;

                // Instantiate marker for full minimap
                GameObject markerFull = Instantiate(markerPrefab, markerParentFull);
                AllyMiniMapMarker trackerFull = markerFull.AddComponent<AllyMiniMapMarker>();
                trackerFull.target = ally.transform;
                trackerFull.markerUI = markerFull.GetComponent<RectTransform>();
                trackerFull.miniMapCamera = miniMapCamera;
            }
        }
        CreatePlayerMarker();
    }

    void CreatePlayerMarker()
    {
        // Instantiate the player marker for small minimap
        playerMarker = Instantiate(playerMarkerPrefab, markerParentSmall);
        PlayerMiniMapMarker playerTrackerSmall = playerMarker.AddComponent<PlayerMiniMapMarker>();
        playerTrackerSmall.target = GameObject.FindGameObjectWithTag("Player").transform;
        playerTrackerSmall.markerUI = playerMarker.GetComponent<RectTransform>();
        playerTrackerSmall.miniMapCamera = miniMapCamera;

        // Instantiate the player marker for full minimap
        GameObject playerMarkerFull = Instantiate(playerMarkerPrefab, markerParentFull);
        PlayerMiniMapMarker playerTrackerFull = playerMarkerFull.AddComponent<PlayerMiniMapMarker>();
        playerTrackerFull.target = GameObject.FindGameObjectWithTag("Player").transform;
        playerTrackerFull.markerUI = playerMarkerFull.GetComponent<RectTransform>();
        playerTrackerFull.miniMapCamera = miniMapCamera;
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
}
