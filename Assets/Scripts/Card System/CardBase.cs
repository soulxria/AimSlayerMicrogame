using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using System.Collections.Generic;

public class CardBase 
{
    public bool twoWay;
    public CardSelector.StatType positiveStatChosen;
    public CardSelector.StatType negativeStatChosen;
    //list of stats to choose from when creating cards
    public CardBase()
    {
        CardBuiler();
    }
    void CardBuiler()
    {
        Debug.Log("Card Builder Called");
        twoWay = Random.Range(0, 2) == 0;

        if (twoWay)
        {
            //use new values for two way cards
            Debug.Log("Two Way Card Created");
            
            positiveStatChosen = GetRandomStatType();
            do
            {
                negativeStatChosen = GetRandomStatType();
            }while(negativeStatChosen == positiveStatChosen);

        }
        else
        {
            Debug.Log("One Way Card Created");
            positiveStatChosen = GetRandomStatType();
        }
    }

    private static CardSelector.StatType GetRandomStatType()
    {
        return (CardSelector.StatType)Random.Range(0, (int)CardSelector.StatType.NUM);
    }
}
