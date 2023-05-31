using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public WorldGrid<bool> world;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float cellSize;
    [SerializeField] Vector3 origin;
    public bool debug;

    void Awake()
    {
        world = new WorldGrid<bool>(width, height, cellSize, origin);
        if (debug)
            world.DebugGrid();
    }
}
