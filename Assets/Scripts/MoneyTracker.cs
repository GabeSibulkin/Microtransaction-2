using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoneyTracker : MonoBehaviour
{

    public int valueToEarn;
    public int currentValue;
    public TextMeshProUGUI scoreTracker;
    // Start is called before the first frame update
    void Start()
    {
        currentValue = 0;
        UpdateTracker();
    }

    public void AddMoney(int addition)
    {
        currentValue+= addition;
        UpdateTracker();
    }

    public void UpdateTracker()
    {
        scoreTracker.text = currentValue + "/" + valueToEarn;
    }

    public void CheckIfWon()
    {
        if (valueToEarn > currentValue)
        {
            Debug.Log("Not enough money to win");
            
        }
        else 
        { 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        }
    }
}
