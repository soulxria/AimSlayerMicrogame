using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TargetManager targetManager;
    public CardSelector cardSelector;
    private bool roundActive;

    private float sensMultiplier = 1.0f;
    private int highScore = 0;
    int currentRound;

    protected float timeLimit;
    protected float timeRemaining;
    private int targetCount;
    protected float spawnGap;
    protected float spawnTimer;

    private bool gameStarted = false;

    public Coroutine spawnGapRoutine;

    public float setSens(float Sens)
    {
        sensMultiplier = Sens;
        return sensMultiplier;
    }

    public float getSens()
    {
        return sensMultiplier;
    }

    public int GetTargetCount()
    {
        return targetCount;
    }

    public void decrementTargetCount()
    {
        targetCount--;
    }

    int CheckHighScore(int score)
    {
        if (score > highScore)
        {
            highScore = score;
        }
        return highScore;
    }

    public void InitiateGame()
    {
        StartRound();
    }

    //initiate round values that scale off round number logarithmically.
    void StartRound()
    {
        currentRound++;
        Debug.Log("Round " + currentRound + " Starting.");
        spawnGap = 1.5f + cardSelector.FetchStat(CardSelector.StatType.TargetDuration);
        timeLimit = 30.0f - (Mathf.Log(currentRound, 10));
        targetCount = Mathf.FloorToInt(10f + (Mathf.Log(currentRound, 1.6f)));
        gameStarted = true;
        roundActive = true;
        //start timer for end round
        StartCoroutine(CycleRoundTimer());
        StartCoroutine(ActiveRound());
    }

    IEnumerator ActiveRound()
    {
        while (gameStarted == true)
        {
            yield return StartCoroutine(RoundActivity()); //start round activity, which will end when the target count is zero or the time limit is up.
            if (targetCount > 0)
            {
                break;
            }
            else
            {
                StartRound();
            }
        }
        gameStarted = false;
        yield return new WaitUntil(() => gameStarted == false);
    }

    IEnumerator RoundActivity()
    {
        while (roundActive) //keeps track of round activity
        {
            if (targetCount <= 0) //target count zero, end round and let the new one start
            {
                Debug.Log("Round Ended Early");
                SkipRound();
                roundActive = false;
                yield break;
            }

            Debug.Log("Spawning Target");
            GameObject currentTarget = targetManager.SpawnTarget(cardSelector.FetchStat(CardSelector.StatType.TargetSize));
            yield return StartCoroutine(CycleSpawnTimer());
            if (currentTarget != null)
            {
                targetManager.TargetMissed(currentTarget); //automatically delete
            }

        }
    }

    public void SkipGap()
    {
        spawnTimer = 0f;
    }
    // Update is called once per frame
    IEnumerator CycleSpawnTimer()
    {
        spawnTimer = spawnGap;
        while (spawnTimer > 0f)
            yield return new WaitUntil(() => spawnTimer <= 0f);
    }

    public void SkipRound()
    {
        timeRemaining = 0f;
    }
    // Update is called once per frame
    IEnumerator CycleRoundTimer()
    {
        timeRemaining = timeLimit;
        yield return new WaitUntil(() => timeRemaining <= 0f);
    }

    void Update() //timers to avoid confliction with double coroutines
    {
        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
        }
        if (spawnTimer > 0f)
        {
            spawnTimer -= Time.deltaTime;
        }

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
        }
        if (spawnTimer <= 0f)
        {
            spawnTimer = 0f;
        }
    }
}

