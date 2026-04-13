using UnityEngine;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    private Queue<GameObject> targetPool = new Queue<GameObject>();
    public GameObject target;
    public GameObject targetPlane;
    public CardSelector cardSelector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(target);
            target.SetActive(false);
            targetPool.Enqueue(target);
        }
    }

    // Update is called
    // Spawn Target
    // Randomize size and position on plane
    void SpawnTarget() 
    {
        GameObject currentTarget = targetPool.Dequeue();
        target.transform.localScale = new Vector3(cardSelector.FetchStat(2), cardSelector.FetchStat(2), 0);
        target.transform.position = new Vector3(Random.Range(-targetPlane.transform.localScale.x, targetPlane.transform.localScale.x), Random.Range(-targetPlane.transform.localScale.y, targetPlane.transform.localScale.y), 0);
        currentTarget.SetActive(true);
    }
}
