using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;
using Utils;

/**
 * this will follow a path - a waypoint
 */
[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
  [SerializeField] [Range(0f, 5f)] private float speed = 0.5f;

  private List<Node> _path = new List<Node>();

  private Enemy _enemy;
  private GridManager _gridManager;
  private Pathfinder _pathfinder;

  private void Awake()
  {
    _enemy = GetComponent<Enemy>();
    _gridManager = FindObjectOfType<GridManager>();
    _pathfinder = FindObjectOfType<Pathfinder>();
  }

  private void OnEnable()
  {
    ReturnToStart();
    RecalculatePath(true);
  }

  private void RecalculatePath(bool resetPath)
  {
    var coordinate =
      resetPath ? _pathfinder.StartCoordinate : _gridManager.GetCoordinatesFromPosition(transform.position);

    StopAllCoroutines();

    // clear the existing path first
    // so it won't add the new path to our existing path
    _path.Clear();

    _path = _pathfinder.GetNewPath(coordinate);

    StartCoroutine(FollowPath());
  }

  /**
   * move our enemy into the first waypoint
   */
  void ReturnToStart()
  {
    transform.position = _gridManager.GetPositionFromCoordinates(_pathfinder.StartCoordinate);
  }

  private IEnumerator FollowPath()
  {
    for (int i = 1; i < _path.Count; i++)
    {
      var startPos = transform.position;
      var endPos = _gridManager.GetPositionFromCoordinates(_path[i].coordinate); // next nodes position
      float travelPercent = 0;

      transform.LookAt(endPos);

      while (travelPercent < 1f)
      {
        travelPercent += Time.deltaTime * speed; // increase travel percent at each frame
        transform.position = Vector3.Lerp(startPos, endPos, travelPercent);

        yield return new WaitForEndOfFrame();
      }
    }

    // after finishing the path
    // disable instead of Destroy
    _enemy.StealGold();
    gameObject.SetActive(false);
  }
}