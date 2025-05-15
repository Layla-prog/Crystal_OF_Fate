using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class MiniMapRouteDrawer : MonoBehaviour
{
    [Header("Core References")]
    public Transform player;
    public Transform exitPoint;
    public Camera minimapCamera;

    [Header("Minimap Panels")]
    public RectTransform minimapSmallRect;
    public RectTransform minimapFullRect;

    [Header("Line Renderers")]
    public UILineRenderer routeToExitSmall;
    public UILineRenderer routeToExitFull;

    public UILineRenderer[] allyLinesSmall; // Assign one per ally
    public UILineRenderer[] allyLinesFull;

    [Header("Update Settings")]
    public float updateRate = 0.5f;

    private List<Transform> allies = new List<Transform>();
    private float timer;

    void Start()
    {
        // Find all allies in scene
        GameObject[] allyObjects = GameObject.FindGameObjectsWithTag("Ally");
        foreach (GameObject obj in allyObjects)
            allies.Add(obj.transform);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            UpdateExitRoutes();
            UpdateAllyRoutes();
            timer = 0f;
        }
    }

    void UpdateExitRoutes()
    {
        NavMeshPath pathToExit = new NavMeshPath();
        if (NavMesh.CalculatePath(player.position, exitPoint.position, NavMesh.AllAreas, pathToExit))
        {
            DrawRoute(pathToExit, routeToExitSmall, minimapSmallRect);
            DrawRoute(pathToExit, routeToExitFull, minimapFullRect);
        }
    }

    void UpdateAllyRoutes()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            NavMeshPath pathToAlly = new NavMeshPath();
            if (NavMesh.CalculatePath(player.position, allies[i].position, NavMesh.AllAreas, pathToAlly))
            {
                if (i < allyLinesSmall.Length)
                    DrawRoute(pathToAlly, allyLinesSmall[i], minimapSmallRect);
                if (i < allyLinesFull.Length)
                    DrawRoute(pathToAlly, allyLinesFull[i], minimapFullRect);
            }
        }
    }



    /*void UpdateExitRoutes()
    {
        DrawRoute(player.position, exitPoint.position, routeToExitSmall, minimapSmallRect);
        DrawRoute(player.position, exitPoint.position, routeToExitFull, minimapFullRect);
    }

    void UpdateAllyRoutes()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (i < allyLinesSmall.Length)
                DrawRoute(player.position, allies[i].position, allyLinesSmall[i], minimapSmallRect);
            if (i < allyLinesFull.Length)
                DrawRoute(player.position, allies[i].position, allyLinesFull[i], minimapFullRect);
        }
    }*/

    /*void DrawRoute(Vector3 start, Vector3 end, UILineRenderer lineRenderer, RectTransform minimapRect)
    {
        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path) || path.corners.Length < 2)
            return;

        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 worldPoint = path.corners[i];

            // Project world point to minimap camera's screen
            Vector3 screenPos = minimapCamera.WorldToScreenPoint(worldPoint);

            // Convert screen to local UI space
            RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRect, screenPos, null, out Vector2 localPos);

            points.Add(localPos);
        }

        lineRenderer.Points = points.ToArray();
        lineRenderer.SetAllDirty(); // Force refresh
    }*/

    void DrawRoute(NavMeshPath path, UILineRenderer lineRenderer, RectTransform minimapRect)
    {
        if (path == null || path.corners.Length < 2) return;

        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 worldPoint = path.corners[i];
            Vector3 screenPos = minimapCamera.WorldToScreenPoint(worldPoint);

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                minimapRect,
                screenPos,
                minimapCamera,
                out localPoint
            );

            points.Add(localPoint);
        }

        lineRenderer.Points = points.ToArray();
        lineRenderer.SetAllDirty(); // Refresh the line
    }


}

