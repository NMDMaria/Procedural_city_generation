using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;
    public KeyCode activateKey = KeyCode.C;
    
    private bool isActivated = false;
    private Vector3 topDownPosition;
    private Quaternion topDownRotation;

    void Start()
    {
        // Store the top-down position and rotation of the camera
        topDownPosition = new Vector3(0f, 30f, 0f);
        topDownRotation = Quaternion.Euler(90f, 0f, 0f);
        
        // Set the initial camera position and rotation to top-down view
        transform.position = topDownPosition;
        transform.rotation = topDownRotation;
    }

    void Update()
    {
        // Check if the activate key is pressed
        if (Input.GetKeyDown(activateKey))
        {
            isActivated = !isActivated;

            if (isActivated)
            {
                // Set the camera to player mode
                transform.position = new Vector3(0f, 1f, 0f);
                transform.rotation = Quaternion.identity;
            }
            else
            {
                // Set the camera back to top-down view
                transform.position = topDownPosition;
                transform.rotation = topDownRotation;
            }
        }

        // Check if the camera controls are activated
        if (isActivated)
        {
            // Camera rotation
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            transform.Rotate(-mouseY, mouseX, 0f);

            // Camera movement
            float moveHorizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
            float moveVertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
            transform.Translate(moveHorizontal, 0f, moveVertical);
        }
    }
}
