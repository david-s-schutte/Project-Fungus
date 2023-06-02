using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    #region Properties
    private WorldGrid<PathNode> worldGrid;  //Reference to the grid each PathNode lies on
    public int x;                           //PathNode's x position
    public int y;                           //PathNode's y position

    public int gCost;                       //Expected G Cost of this PathNode
    public int hCost;                       //Expected H Cost of this PathNode
    public int fCost;                       //Expected F Cost of this PathNode

    public bool isWalkable;
    public PathNode prevNode;               //Reference to the previous PathNode when finding a path
    #endregion

    //Constructor
    public PathNode(WorldGrid<PathNode> worldGrid, int x, int y)
    {
        this.worldGrid = worldGrid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }

    //Used to calculate the FCost of this PathNode
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable()
    {
        isWalkable = !isWalkable;
    }

    public override string ToString()
    {
        return worldGrid +  ", "+ x + "," + y;
    }
}
