using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
  // grass Tile.isPlaceable == true, other tiles.isPlaceable == false
  [SerializeField] private bool isPlaceable;
  public bool IsPlaceable => isPlaceable; // set property to get isPlaceable

  [SerializeField] private Tower towerPrefab;
  
  private void OnMouseDown()
  {
    if (isPlaceable)
    {
      var isPlaced = towerPrefab.CreateTower(towerPrefab, transform.position);
      
      isPlaceable = !isPlaced; // to avoid placing more than 1 tower to each location
    }
  }
}