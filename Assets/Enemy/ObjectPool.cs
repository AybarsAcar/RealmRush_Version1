using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * our object pool for creating enemy waves
 * enemies will be instantiated all at once
 * then activated and deactivated for performance
 */
public class ObjectPool : MonoBehaviour
{
  [SerializeField] private GameObject enemyPrefab;
  [SerializeField] [Range(0, 50)] private int poolSize = 5;
  [SerializeField] [Range(0.1f, 30f)] private float spawnTimer = 2.5f;

  private GameObject[] _pool;

  private void Awake()
  {
    PopulatePool();
  }

  private void PopulatePool()
  {
    _pool = new GameObject[poolSize];

    for (int i = 0; i < poolSize; i++)
    {
      _pool[i] = Instantiate(enemyPrefab, transform);
      _pool[i].SetActive(false); // make sure deactivate them
    }
  }

  void OnEnable()
  {
    StartCoroutine(SpawnEnemy());
  }

  private IEnumerator SpawnEnemy()
  {
    while (true)
    {
      EnableObjectInPool();
      yield return new WaitForSeconds(spawnTimer);
    }
  }

  private void EnableObjectInPool()
  {
    foreach (var enemy in _pool)
    {
      if (enemy.activeInHierarchy == false)
      {
        enemy.SetActive(true);
        return;
      }
    }
  }
}