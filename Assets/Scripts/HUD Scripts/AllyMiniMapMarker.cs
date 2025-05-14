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
    //void Update()
    //{
    //    if (target == null || miniMapCamera == null || markerUI == null) return;

    //    Vector3 worldPos = target.position;
    //    Vector3 viewportPos = miniMapCamera.WorldToViewportPoint(worldPos);

    //    // Convert to UI position
    //    Vector2 markerPos = new Vector2(
    //        (viewportPos.x * markerUI.parent.GetComponent<RectTransform>().rect.width),
    //        (viewportPos.y * markerUI.parent.GetComponent<RectTransform>().rect.height)
    //    );

    //    markerUI.anchoredPosition = markerPos;
    //}
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

        // Keep the markers within the minimap bounds (avoid overflow)
        markerUI.anchoredPosition = new Vector2(
            Mathf.Clamp(markerPos.x, 0, markerUI.parent.GetComponent<RectTransform>().rect.width),
            Mathf.Clamp(markerPos.y, 0, markerUI.parent.GetComponent<RectTransform>().rect.height)
        );

        //if (viewportPos.z < 0)
        //{
        //    // Target is behind the camera; optionally hide marker
        //    markerUI.gameObject.SetActive(false);
        //    return;
        //}
        //else
        //{
        //    markerUI.gameObject.SetActive(true);
        //}

        //RectTransform parentRect = markerUI.parent.GetComponent<RectTransform>();

        //float x = (viewportPos.x - 0.5f) * parentRect.rect.width;
        //float y = (viewportPos.y - 0.5f) * parentRect.rect.height;

        //Vector2 clampedPos = new Vector2(
        //    Mathf.Clamp(x, -parentRect.rect.width / 2f, parentRect.rect.width / 2f),
        //    Mathf.Clamp(y, -parentRect.rect.height / 2f, parentRect.rect.height / 2f)
        //);

        //markerUI.anchoredPosition = clampedPos;
    }

}
