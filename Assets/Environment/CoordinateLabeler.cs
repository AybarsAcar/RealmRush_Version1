using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
 * Labels the Parent Game Object and the Text of the TextMeshPro
 * to the position of the object in our world coordinate system
 * Displays the current position
 */
[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
  private Color _defaultColor = Color.white;
  private Color _blockedColor = Color.grey;
  private Color _exploredColor = Color.yellow;
  private Color _pathColor = Color.blue;

  private TextMeshPro _label;
  private Vector2Int _gridPosition = new Vector2Int();
  private GridManager _gridManager;

  private void Awake()
  {
    _gridManager = FindObjectOfType<GridManager>();

    _label = GetComponent<TextMeshPro>();
    _label.enabled = false; // start the game of with labeled not visible

    DisplayCoordinates(); // runs once in Awake in Game Mode
  }

  void Update()
  {
    // the script will only execute in Edit mode
    if (!Application.isPlaying)
    {
      DisplayCoordinates();
      UpdateObjectName();
      _label.enabled = true;
    }

    SetLabelColor();

    ToggleLabels();
  }

  private void SetLabelColor()
  {
    var node = _gridManager.GetNode(_gridPosition);

    if (node == null) return;

    if (!node.isWalkable)
    {
      _label.color = _blockedColor;
    }
    else if (node.isPath)
    {
      _label.color = _pathColor;
    }
    else if (node.isExplored)
    {
      _label.color = _exploredColor;
    }
    else
    {
      _label.color = _defaultColor;
    }
  }

  private void DisplayCoordinates()
  {
    if (_gridManager == null)
    {
      return;
    }

    _gridPosition.x = Mathf.RoundToInt(transform.parent.position.x / _gridManager.UnityGridSize);
    _gridPosition.y = Mathf.RoundToInt(transform.parent.position.z / _gridManager.UnityGridSize);

    _label.text = $"{_gridPosition.x},{_gridPosition.y}";
  }

  void UpdateObjectName()
  {
    transform.parent.name = _gridPosition.ToString();
  }

  /**
   * turns the text labels on and off when hit a key
   */
  private void ToggleLabels()
  {
    if (Input.GetKeyDown(KeyCode.C))
    {
      _label.enabled = !_label.enabled;
    }
  }
}