using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GridManager : MonoBehaviour
{
  [SerializeField] private Vector2Int gridSize;

  [Tooltip("World Grid Size from Unity Editor Snap Settings")] [SerializeField]
  private int unityGridSize = 10; // our current size

  public int UnityGridSize => unityGridSize;

  private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();
  public Dictionary<Vector2Int, Node> Grid => _grid;

  private void Awake()
  {
    CreateGrid();
  }

  /**
   * blocks the node
   * set the nodes isWalkable flag to false
   */
  public void BlockNode(Vector2Int coordinate)
  {
    if (_grid.ContainsKey(coordinate))
    {
      _grid[coordinate].isWalkable = false;
    }
  }

  /**
   * loop through all the nodes in our grid and resets it
   */
  public void ResetNodes()
  {
    foreach (var entry in _grid)
    {
      entry.Value.next = null;
      entry.Value.isExplored = false;
      entry.Value.isPath = false;
    }
  }

  public Vector2Int GetCoordinatesFromPosition(Vector3 position)
  {
    var coordinate = new Vector2Int
    {
      x = Mathf.RoundToInt(position.x / unityGridSize),
      y = Mathf.RoundToInt(position.z / unityGridSize)
    };


    return coordinate;
  }

  public Vector3 GetPositionFromCoordinates(Vector2Int coordinate)
  {
    var position = new Vector3
    {
      x = coordinate.x * unityGridSize,
      z = coordinate.y * unityGridSize
    };


    return position;
  }


  /**
   * returns the node at a coordinate
   */
  public Node GetNode(Vector2Int coordinate)
  {
    return !_grid.ContainsKey(coordinate) ? null : _grid[coordinate];
  }

  private void CreateGrid()
  {
    for (int i = -20; i < gridSize.x; i++) // i is the each x coordinate
    {
      for (int j = -20; j < gridSize.y; j++) // j is the each y coordinate
      {
        var coordinate = new Vector2Int(i, j);
        _grid.Add(coordinate, new Node(coordinate, true));
      }
    }
  }
}