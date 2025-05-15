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

    [Header("Exit Arrow")]
    public RectTransform exitArrowSmall; // Arrow for small minimap
    public RectTransform exitArrowFull;  // Arrow for fullscreen minimap
    public float edgeBuffer = 10f;

    [Header("Ally Route Lines (Optional)")]
    public UILineRenderer[] allyLinesSmall;
    public UILineRenderer[] allyLinesFull;

    [Header("Update Settings")]
    public float updateRate = 0.5f;

    private List<Transform> allies = new List<Transform>();
    private float timer;

    [Header("Smoothing")]
    public float smoothingSpeed = 5f;

    private float targetAngleSmall, targetAngleFull;
    private Vector2 targetPosSmall, targetPosFull;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            ComputeArrowTarget(exitArrowSmall, minimapSmallRect, out targetAngleSmall, out targetPosSmall);
            ComputeArrowTarget(exitArrowFull, minimapFullRect, out targetAngleFull, out targetPosFull);
            UpdateAllyRoutes();
            timer = 0f;
        }

        // move each arrow smoothly every frame
        SmoothMoveArrow(exitArrowSmall, targetAngleSmall, targetPosSmall);
        SmoothMoveArrow(exitArrowFull, targetAngleFull, targetPosFull);
    }

    void ComputeArrowTarget(RectTransform arrow, RectTransform minimapRect, out float angleDeg, out Vector2 localPos)
    {
        // same code until angle and localPos
        Vector3 dirToExit = exitPoint.position - player.position;
        dirToExit.y = 0;
        float rawAngle = Mathf.Atan2(dirToExit.x, dirToExit.z) * Mathf.Rad2Deg;
        angleDeg = -rawAngle;

        Vector3 projected = player.position + dirToExit.normalized * 5f;
        Vector3 screenPt = minimapCamera.WorldToScreenPoint(projected);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            minimapRect, screenPt, minimapCamera, out localPos
        );
        float maxR = Mathf.Min(minimapRect.rect.width, minimapRect.rect.height) / 2 - edgeBuffer;
        localPos = Vector2.ClampMagnitude(localPos, maxR);
    }

    void SmoothMoveArrow(RectTransform arrow, float targetAngle, Vector2 targetPos)
    {
        // lerp rotation
        float currentZ = arrow.localEulerAngles.z;
        float smoothedZ = Mathf.LerpAngle(currentZ, targetAngle, Time.deltaTime * smoothingSpeed);
        arrow.localEulerAngles = new Vector3(0, 0, smoothedZ);

        // lerp position
        arrow.localPosition = Vector2.Lerp(arrow.localPosition, targetPos, Time.deltaTime * smoothingSpeed);
    }

    void Start()
    {
        // Find all allies in scene
        GameObject[] allyObjects = GameObject.FindGameObjectsWithTag("Ally");
        foreach (GameObject obj in allyObjects)
            allies.Add(obj.transform);
    }

    //void Update()
    //{
    //    timer += Time.deltaTime;
    //    if (timer >= updateRate)
    //    {
    //        UpdateExitArrow(exitArrowSmall, minimapSmallRect);
    //        UpdateExitArrow(exitArrowFull, minimapFullRect);
    //        UpdateAllyRoutes(); // Optional
    //        timer = 0f;
    //    }
    //}

    void UpdateExitArrow(RectTransform arrow, RectTransform minimapRect)
    {
        if (player == null || exitPoint == null || arrow == null || minimapCamera == null) return;

        Vector3 dirToExit = exitPoint.position - player.position;
        dirToExit.y = 0f;
        Vector3 forwardDir = player.forward;
        forwardDir.y = 0f;

        // Rotate arrow to point toward exit
        float angle = Mathf.Atan2(dirToExit.x, dirToExit.z) * Mathf.Rad2Deg;
        arrow.localEulerAngles = new Vector3(0, 0, -angle);

        // Offset from player to arrow direction (in front)
        Vector3 projectedPoint = player.position + dirToExit.normalized * 5f;
        Vector3 screenPos = minimapCamera.WorldToScreenPoint(projectedPoint);

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            minimapRect, screenPos, minimapCamera, out localPos
        );

        // Clamp position within minimap bounds
        float maxRadius = Mathf.Min(minimapRect.rect.width, minimapRect.rect.height) / 2f - edgeBuffer;
        localPos = Vector2.ClampMagnitude(localPos, maxRadius);

        arrow.localPosition = localPos;
    }

    void UpdateAllyRoutes()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(player.position, allies[i].position, NavMesh.AllAreas, path))
            {
                if (i < allyLinesSmall.Length)
                    DrawRoute(path, allyLinesSmall[i], minimapSmallRect);
                if (i < allyLinesFull.Length)
                    DrawRoute(path, allyLinesFull[i], minimapFullRect);
            }
        }
    }

    void DrawRoute(NavMeshPath path, UILineRenderer lineRenderer, RectTransform minimapRect)
    {
        if (path == null || path.corners.Length < 2 || lineRenderer == null) return;

        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 screenPos = minimapCamera.WorldToScreenPoint(path.corners[i]);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                minimapRect, screenPos, minimapCamera, out Vector2 localPoint))
            {
                points.Add(localPoint);
            }
        }

        lineRenderer.Points = points.ToArray();
        lineRenderer.SetAllDirty();
    }

   /* void UpdateExitRoutes()
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
   */


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

    /*void DrawRoute(NavMeshPath path, UILineRenderer lineRenderer, RectTransform minimapRect)
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
    }*/


}

