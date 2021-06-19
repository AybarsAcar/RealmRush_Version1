using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
  [SerializeField] private int cost = 75;

  private const float BuildDelayTime = 1f;

  private void Start()
  {
    StartCoroutine(Build());
  }

  private IEnumerator Build()
  {
    foreach (Transform child in transform)
    {
      child.gameObject.SetActive(false);
      foreach (Transform grandchild in child)
      {
        grandchild.gameObject.SetActive(false);
      }
    }
    
    foreach (Transform child in transform)
    {
      child.gameObject.SetActive(true);

      yield return new WaitForSeconds(BuildDelayTime);
      
      foreach (Transform grandchild in child)
      {
        grandchild.gameObject.SetActive(true);
      }
    }
  }

  /**
   * when this method is called from another script it instantiates this game object
   * so it will call teh Start method since it instantiates this object
   * hence the Build is called
   */
  public bool CreateTower(Tower tower, Vector3 position)
  {
    var bank = FindObjectOfType<Bank>();

    if (bank == null) return false;
    
    if (bank.CurrentBalance < cost) return false;
    
    Instantiate(tower.gameObject, position, Quaternion.identity);
    bank.Withdraw(cost);
    return true;
  }
}