using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using System.Collections.Generic;

public class CardBase : MonoBehaviour   
{

    public bool twoWay;
    public string positiveStatChosen;
    public string negativeStatChosen;

    //list of stats to choose from when creating cards
    void Awake()
    {
            CardBuiler();
    }
    void CardBuiler()
    {
        Debug.Log("Card Builder Called");
        List<string> statsList = new List<string> { "projectileSize", "targetSpeed", "targetSize", "targetDuration" };
        twoWay = Random.Range(0, 2) == 0;

        if (twoWay)
        {
            //use new values for two way cards
            Debug.Log("Two Way Card Created");

            positiveStatChosen = statsList[Random.Range(0, 4)];
            foreach (string stat in statsList)
            {
                if (stat == positiveStatChosen)
                {
                    statsList.Remove(stat);
                    break;
                }
            }
            negativeStatChosen = statsList[Random.Range(0, 3)];

        }
        else
        {
            Debug.Log("One Way Card Created");
            positiveStatChosen = statsList[Random.Range(0, 4)];
        }
    }
}
