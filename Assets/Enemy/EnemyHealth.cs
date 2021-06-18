using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tracks enemy HP and damage it took
 * destroys itself if no hp left
 */
[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
  [SerializeField] private int maxHP = 3;
  [SerializeField] private int difficultyRamp = 2;

  private int _currentHP = 0;
  private Enemy _enemy;

  private void Awake()
  {
    _enemy = GetComponent<Enemy>();
  }

  void OnEnable()
  {
    _currentHP = maxHP;
  }

  private void OnParticleCollision(GameObject other)
  {
    ProcessHit();
  }

  void ProcessHit()
  {
    _currentHP--;

    if (_currentHP <= 0)
    {
      gameObject.SetActive(false); // instead of Destroy so its back in the pool
      _enemy.RewardGold();
      maxHP += difficultyRamp; // enemies get stronger over time as they die
    }
  }
}