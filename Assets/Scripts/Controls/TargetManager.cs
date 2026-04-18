using UnityEngine;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    private Queue<GameObject> targetPool = new Queue<GameObject>(10);
    public GameObject target;
    public GameObject targetPlane;
    public CardSelector cardSelector;
    public GameData gameData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject targetInstance = Instantiate(target);
            targetInstance.SetActive(false);
            targetPool.Enqueue(targetInstance);
        }
    }

    // Update is called
    // Spawn Target
    // Randomize size and position on plane
    public GameObject SpawnTarget(float additiveSize) 
    {
       
        GameObject currentTarget = targetPool.Dequeue();
        if (additiveSize != 0f) //avoid a divide by zero error.
        {
            currentTarget.transform.localScale = new Vector3((2f * (1f + (additiveSize) / 100f)), 0f, (2f * (1f + additiveSize / 100f)));
        }
        else
        {
            currentTarget.transform.localScale = new Vector3(2f, 0f, 2f);
        }
        currentTarget.transform.position = new Vector3(
            Random.Range(targetPlane.transform.position.x - targetPlane.transform.localScale.x*2, targetPlane.transform.position.x + targetPlane.transform.localScale.x*2),
            Random.Range(targetPlane.transform.position.y - targetPlane.transform.localScale.y*40, targetPlane.transform.position.y + targetPlane.transform.localScale.y*40),
            targetPlane.transform.position.z - 2f);
        currentTarget.SetActive(true);

        return currentTarget;

    }

    public void TargetMissed(GameObject targetShot)
    {
        targetShot.SetActive(false);
        targetPool.Enqueue(targetShot);
    }

    public void TargetHit(GameObject targetShot)
    {
        targetShot.SetActive(false);
        targetPool.Enqueue(targetShot);
        gameData.decrementTargetCount();
        gameData.SkipGap();
        Debug.Log("Target Hit!"+" Remaining Targets: " + (gameData.GetTargetCount())); 
    }
}
