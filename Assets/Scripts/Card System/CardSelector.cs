using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CardSelector : MonoBehaviour
{

    [SerializeField] float projectileSize = 1.0f;
    [SerializeField] float targetSpeed = 10.0f;
    [SerializeField] float targetSize = 1.0f;
    [SerializeField] float targetDuration = 1.0f;

    float addProjectileSize;
    float addTargetSpeed;
    float addTargetSize;
    float addTargetDuration;

    [SerializeField] string writtenStat;
    [SerializeField] string writtenNegStat;

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
    public GameData gameData; //reference to the game data to pass the card choice for use in the game
    CardBase card1;
    CardBase card2;
    CardBase card3;

    private void Start()
    {   //clamp the stats to reasonable values to avoid breaking the game
        statToEnum = new Dictionary<string, StatType>() //for value adjustment, getting the correct stat
        {
            {"projectileSize", StatType.ProjectileSize},
            {"targetSpeed", StatType.TargetSpeed},
            {"targetSize", StatType.TargetSize},
            {"targetDuration", StatType.TargetDuration}
        };

        statToString = new Dictionary<string, string>() //for UI purposes
        {
            {"projectileSize", "Projectile Size: "+addProjectileSize},
            {"targetSpeed", "Target Speed: "+addTargetSpeed},
            {"targetSize", "Target Size: "+addTargetSize},
            {"targetDuration", "Target Duration: "+addTargetDuration}
        };

        statToValue = new Dictionary<string, float>() //for value adjustment, gets the value
        {
            {"projectileSize", addProjectileSize},
            {"targetSpeed", addTargetSpeed},
            {"targetSize", addTargetSize},
            {"targetDuration", addTargetDuration}
        };
        cardSelectUI.enabled = false; //make sure the card select UI is disabled at the start of the game
    }
    void UpdateDictionaries(float value1, float value2, float value3, float value4) //initiate only once at start to borrow values instead of each time draw cards is called
    {
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    statToString["projectileSize"] = "Projectile Size: " + value1.ToString("F2");
                    statToValue["projectileSize"] = value1;
                    break;
                case 1:
                    statToString["targetSpeed"] = "Target Speed: " + value2.ToString("F2");
                    statToValue["targetSpeed"] = value2;
                    break;
                case 2:
                    statToString["targetSize"] = "Target Size: " + value3.ToString("F2");
                    statToValue["targetSize"] = value3;
                    break;
                case 3:
                    statToString["targetDuration"] = "Target Duration: " + value4.ToString("F2");
                    statToValue["targetDuration"] = value4;
                    break;                                                      
            }
            
        }
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

    public void ResetStats() //used to reset the stats after each game
    {
        projectileSize = 1.0f;
        targetSpeed = 10.0f;
        targetSize = 1.0f;
        targetDuration = 1.0f;
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
        Debug.Log("Drawing Cards");
        cardSelectUI.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        card1 = this.AddComponent<CardBase>();//initiate the card base to gather which stats it wants, then break down those stats into values and text for the UI and stat modification
        StatBreakdown(card1);
        CardVisOutput(card1, card1Text);
        card2 = this.AddComponent<CardBase>();
        StatBreakdown(card2);
        CardVisOutput(card2, card2Text);
        card3 = this.AddComponent<CardBase>();
        StatBreakdown(card3);
        CardVisOutput(card3, card3Text);
         //enable the card select UI when drawing cards
    }

    private void PostCardSelection()
    {
        Destroy(card1);//remove the card base components after selection to avoid stacking cards and stats
        Destroy(card2);
        Destroy(card3);
    }
    void StatBreakdown(CardBase card) //assigns card stats
    {
        //card base will gather which stats it wants, this script is used to quantify those stats and pass into the game.
        if (card.twoWay == true)
        {
            //list additive values that will be inserted later for value 
            addProjectileSize = Random.Range(0.1f, 1.0f);
            addTargetSpeed = Random.Range(-5.0f, -12.0f);
            addTargetSize = Random.Range(2.0f, 4.0f);
            addTargetDuration = Random.Range(.03f, 1.0f);
            UpdateDictionaries(addProjectileSize, addTargetSpeed, addTargetSize, addTargetDuration);

            //this will gather the text version and value version of the stats for use in the UI and the stat modification, respectively
            writtenStat = statToString[card.positiveStatChosen];
            statValue = statToValue[card.positiveStatChosen];

            //list of additive values that will be inserted for decrement
            addProjectileSize = Random.Range(0.1f, 1.0f);
            addTargetSpeed = Random.Range(-5.0f, -8.0f);
            addTargetSize = Random.Range(2.0f, 3.0f);
            addTargetDuration = Random.Range(.03f, .07f);
            UpdateDictionaries(addProjectileSize, addTargetSpeed, addTargetSize, addTargetDuration);

            //gather text and value for negative stat
            writtenNegStat = statToString[card.negativeStatChosen];
            negStatValue = statToValue[card.negativeStatChosen];
        }
        else
        {
            //gather the stat one time
            addProjectileSize = Random.Range(0.1f, 1.0f);
            addTargetSpeed = Random.Range(-5.0f, -8.0f);
            addTargetSize = Random.Range(1.0f, 3.0f);
            addTargetDuration = Random.Range(.03f, .07f);
            UpdateDictionaries(addProjectileSize, addTargetSpeed, addTargetSize, addTargetDuration);

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
            float negativeModification = -1*negStatValue;
            ChangeSpecificStat(statToEnum[card.negativeStatChosen], negativeModification);
        }
        else
        {
            float positiveModification = statValue;
            ChangeSpecificStat(statToEnum[card.positiveStatChosen], positiveModification);
        }
        DisableCards(); //close the card select UI after selection
        gameData.EndCardSelect(); //pass the card chosen to the game data for use in the game
        Cursor.lockState = CursorLockMode.Locked;
        PostCardSelection();
    }

    public void DisableCards()
    {
        cardSelectUI.enabled = false;
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
                projectileSize = Mathf.Clamp(projectileSize += modificationValue, 0.1f, 3.0f);
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
