using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiniMapMarker : MonoBehaviour
{
    public Transform target; 
    public RectTransform markerUI; 
    public Camera miniMapCamera;

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

        // Adjust the marker position for both small and full minimap views
        Vector2 markerPos = new Vector2(
            (viewportPos.x * markerUI.parent.GetComponent<RectTransform>().rect.width),
            (viewportPos.y * markerUI.parent.GetComponent<RectTransform>().rect.height)
        );

        // Keep the marker within the minimap bounds (avoid overflow)
        markerUI.anchoredPosition = new Vector2(
            Mathf.Clamp(markerPos.x, 0, markerUI.parent.GetComponent<RectTransform>().rect.width),
            Mathf.Clamp(markerPos.y, 0, markerUI.parent.GetComponent<RectTransform>().rect.height)
        );
    }
}
