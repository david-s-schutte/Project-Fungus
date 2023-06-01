using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private WorldGrid<PathNode> worldGrid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode prevNode;

    public PathNode(WorldGrid<PathNode> worldGrid, int x, int y)
    {
        this.worldGrid = worldGrid;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "y, " + y;
    }
}
