using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TargetManager targetManager;
    public CardSelector cardSelector;
    public StartTarget startTarget;
    [SerializeField] private bool roundActive;
    public PlayerCameraController playerCameraController;

    private float sensMultiplier = 1.0f;
    private int highScore = 0;
    int currentRound;

    public GameObject startTextPanel;

    [SerializeField] protected float timeLimit;
    [SerializeField] protected float timeRemaining;
    [SerializeField] private int targetCount;
    [SerializeField] protected float spawnGap;
    [SerializeField] protected float spawnTimer;
    [SerializeField] protected float cardSelectTime;

    Coroutine roundActiveCR;
    Coroutine midRoundCR;
    Coroutine cardSelectCR;

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
        startTextPanel.SetActive(false);
        StartRound();
    }

    //initiate round values that scale off round number logarithmically.
    void StartRound()
    {
        currentRound++;
        Debug.Log("Round " + currentRound + " Starting.");
        spawnGap = 1.2f + cardSelector.FetchStat(CardSelector.StatType.TargetDuration);
        spawnGap = Mathf.Clamp(spawnGap, 0.5f, 3);
        
        timeLimit = 20.0f - (Mathf.Log(currentRound, 10));
        targetCount = Mathf.FloorToInt(12f + (Mathf.Log(currentRound, 1.6f)));
        gameStarted = true;

        //start timer for end round
        timeRemaining = timeLimit;
        roundActiveCR = StartCoroutine(ActiveGame());
    }

    public void EndGame(int fromPauseMenu)
    {
        if (fromPauseMenu == 0)
        {
            gameStarted = false;
            Debug.Log("Game Ended. Final Score: " + (currentRound - 1) + " High Score: " + CheckHighScore(currentRound - 1));
            startTarget.gameObject.SetActive(true); //reactivate start target to allow for restarting the game. 
            currentRound = 0;
            cardSelector.ResetStats();
            startTextPanel.SetActive(true);
        }
        else if (fromPauseMenu == 1)
        {
            gameStarted = false;
            Debug.Log("Game Ended. Final Score: " + (currentRound - 1) + " High Score: " + CheckHighScore(currentRound - 1));
            startTarget.gameObject.SetActive(true); //reactivate start target to allow for restarting the game. 
            currentRound = 0;
            cardSelector.ResetStats();
            startTextPanel.SetActive(true);
            playerCameraController.ResumeGame();
        }
    }

    IEnumerator ActiveGame()
    {
        while (gameStarted == true)
        {
            roundActive = true;
            Debug.Log("Round activity on now" );
            yield return midRoundCR = StartCoroutine(RoundActivity());
            StopCoroutine(midRoundCR);//start round activity, which will end when the target count is zero or the time limit is up.
            if (targetCount > 0)
            {
                Debug.Log("Round Ended Due to Time Limit");
                break;
            }
            else
            {
                cardSelectTime = 8f;
                cardSelector.DrawCards();
                yield return cardSelectCR = StartCoroutine(CardSelectionCycle());
                Debug.Log("Card Selection Ended");
                StopCoroutine(cardSelectCR);
                InputSystem.actions.FindAction("Shoot").Enable();
                StartRound();
            }
        }
        EndGame(0);
        yield return new WaitUntil(() => gameStarted == false);
        StopCoroutine(roundActiveCR);
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
            if (currentTarget == null)
            {
                Debug.Log("Too many targets on field, failed to spawn");
                continue;
            }

            spawnTimer = spawnGap;
            yield return new WaitUntil(() => spawnTimer <= 0); //wait until spawn gap is up to spawn the next target, allowing for a consistent spawn rate.
            if (currentTarget != null)
            {
                targetManager.TargetMissed(currentTarget); //automatically delete
            }

        }
    }



    //spawn gap code, used to skip the spawn gap when a target is hit, allowing for faster gameplay and more targets on screen.
    public void SkipGap()
    {
        spawnTimer = 0f;
    }



    //round timer code, used to skip the round when the time limit is up, allowing for faster gameplay and more rounds.
    public void SkipRound()
    {
        timeRemaining = 0f;
    }

    public void EndCardSelect()
    {
        cardSelectTime = 0f;
    }
    IEnumerator CardSelectionCycle()
    {
        Debug.Log("Card Selection Started");
        InputSystem.actions.FindAction("Shoot").Disable();
        yield return new WaitUntil(() => cardSelectTime <= 0f);
        cardSelector.DisableCards();
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
        if (cardSelectTime > 0f)
        {
            cardSelectTime -= Time.deltaTime;
        }


        if (cardSelectTime <= 0f)
        {
            cardSelectTime = 0f;
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

