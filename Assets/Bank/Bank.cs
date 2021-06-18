using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
  [SerializeField] private int startingBalance = 250;

  [SerializeField] private TextMeshProUGUI displayBalance;

  private int _currentBalance;
  public int CurrentBalance => _currentBalance;

  private void Awake()
  {
    _currentBalance = startingBalance;
    UpdateDisplay();
  }

  public void Deposit(int amount)
  {
    if (amount < 0) throw new Exception("Amount must be positive");

    _currentBalance += amount;
    UpdateDisplay();
  }

  public void Withdraw(int amount)
  {
    if (amount < 0) throw new Exception("Amount must be positive");

    _currentBalance -= amount;
    UpdateDisplay();

    if (_currentBalance < 0)
    {
      // Lose the game
      ReloadScene();
    }
  }

  void UpdateDisplay()
  {
    displayBalance.text = "Gold " + _currentBalance.ToString();
  }

  void ReloadScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
}