using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameData gameData;
    public CardSelector cardStats;
    public TargetManager targetManager;

    private float xRotation;
    private float sensitivity = 0;
    public Transform playerBody;
    public Transform camTransform;

    private PlayerInput controls;
    private bool isPaused = false;
    public Canvas pauseScreen;

    public void ChangeSens(float newSens)
    {
        sensitivity = gameData.setSens(newSens);
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        sensitivity = gameData.getSens();
        pauseScreen.enabled = false;
    }
    
    public void OnMouseX(InputAction.CallbackContext context)
    {
        float mouseX = context.ReadValue<float>() * (1 + sensitivity) * Time.deltaTime;
        //playerBody.Rotate(Vector3.up * mouseX);
        transform.Rotate(0f, mouseX, 0f);
    }

    public void OnMouseY(InputAction.CallbackContext context)
    {
        float mouseY = context.ReadValue<float>() * (-1 + -1*sensitivity) * Time.deltaTime;
        Vector3 newRot = camTransform.rotation.eulerAngles + new Vector3(mouseY, 0f, 0f);
        camTransform.rotation = (Quaternion.Euler(newRot.x, newRot.y , newRot.z));
    }   

    public void ShootProjectile(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.SphereCast(camTransform.position, (1f + cardStats.FetchStat(CardSelector.StatType.ProjectileSize)), camTransform.forward, out hit, 100f))
        {
            if (hit.collider.gameObject.CompareTag("Target"))
            {
                targetManager.TargetHit(hit.collider.gameObject);
            }
            if (hit.collider.gameObject.CompareTag("StartTarget"))
            {
                hit.collider.gameObject.SetActive(false);
            }
        }
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InputSystem.actions.FindActionMap("Player").Disable();
            InputSystem.actions.FindActionMap("UI").Enable();
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            isPaused = true;
            pauseScreen.enabled = true;
        }
        else if (context.performed && isPaused == true)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        InputSystem.actions.FindActionMap("Player").Enable();
        InputSystem.actions.FindActionMap("UI").Disable();
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        isPaused = false;
        pauseScreen.enabled = false;
    }
}
