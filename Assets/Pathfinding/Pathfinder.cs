using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * Specifies any node it exist
 * searches for the neighbours to find the path
 */
public class Pathfinder : MonoBehaviour
{
  [SerializeField] private Vector2Int startCoordinate;
  public Vector2Int StartCoordinate => startCoordinate;

  [SerializeField] private Vector2Int destinationCoordinate;
  public Vector2Int DestinationCoordinate => destinationCoordinate;

  private Node _start, _destination;
  private Node current; // node being currently explored

  private Queue<Node> _frontier = new Queue<Node>();

  private Dictionary<Vector2Int, Node>
    _reached = new Dictionary<Vector2Int, Node>(); // lookup reference to see if a node isExplored or not 

  // search direction
  private readonly Vector2Int[] _directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down,};

  private GridManager _gridManager;

  private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();

  private void Awake()
  {
    _gridManager = FindObjectOfType<GridManager>();

    if (_gridManager != null)
    {
      _grid = _gridManager.Grid;

      // initialise the coordinates
      _start = _grid[startCoordinate];
      _destination = _grid[destinationCoordinate];
    }
  }

  void Start()
  {
    GetNewPath();
  }

  public List<Node> GetNewPath()
  {
    return GetNewPath(startCoordinate);
  }

  public List<Node> GetNewPath(Vector2Int coordinate)
  {
    _gridManager.ResetNodes(); // reset the nodes before BFS
    BreadthFirstSearch(coordinate); // find the path in start
    return BuildPath();
  }

  private void ExploreNeighbors()
  {
    var neighbors = new List<Node>();

    foreach (var direction in _directions)
    {
      // calculate the coordinates of the neighbor from our current Node
      // for all directions
      var neighborCoordinate = current.coordinate + direction;

      if (_grid.ContainsKey(neighborCoordinate))
      {
        neighbors.Add(_grid[neighborCoordinate]); // add the node to our neighbors list
      }
    }

    // neighbors we found and add them to frontier
    foreach (var neighbor in neighbors)
    {
      if (!_reached.ContainsKey(neighbor.coordinate) && neighbor.isWalkable)
      {
        neighbor.next = current; // build the tree

        _reached.Add(neighbor.coordinate, neighbor);
        _frontier.Enqueue(neighbor);
      }
    }
  }

  /**
   * backtracks the tree and build the path
   */
  private List<Node> BuildPath()
  {
    var path = new List<Node>();

    var currentNode = _destination;

    path.Add(currentNode);
    currentNode.isPath = true;

    while (currentNode.next != null)
    {
      currentNode = currentNode.next;

      path.Add(currentNode);
      currentNode.isPath = true;
    }

    path.Reverse();
    return path;
  }


  /**
  * executes the BSF
   * we will calculate BFS starting from the coordinate passed in
   * coordinate is the enemy's current position in the world
  */
  private void BreadthFirstSearch(Vector2Int coordinate)
  {
    _start.isWalkable = true;
    _destination.isWalkable = true;

    // reset when calling pathfinding again
    _frontier.Clear();
    _reached.Clear();

    var isRunning = true;

    _frontier.Enqueue(_grid[coordinate]); // add the start node
    _reached.Add(coordinate, _grid[coordinate]); // add the start node to reached

    while (_frontier.Count > 0 && isRunning)
    {
      current = _frontier.Dequeue(); // make the next one in the queue as current node
      current.isExplored = true;
      ExploreNeighbors();

      if (current.coordinate == destinationCoordinate)
      {
        // break out of loop if the current node is the destination
        isRunning = false;
      }
    }
  }

  /**
   * make an adjustment to the node at teh coordinate
   * is BFS can fin d a path we return false
   * else return true
   */
  public bool WillBlockPath(Vector2Int coordinate)
  {
    if (_grid.ContainsKey(coordinate))
    {
      var previousState = _grid[coordinate].isWalkable;

      _grid[coordinate].isWalkable = false;
      var newPath = GetNewPath();
      _grid[coordinate].isWalkable = previousState;

      if (newPath.Count <= 1)
      {
        // path is blocked
        GetNewPath();
        return true;
      }
    }

    return false;
  }

  /**
   * sending out the broadcast message
   */
  public void NotifyReceivers()
  {
    BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
  }
}