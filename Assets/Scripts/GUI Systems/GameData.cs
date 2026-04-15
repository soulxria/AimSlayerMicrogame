using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private float sensMultiplier = 1.0f;
    private int highScore = 0;
    int currentRound;

    private float timeLimit;
    public float timeRemaining;
    private float targetCount;

    private bool gameStarted = false;

    public float setSens(float Sens)
    {
        sensMultiplier = Sens;
        return sensMultiplier;
    }

    public float getSens()
    {
        return sensMultiplier;
    }

    int UpdateHighScore(int score)
    {
        if (score > highScore)
        {
            highScore = score;
        }
        return highScore;
    }

    //initiate round values that scale off round number logarithmically.
    void StartRound ()
    {
        currentRound++;
        timeLimit = 30.0f - (Mathf.Log(currentRound, 10));
        targetCount = 5.0f + (Mathf.Log(currentRound, 3));
        gameStarted = true;
    }
    IEnumerator ActiveGame()
    {   


        yield return new WaitUntil(() => gameStarted = false);
    }

    IEnumerator ActiveRound()
    {


        yield return new WaitUntil(() => targetCount <= 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
