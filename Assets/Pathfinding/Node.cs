using System;
using UnityEngine;

namespace Pathfinding
{
  [Serializable]
  public class Node
  {
    public Vector2Int coordinate;
    public bool isWalkable; // whether to be added to a tree or not, so it wont be searchable
    public bool isExplored;
    public bool isPath;
    public Node next;

    public Node(Vector2Int coordinate, bool isWalkable)
    {
      this.coordinate = coordinate;
      this.isWalkable = isWalkable;
    }
  }
}