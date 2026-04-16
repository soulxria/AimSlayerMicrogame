using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardSelector : MonoBehaviour
{
    float projectileSize;
    float targetSpeed;
    float targetSize;  
    float targetDuration;

    float addProjectileSize;
    float addTargetSpeed;
    float addTargetSize;
    float addTargetDuration;

    string writtenStat;
    string writtenNegStat;

    float statValue;
    float negStatValue;

    public TextMeshProUGUI card1Text;
    public TextMeshProUGUI card2Text;   
    public TextMeshProUGUI card3Text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CardBase cardBase;

    public float FetchStat(StatType currentStat)
    {
        switch (currentStat)
        {
            case StatType.ProjectileSize:
                return projectileSize;
            case StatType.TargetSpeed:
                return targetSpeed;
            case StatType.TargetSize:
                return targetSize;
            case StatType.TargetDuration:
                return targetDuration;
            default:
                return 0.0f;
        }
    }

    public enum StatType
    {
        ProjectileSize,
        TargetSpeed,
        TargetSize,
        TargetDuration
    }

    public StatType currentStat;
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
        Dictionary<string, string> statToString = new Dictionary<string, string>() //for UI purposes
        {
            {"projectileSize", "Projectile Size: "+addProjectileSize},
            {"targetSpeed", "Target Speed: "+addTargetSpeed},
            {"targetSize", "Projectile Size: "+addTargetSize},
            {"targetDuration", "Target Duration: "+addTargetDuration}
        };

        Dictionary<string, float> statToValue = new Dictionary<string, float>() //for value adjustment
        {
            {"projectileSize", addProjectileSize},
            {"targetSpeed", addTargetSpeed},
            {"targetSize", addTargetSize},
            {"targetDuration", addTargetDuration}
        };

        //card base will gather which stats it wants, this script is used to quantify those stats and pass into the game.
        if (card.twoWay == true)
        {
            addProjectileSize = Random.Range(5.0f, 12.0f);
            addTargetSpeed = Random.Range(-5.0f, -12.0f);
            addTargetSize = Random.Range(3.0f, 8.0f);
            addTargetDuration = Random.Range(.03f, 1.0f);

            writtenStat = statToString[card.positiveStatChosen];
            statValue = statToValue[card.positiveStatChosen];

            addProjectileSize = Random.Range(5.0f, 8.0f);
            addTargetSpeed = Random.Range(-5.0f, -8.0f);
            addTargetSize = Random.Range(3.0f, 5.0f);
            addTargetDuration = Random.Range(.03f, .07f);

            writtenNegStat = statToString[card.negativeStatChosen];
            negStatValue = statToValue[card.negativeStatChosen];
        }
        else
        {
            float addProjectileSize = Random.Range(5.0f, 8.0f);
            float addTargetSpeed = Random.Range(-5.0f, -8.0f);
            float addTargetSize = Random.Range(3.0f, 5.0f);
            float addTargetDuration = Random.Range(.03f, .07f);

            writtenStat = statToString[card.positiveStatChosen];
            statValue = statToValue[card.positiveStatChosen];
        }
    }

    void ModifyStats(CardBase card)
    {
        if (card.twoWay)
        {
            float positiveModification = statValue;
            float negativeModification = negStatValue;
        }

    }

    void CardVisOutput(CardBase card)
    {
        //use the written stat to output to the UI
        if (card.twoWay)
        {
            //output writtenStat and writtenNegStat to the UI
        }
        else
        {
            //output writtenStat to the UI
        }
    }
}
