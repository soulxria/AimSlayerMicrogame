using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameData gameData;
    private float xRotation;
    private float sensitivity;
    public Transform playerBody;
    public Transform camTransform;

    private PlayerInput controls;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        sensitivity = gameData.getSens();
    }

   public void OnMouseX(InputAction.CallbackContext context)
    {
        float mouseX = context.ReadValue<float>() * sensitivity * Time.deltaTime;
        //playerBody.Rotate(Vector3.up * mouseX);
        transform.Rotate(0f, mouseX, 0f);
    }

    public void OnMouseY(InputAction.CallbackContext context)
    {
        float mouseY = context.ReadValue<float>() * (-1*sensitivity) * Time.deltaTime;
        Vector3 newRot = camTransform.rotation.eulerAngles + new Vector3(mouseY, 0f, 0f);
        camTransform.rotation = (Quaternion.Euler(newRot.x, newRot.y , newRot.z));
    }   

    public void ShootProjectile(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.SphereCast(camTransform.position, 10f, camTransform.forward, out hit, 100f))
        {
            if (hit.collider.gameObject.CompareTag("Target"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
