using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

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

    public Canvas cardSelectUI;

    Dictionary<string, string> statToString;
    Dictionary<string, float> statToValue;
    Dictionary<string, StatType> statToEnum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CardBase cardBase;
    CardBase card1;
    CardBase card2;
    CardBase card3;

    void Start() //initiate only once at start to borrow values instead of each time draw cards is called
    {
        statToString = new Dictionary<string, string>() //for UI purposes
        {
            {"projectileSize", "Projectile Size: "+addProjectileSize},
            {"targetSpeed", "Target Speed: "+addTargetSpeed},
            {"targetSize", "Projectile Size: "+addTargetSize},
            {"targetDuration", "Target Duration: "+addTargetDuration}
        };

        statToValue = new Dictionary<string, float>() //for value adjustment
        {
            {"projectileSize", addProjectileSize},
            {"targetSpeed", addTargetSpeed},
            {"targetSize", addTargetSize},
            {"targetDuration", addTargetDuration}
        };

        statToEnum = new Dictionary<string, StatType>() //for value adjustment
        {
            {"projectileSize", StatType.ProjectileSize},
            {"targetSpeed", StatType.TargetSpeed},
            {"targetSize", StatType.TargetSize},
            {"targetDuration", StatType.TargetDuration}
        };
    }
    

    public float FetchStat(StatType currentStat) //this will be used to fetch the current value of the stat for use in the game
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

    public enum StatType //enum to hold the stat types
    {
        ProjectileSize,
        TargetSpeed,
        TargetSize,
        TargetDuration
    }

    public StatType currentStat; //instance enum 
    public void DrawCards() //main function to fetch cards and their information.
    {
        card1 = new CardBase();
        StatBreakdown(card1);
        CardVisOutput(card1, card1Text);
        card2 = new CardBase();
        StatBreakdown(card2);
        CardVisOutput(card2, card2Text);
        card3 = new CardBase();
        StatBreakdown(card3);
        CardVisOutput(card3, card3Text);
        cardSelectUI.enabled = true; //enable the card select UI when drawing cards
    }

    void StatBreakdown(CardBase card) //assigns card stats
    {
        //card base will gather which stats it wants, this script is used to quantify those stats and pass into the game.
        if (card.twoWay == true)
        {
            //list additive values that will be inserted later for value 
            addProjectileSize = Random.Range(5.0f, 12.0f);
            addTargetSpeed = Random.Range(-5.0f, -12.0f);
            addTargetSize = Random.Range(3.0f, 8.0f);
            addTargetDuration = Random.Range(.03f, 1.0f);

            //this will gather the text version and value version of the stats for use in the UI and the stat modification, respectively
            writtenStat = statToString[card.positiveStatChosen];
            statValue = statToValue[card.positiveStatChosen];

            //list of additive values that will be inserted for decrement
            addProjectileSize = Random.Range(5.0f, 8.0f);
            addTargetSpeed = Random.Range(-5.0f, -8.0f);
            addTargetSize = Random.Range(3.0f, 5.0f);
            addTargetDuration = Random.Range(.03f, .07f);

            //gather text and value for negative stat
            writtenNegStat = statToString[card.negativeStatChosen];
            negStatValue = statToValue[card.negativeStatChosen];
        }
        else
        {
            //gather the stat one time
            float addProjectileSize = Random.Range(5.0f, 8.0f);
            float addTargetSpeed = Random.Range(-5.0f, -8.0f);
            float addTargetSize = Random.Range(3.0f, 5.0f);
            float addTargetDuration = Random.Range(.03f, .07f);

            writtenStat = statToString[card.positiveStatChosen];
            statValue = statToValue[card.positiveStatChosen];
        }
    }

    public void ModifyStats(int cardIndex) //used via button system to modify the stats based on the card chosen
    {
        CardBase card = GetCardByIndex(cardIndex);
        if (card.twoWay)
        {
            float positiveModification = statValue;
            ChangeSpecificStat(statToEnum[card.positiveStatChosen], positiveModification);
            float negativeModification = negStatValue;
            ChangeSpecificStat(statToEnum[card.negativeStatChosen], negativeModification);
        }
        else
        {
            float positiveModification = statValue;
            ChangeSpecificStat(statToEnum[card.positiveStatChosen], positiveModification);
        }
        cardSelectUI.enabled = false; //close the card select UI after selection
    }

    CardBase GetCardByIndex(int cardIndex)
    {
        switch (cardIndex)
        {
            case 1:
                return card1;
            case 2:
                return card2; 
            case 3:
                return card3; 
            default:
                return null;
        }
    }

    void ChangeSpecificStat(StatType statToChange, float modificationValue) //interact with the enums to change the stat values based on the card chosen
    {
        switch (statToChange)
        {
            case StatType.ProjectileSize:
                projectileSize += modificationValue;
                break;
            case StatType.TargetSpeed:
                targetSpeed += modificationValue;
                break;
            case StatType.TargetSize:
                targetSize += modificationValue;
                break;
            case StatType.TargetDuration:
                targetDuration += modificationValue;
                break;
        }
    }

    void CardVisOutput(CardBase card, TextMeshProUGUI cardText)
    {
        //use the written stat to output to the UI
        if (card.twoWay)
        {
            cardText.text = writtenStat + "\n" + writtenNegStat;
        }
        else
        {
            cardText.text = writtenStat;
        }
    }
}   
