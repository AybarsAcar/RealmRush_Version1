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
  [SerializeField] Color defaultColor = Color.white;
  [SerializeField] Color blockedColor = Color.grey;
  
  private TextMeshPro _label;
  private Vector2Int _gridPosition = new Vector2Int();
  private Waypoint _waypoint;

  private void Awake()
  {
    _label = GetComponent<TextMeshPro>();
    _label.enabled = false; // start the game of with labeled not visible
    
    DisplayCoordinates(); // runs once in Awake in Game Mode
    _waypoint = GetComponentInParent<Waypoint>();
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
    var labelColor = _waypoint.IsPlaceable ? defaultColor : blockedColor;
    _label.color = labelColor;
  }

  private void DisplayCoordinates()
  {
    _gridPosition.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
    _gridPosition.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

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