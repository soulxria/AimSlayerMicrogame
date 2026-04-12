using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardSelector : MonoBehaviour
{
    float projectileSize;
    float targetSpeed;
    float targetSize;
    float targetDuration;

    string writtenStat;
    string writtenNegStat;

    float statValue;
    float negStatValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CardBase cardBase;
    void DrawCards()
    {
        CardBase card1 = new CardBase();
        StatBreakdown(card1);
        CardBase card2 = new CardBase();
        StatBreakdown(card2);
        CardBase card3 = new CardBase();
        StatBreakdown(card3);
    }

    void StatBreakdown(CardBase card)
    {
        Dictionary<string, string> statToString = new Dictionary<string, string>()
        {
            {"projectileSize", "Projectile Size: "+projectileSize},
            {"targetSpeed", "Target Speed: "+targetSpeed},
            {"targetSize", "Projectile Size: "+targetSize},
            {"targetDuration", "Target Duration: "+targetDuration}
        };

        Dictionary<string, float> statToValue = new Dictionary<string, float>()
        {
            {"projectileSize", projectileSize},
            {"targetSpeed", targetSpeed},
            {"targetSize", targetSize},
            {"targetDuration", targetDuration}
        };

        if (card.twoWay == true)
        {
            projectileSize = Random.Range(5.0f, 12.0f);
            targetSpeed = Random.Range(-5.0f, -12.0f);
            targetSize = Random.Range(3.0f, 8.0f);
            targetDuration = Random.Range(.03f, 1.0f);

            writtenStat = statToString[card.positiveStatChosen];
            statValue = statToValue[card.positiveStatChosen];

            projectileSize = Random.Range(5.0f, 8.0f);
            targetSpeed = Random.Range(-5.0f, -8.0f);
            targetSize = Random.Range(3.0f, 5.0f);
            targetDuration = Random.Range(.03f, .07f);

            writtenNegStat = statToString[card.negativeStatChosen];
            negStatValue = statToValue[card.negativeStatChosen];
        }
        else
        {
            float projectileSize = Random.Range(5.0f, 8.0f);
            float targetSpeed = Random.Range(-5.0f, -8.0f);
            float targetSize = Random.Range(3.0f, 5.0f);
            float targetDuration = Random.Range(.03f, .07f);

            writtenStat = statToString[card.positiveStatChosen];
            statValue = statToValue[card.positiveStatChosen];
        }
    }

    void ModifyStats(CardBase card)
    {


    }

}
