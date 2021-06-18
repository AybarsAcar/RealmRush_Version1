using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Locates the target
 * Faces the target and shoots them
 */
public class TargetLocator : MonoBehaviour
{
  [SerializeField] private Transform weapon;
  [SerializeField] private float range = 25f;
  [SerializeField] private ParticleSystem projectileParticles;
  
  private Transform _target;

  private void Update()
  {
    FindClosestTarget();

    AimWeapon();
  }

  private void FindClosestTarget()
  {
    var enemies = FindObjectsOfType<Enemy>();

    Transform closestTarget = null;
    var maxDistance = Mathf.Infinity;

    foreach (var enemy in enemies)
    {
      var targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
      
      if (maxDistance > targetDistance)
      {
        maxDistance = targetDistance;
        closestTarget = enemy.transform;
      }
    }

    _target = closestTarget;
  }

  private void AimWeapon()
  {
    var targetDistance = Vector3.Distance(transform.position, _target.position);
    
    weapon.LookAt(_target);

    if (targetDistance > range)
    {
      Attack(false);
    }
    else
    {
      Attack(true);
    }
  }

  void Attack(bool isActive)
  {
    var emissionModule = projectileParticles.emission;
    emissionModule.enabled = isActive;
  }
}