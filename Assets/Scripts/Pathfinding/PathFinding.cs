using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAG_COST = 14;

    private WorldGrid<PathNode> worldGrid;
    private List<PathNode> openList;
    private List<PathNode> closedList;


    public PathFinding(int width, int height, float cellSize, Vector3 originPosition)
    {
        worldGrid = new WorldGrid<PathNode>(width, height, cellSize, originPosition);
    }

    private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = worldGrid.GetValue(startX, startY);
        PathNode endNode = worldGrid.GetValue(endX, endY);

        openList = new List<PathNode>();
        closedList = new List<PathNode>();

        for(int x = 0; x < worldGrid.GetWidth(); x++)
        {
            for(int y = 0; y < worldGrid.GetHeight(); y++)
            {
                PathNode pathNode = worldGrid.GetValue(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.prevNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCost(openList);
            if(currentNode == endNode)
            {
                return CalculatePath(endNode);  
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode node in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(node)) continue;
                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, node);
                if(tentativeGCost < node.gCost)
                {
                    node.prevNode = currentNode;
                    node.gCost = tentativeGCost;
                    node.hCost = CalculateDistance(node, endNode);
                    node.CalculateFCost();

                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }
            }
        }
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        return null;
    }

    private int CalculateDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAG_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCost(List<PathNode> pathNodeList)
    {
        PathNode lowestFCost = pathNodeList[0];
        for(int i = 1; i <pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCost.fCost)
                lowestFCost = pathNodeList[i];
        }
        return lowestFCost;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        
        if(currentNode.x -1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.x-1, currentNode.y));
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            if (currentNode.y + 1 < worldGrid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }

        if(currentNode.x + 1 < worldGrid.GetWidth())
        {
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            if (currentNode.y + 1 < worldGrid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }

        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        if (currentNode.y + 1 < worldGrid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    public WorldGrid<PathNode> GetPathNodeGrid()
    {
        return worldGrid;
    }

    private PathNode GetNode(int x, int y)
    {
        return worldGrid.GetValue(x, y);
    }
}