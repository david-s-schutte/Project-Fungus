using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public WorldGrid<int> world;
    private PathFinding pathFinding;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float cellSize;
    [SerializeField] Vector3 origin;
    public bool debug;
    Vector3 mousePosition;

    public GameObject unwalkableVisual;

    void Awake()
    {
        //world = new WorldGrid<int>(width, height, cellSize, origin);
        //if (debug)
        //    world.DebugGrid();
        pathFinding = new PathFinding(width, height, cellSize, origin);
        if (debug)
            pathFinding.GetPathNodeGrid().DebugGrid();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (debug)
                DebugPathFinding();
        }

        if (Input.GetMouseButtonDown(1))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            pathFinding.GetPathNodeGrid().GetXY(mousePosition, out int x, out int y);
            pathFinding.GetNode(x, y).SetIsWalkable();
            GameObject debugCellClone = GameObject.Instantiate(unwalkableVisual, pathFinding.GetPathNodeGrid().GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, Quaternion.identity);
        }
    }

    private void DebugPathFinding()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        pathFinding.GetPathNodeGrid().GetXY(mousePosition, out int x, out int y);
        List<PathNode> path = pathFinding.FindPath(0, 0, x, y);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(pathFinding.GetPathNodeGrid().GetWorldPosition(path[i].x, path[i].y) + new Vector3(cellSize, cellSize) * 0.5f,
                    pathFinding.GetPathNodeGrid().GetWorldPosition(path[i + 1].x, path[i + 1].y) + new Vector3(cellSize, cellSize) * 0.5f,
                    Color.green, 100f);
                Debug.Log("drawing a line from " + (new Vector3(path[i].x, path[i].y) + " to " + (new Vector3(path[i + 1].x, path[i + 1].y))));
            }
        }
        else
        {
            Debug.LogError("ERROR: path is null");
        }
    }
}
