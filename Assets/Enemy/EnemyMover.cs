using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

/**
 * this will follow a path - a waypoint
 */
[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
  [SerializeField] [Range(0f, 5f)] private float speed = 0.5f;
  [SerializeField] private List<Waypoint> _path = new List<Waypoint>();

  private Enemy _enemy;

  private void Awake()
  {
    _enemy = GetComponent<Enemy>();
  }

  private void OnEnable()
  {
    FindPath();
    ReturnToStart();
    StartCoroutine(FollowPath());
  }

  void FindPath()
  {
    // clear the existing path first
    // so it won't add the new path to our existing path
    _path.Clear();

    var parent = GameObject.FindGameObjectWithTag(Tag.Path);

    foreach (Transform child in parent.transform)
    {
      var waypoint = child.GetComponent<Waypoint>();

      if (waypoint != null)
      {
        _path.Add(child.GetComponent<Waypoint>());
      }
    }
  }

  /**
   * move our enemy into the first waypoint
   */
  void ReturnToStart()
  {
    transform.position = _path[0].transform.position;
  }

  private IEnumerator FollowPath()
  {
    foreach (var waypoint in _path)
    {
      var startPos = transform.position;
      var endPos = waypoint.transform.position;

      transform.LookAt(endPos);

      float travelPercent = 0;

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