using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject exitMarkerPrefab;
    public Transform exitPoint;

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
                // small minimap marker
                GameObject markerSmall = Instantiate(markerPrefab, markerParentSmall);
                AllyMiniMapMarker trackerSmall = markerSmall.AddComponent<AllyMiniMapMarker>();
                trackerSmall.target = ally.transform;
                trackerSmall.markerUI = markerSmall.GetComponent<RectTransform>();
                trackerSmall.miniMapCamera = miniMapCamera;

                // marker colors for small minimap
                Image imageSmall = markerSmall.GetComponent<Image>();
                if (imageSmall != null)
                {
                    switch (npcComponent.npcType)
                    {
                        case NPCType.Healer:
                            imageSmall.color = Color.red;
                            break;
                        case NPCType.Archer:
                            imageSmall.color = Color.green;
                            break;
                        case NPCType.PotionCrafter:
                            imageSmall.color = Color.blue;
                            break;
                        case NPCType.Craftsman:
                            imageSmall.color = Color.gray;
                            break;
                    }
                }

                // full minimap marker
                GameObject markerFull = Instantiate(markerPrefab, markerParentFull);
                AllyMiniMapMarker trackerFull = markerFull.AddComponent<AllyMiniMapMarker>();
                trackerFull.target = ally.transform;
                trackerFull.markerUI = markerFull.GetComponent<RectTransform>();
                trackerFull.miniMapCamera = miniMapCamera;

                // marker colors for small minimap
                Image imageFull = markerSmall.GetComponent<Image>();
                if (imageFull != null)
                {
                    switch (npcComponent.npcType)
                    {
                        case NPCType.Healer:
                            imageSmall.color = Color.red;
                            break;
                        case NPCType.Archer:
                            imageSmall.color = Color.green;
                            break;
                        case NPCType.PotionCrafter:
                            imageSmall.color = Color.blue;
                            break;
                        case NPCType.Craftsman:
                            imageSmall.color = Color.gray;
                            break;
                    }
                }

                // Small minimap exit marker
                GameObject exitMarkerSmall = Instantiate(exitMarkerPrefab, markerParentSmall);
                AllyMiniMapMarker exitTrackerSmall = exitMarkerSmall.AddComponent<AllyMiniMapMarker>();
                exitTrackerSmall.target = exitPoint;
                exitTrackerSmall.markerUI = exitMarkerSmall.GetComponent<RectTransform>();
                exitTrackerSmall.miniMapCamera = miniMapCamera;

                // Full minimap exit marker
                GameObject exitMarkerFull = Instantiate(exitMarkerPrefab, markerParentFull);
                AllyMiniMapMarker exitTrackerFull = exitMarkerFull.AddComponent<AllyMiniMapMarker>();
                exitTrackerFull.target = exitPoint;
                exitTrackerFull.markerUI = exitMarkerFull.GetComponent<RectTransform>();
                exitTrackerFull.miniMapCamera = miniMapCamera;

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
