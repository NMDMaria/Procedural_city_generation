using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;
    public KeyCode activateKey = KeyCode.C;
    public KeyCode jumpKey = KeyCode.Space;
    public PerlinGenerator perlinGenerator;
    public float fieldOfView = 50f; // Camera's field of view

    private bool isActivated = false;
    private Vector3 topDownPosition;
    private Quaternion topDownRotation;
    private int cityX;
    private int cityY;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        cityX = perlinGenerator.textureX;
        cityY = perlinGenerator.textureY;
        float maxCityDimension = Mathf.Max(3 * cityX, 3 * cityY);
        float cameraHeight = maxCityDimension / (2f * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad));

        topDownPosition = new Vector3(3 * cityX / 2, cameraHeight, 3 * cityY / 2);

        // Store the top-down position and rotation of the camera
        topDownRotation = Quaternion.Euler(90f, 0f, 0f);

        // Set the initial camera position and rotation to top-down view
        transform.position = topDownPosition;
        transform.rotation = topDownRotation;

        // Store the initial camera position
        initialPosition = new Vector3(0f, 1f, 0f);
        initialRotation = Quaternion.identity;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            // Reset the camera position to the initial position
            transform.position = initialPosition;

            cityX = perlinGenerator.textureX;
            cityY = perlinGenerator.textureY;
            float maxCityDimension = Mathf.Max(3 * cityX, 3 * cityY);
            float cameraHeight = maxCityDimension / (2f * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad));

            topDownPosition = new Vector3(3 * cityX / 2, cameraHeight, 3 * cityY / 2);

            if (!isActivated)
            {
                transform.position = topDownPosition;
                transform.rotation = topDownRotation;
            }
        }

        // Check if the activate key is pressed
        if (Input.GetKeyDown(activateKey))
        {
            isActivated = !isActivated;

            if (isActivated)
            {
                // Set the camera to player mode
                transform.position = initialPosition;
                transform.rotation = initialRotation;
            }
            else
            {
                initialPosition = transform.position;
                initialRotation = transform.rotation;
                // Set the camera back to top-down view
                transform.position = topDownPosition;
                transform.rotation = topDownRotation;
            }

            // Lock or unlock the cursor based on isActivated
            Cursor.lockState = isActivated ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isActivated;
        }

        // Check if the camera controls are activated
        if (isActivated)
        {
            // Move the camera
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical);
            transform.Translate(moveDirection * movementSpeed * Time.deltaTime);

            // Rotate the camera
            float rotationX = Input.GetAxis("Mouse X");
            float rotationY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, rotationX * rotationSpeed * Time.deltaTime);
            transform.Rotate(Vector3.left, rotationY * rotationSpeed * Time.deltaTime);
        }
    }
}
