using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
  // grass Tile.isPlaceable == true, other tiles.isPlaceable == false
  [SerializeField] private bool isPlaceable;
  public bool IsPlaceable => isPlaceable; // set property to get isPlaceable

  [SerializeField] private Tower towerPrefab;

  private GridManager _gridManager;
  private Pathfinder _pathfinder;
  private Vector2Int coordinate = new Vector2Int();

  private void Awake()
  {
    _gridManager = FindObjectOfType<GridManager>();
    _pathfinder = FindObjectOfType<Pathfinder>();
  }

  private void Start()
  {
    if (_gridManager != null)
    {
      coordinate = _gridManager.GetCoordinatesFromPosition(transform.position);

      if (!isPlaceable) // if not isPlaceable we will set its isWalkable = false
      {
        _gridManager.BlockNode(coordinate);
      }
    }
  }

  private void OnMouseDown()
  {
    if (_gridManager.GetNode(coordinate).isWalkable && !_pathfinder.WillBlockPath(coordinate))
    {
      var isSuccessfullyPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);

      if (isSuccessfullyPlaced)
      {
        _gridManager.BlockNode(coordinate);
        _pathfinder.NotifyReceivers(); // so the enemies will calculate the path again
      }
    }
  }
}