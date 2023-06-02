using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//CODE BASED OFF OF THE CODE-MONKEY YOUTUBE TUTORIAL ON 2D GRIDS

public class WorldGrid<TGridObject>
{
    #region Grid Properties
    private int width;                  //width of the grid
    private int height;                 //height of the grid
    private float cellSize;             //size of each cell
    private Vector3 originPosition;     //origin position of the grid
    private TGridObject[,] gridArray;   //2D array to store the value of each cell
    #endregion

    #region Debug variables
    public bool debug = false;          //Set to true if need to draw grid - false by default
    #endregion

    //Constructor
    public WorldGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        //Initialise all properties
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridArray = new TGridObject[width, height];

        if(debug)
            DebugGrid();
    }

    #region Getters
    //Return the world position of a cell as a Vector3
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    //Return the relevant coordinates within the grid given a world position
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    //XY METHOD: Get the value of a given coordinate within the Grid Array
    public TGridObject GetValue(int x, int y)
    {
        //Ensure that the given coordinates are valid, otherwise return -1
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else { return default(TGridObject); }
    }

    //Vector3 METHOD: Get the value of a world position within the Grid Array
    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
    #endregion

    #region Setters
    //XY METHOD: Sets the value of the given coordinate within the Grid Array
    public void SetValue(int x, int y, TGridObject value)
    {
        //Ensure we've been given a valid coordinate first
        if (x >= 0 && y >= 0 && x < width && y < height)
            gridArray[x, y] = value;
    }

    //Vector3 METHOD: Sets the value of the given world position within the Grid Array
    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }
    #endregion

    #region Debug Methods
    public void DebugGrid()
    {
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("DebugRubbish");
        foreach (GameObject clone in tmp)
            GameObject.Destroy(clone);

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                GameObject debugCellMaster = GameObject.FindWithTag("EditorOnly").GetComponent<DebugObjects>().debugCell;
                if (debugCellMaster)
                {
                    GameObject debugCellClone = GameObject.Instantiate(debugCellMaster, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, Quaternion.identity);
                    debugCellClone.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + GetValue(x, y);
                }
                else
                    Debug.LogWarning("Error: debugCell prefab not found");
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.red, 100f);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.red, 100f);
                //Debug.Log("Value at: " + x + ", " + y + " is: " + gridArray[x, y]);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.red, 100f);
    }
    #endregion
}