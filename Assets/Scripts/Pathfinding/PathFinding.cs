using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    #region Constants
    private const int MOVE_STRAIGHT_COST = 10;  //The cost of moving to a PathNode directly adjacent to current node
    private const int MOVE_DIAG_COST = 14;      //The cost of moving to a PathNode diagonally adjacent to current node
    #endregion

    #region Properties
    private WorldGrid<PathNode> worldGrid;  //Reference to the WorldGrid used to find a path
    private List<PathNode> openList;        //List used to store PathNodes not yet investiagted
    private List<PathNode> closedList;      //List used to store PathNodes already investiagted
    #endregion

    //Constructors
    public PathFinding(int width, int height, float cellSize, Vector3 originPosition)
    {
        //Instantiate a new WorldGrid to use as reference
        worldGrid = new WorldGrid<PathNode>(width, height, cellSize, originPosition);
        //Populate the new WorldGrid with PathNodes
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                worldGrid.SetValue(x, y, new PathNode(worldGrid, x, y));
            }
        }
    }

    #region Path-finding Functions
    //Used to find the path between a start position and end position given a set of coordinates
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        //Initialise our start and end nodes as well as reset our open and closed lists
        PathNode startNode = worldGrid.GetValue(startX, startY);
        PathNode endNode = worldGrid.GetValue(endX, endY);
        openList = new List<PathNode>() { startNode };
        closedList = new List<PathNode>();

        //Initialise every PathNode in our WorldGrid
        for(int x = 0; x < worldGrid.GetWidth(); x++)
        {
            for(int y = 0; y < worldGrid.GetHeight(); y++)
            {
                PathNode pathNode = worldGrid.GetValue(x, y);
                pathNode.gCost = int.MaxValue;                  //Max out the G Cost
                pathNode.CalculateFCost();                      //Calculate a max F Cost
                pathNode.prevNode = null;                       //Remove any references to previous Nodes
            }
        }

        //Start our search on the start node
        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        //Begin the search for our wanted node
        while (openList.Count > 0)
        {
            //Find the node with the lowest F Cost, then start searching from there
            PathNode currentNode = GetLowestFCost(openList);
            
            //End the search if that is our end node
            if(currentNode == endNode)
            {
                return CalculatePath(endNode);  
            }

            //Mark the current node as searched
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //Check the neighbouring nodes relative to current node
            foreach (PathNode node in GetNeighbourList(currentNode))
            {
                //Move onto next node if this node has been searched already
                if (closedList.Contains(node)) continue;
                
                //Calculate a tentative G Cost based on our current node's G Cost
                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, node);

                //If this cost is lower than the G Cost of the neighbouring node, move onto the neighbouring node
                if(tentativeGCost < node.gCost)
                {
                    node.prevNode = currentNode;
                    node.gCost = tentativeGCost;
                    node.hCost = CalculateDistance(node, endNode);
                    node.CalculateFCost();

                    //Add the neighbouring node to the open list if it hasn't already been added
                    if (!openList.Contains(node))
                    {
                        openList.Add(node);
                    }
                }
            }
        }
        return null;
    }

    //Returns a path as a list of Path Nodes based on an end node given
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        //Create an empty list and add the end node to it
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        
        //Iterate backwards through the previous nodes until we get to the start node
        while(currentNode.prevNode != null)
        {
            path.Add(currentNode.prevNode);
            currentNode = currentNode.prevNode;
        }

        //Reverse the list then return it
        path.Reverse();
        return path;
    }
    #endregion

    #region Helper Functions
    //Returns an int that is the result of the distance calculated between two points
    private int CalculateDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAG_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    //Returns a PathNode that contains the lowest FCost given a list of Path Nodes
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

    //Returns a list of Path Nodes neighbouring a given Path Node
    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        
        if(currentNode.x -1 >= 0)
        {
            //Left node
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            //Left-down node
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            //Left-up node
            if (currentNode.y + 1 < worldGrid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }

        if(currentNode.x + 1 < worldGrid.GetWidth())
        {
            //Right node
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            //Right-down node
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            //Right-up node
            if (currentNode.y + 1 < worldGrid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }

        //Up node
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        //Down node
        if (currentNode.y + 1 < worldGrid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }
    #endregion

    #region Getters
    //Retruns the referenced World Grid
    public WorldGrid<PathNode> GetPathNodeGrid()
    {
        return worldGrid;
    }

    //Returns a PathNode on the World Grid given an x and y coordinate
    private PathNode GetNode(int x, int y)
    {
        return worldGrid.GetValue(x, y);
    }
    #endregion
}