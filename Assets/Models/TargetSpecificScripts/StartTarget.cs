using UnityEngine;

public class StartTarget : MonoBehaviour
{
    public GameData gameData;
    private void OnDisable()
    {
        gameData.InitiateGame();
    }
}
