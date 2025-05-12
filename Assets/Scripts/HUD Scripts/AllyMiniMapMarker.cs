using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMiniMapMarker : MonoBehaviour
{
    public Transform target; // The NPC to follow
    public RectTransform markerUI; // The UI icon on the minimap
    public Camera miniMapCamera; // The camera rendering the minimap

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null || miniMapCamera == null || markerUI == null) return;

        Vector3 worldPos = target.position;
        Vector3 viewportPos = miniMapCamera.WorldToViewportPoint(worldPos);

        // Convert to UI position
        Vector2 markerPos = new Vector2(
            (viewportPos.x * markerUI.parent.GetComponent<RectTransform>().rect.width),
            (viewportPos.y * markerUI.parent.GetComponent<RectTransform>().rect.height)
        );

        markerUI.anchoredPosition = markerPos;
    }
}
