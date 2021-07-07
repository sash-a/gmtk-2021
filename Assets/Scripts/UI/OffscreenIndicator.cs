using System;
using UnityEngine;

public class OffscreenIndicator : MonoBehaviour
{
    private Camera _cam;
    private Transform _agentTransform;

    [NonSerialized] public SpriteRenderer SpriteRenderer;

    public float margin = 0.95f;
    public float maxDist = 30;

    void Start()
    {
        _cam = Camera.main;
        _agentTransform = transform.parent;
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // TODO move this onto the indicator!
    void Update()
    {
        var screenPos = _cam.WorldToScreenPoint(_agentTransform.position);
        if (screenPos.x > 0 && screenPos.y > 0 && screenPos.x < Screen.width && screenPos.y < Screen.height)
        {
            SpriteRenderer.enabled = false;
            return; // ignore if onscreen
        }

        // offscreen
        SpriteRenderer.enabled = true;

        var center = new Vector3(Screen.width, Screen.height, 0) / 2;
        var bounds = center * margin;
        screenPos -= center; // moving 0,0 from bottom left to screen center
        var angle = Mathf.Atan2(screenPos.y, screenPos.x);

        var intersectionPoint = findBoxIntersection(screenPos, bounds) + (Vector2) center;
        var intersectionPointWorld = _cam.ScreenToWorldPoint(intersectionPoint);
        intersectionPointWorld.z = 0;

        Debug.DrawLine(Player.instance.transform.position, intersectionPointWorld, Color.red);
        transform.position = intersectionPointWorld;
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);

        // setting alpha based on dist
        Color oldColour = SpriteRenderer.color;
        float distToPlayer = Vector2.Distance(Player.instance.transform.position, _agentTransform.position);
        oldColour.a = distToPlayer > maxDist ? 0 : 1 - distToPlayer / maxDist;
        SpriteRenderer.color = oldColour;
    }

    /*
     * Finds where the line from origin to screenPos intersects bounds, assuming screen pos is not inside box bounds
     */
    Vector2 findBoxIntersection(Vector3 screenPos, Vector3 bounds)
    {
        var m = screenPos.y / screenPos.x; // grad

        // know it's out of bounds so check quadrant to see which 2 screen bounds it's intersecting
        var xCoeff = screenPos.x > bounds.x ? 1 : -1; // 1: right of player | -1: left of player
        var y = m * xCoeff * bounds.x;
        if (y < bounds.y && y > -bounds.y) // y within bounds so line isn't intersecting y
        {
            return new Vector2(xCoeff * bounds.x, y);
        }

        var yCoeff = screenPos.y > bounds.y ? 1 : -1; // 1: upper screen bound | -1: lower screen bound
        // already know line is out of bounds for x val, so must be in bounds here
        return new Vector2(yCoeff * bounds.y / m, yCoeff * bounds.y);
    }
}