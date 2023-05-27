using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;
    public KeyCode activateKey = KeyCode.C;
    public KeyCode jumpKey = KeyCode.Space;
    public PerlinGenerator perlinGenerator;
    public float fieldOfView = 50f; // Camera's field of view

    public Camera mainCamera; // Reference to the main camera
    public GameObject objectWithCamera; // Object with the camera component

    private bool isActivated = false;
    private Vector3 topDownPosition;
    private Quaternion topDownRotation;
    private int cityX;
    private int cityY;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float rotationX = 0f;

    void Start()
    {
        mainCamera.enabled = true;
        objectWithCamera.GetComponent<Camera>().enabled = false;


        cityX = perlinGenerator.textureX;
        cityY = perlinGenerator.textureY;
        float maxCityDimension = Mathf.Max(3 * cityX, 3 * cityY);
        float cameraHeight = maxCityDimension / (2f * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad));

        topDownPosition = new Vector3(3 * cityX / 2, cameraHeight, 3 * cityY / 2);

        // Store the top-down position and rotation of the camera
        topDownRotation = Quaternion.Euler(90f, 0f, 0f);

        // Set the initial camera position and rotation to top-down view
        mainCamera.transform.position = topDownPosition;
        mainCamera.transform.rotation = topDownRotation;

        // Store the initial camera position
        initialPosition = new Vector3(0f, 1f, 0f);
        initialRotation = Quaternion.identity;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            // Reset the camera position to the initial position
            objectWithCamera.transform.position = initialPosition;

            cityX = perlinGenerator.textureX;
            cityY = perlinGenerator.textureY;
            float maxCityDimension = Mathf.Max(3 * cityX, 3 * cityY);
            float cameraHeight = maxCityDimension / (2f * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad));

            topDownPosition = new Vector3(3 * cityX / 2, cameraHeight, 3 * cityY / 2);

            if (!isActivated)
            {
                mainCamera.transform.position = topDownPosition;
                mainCamera.transform.rotation = topDownRotation;
            }
        }

        // Check if the activate key is pressed
        if (Input.GetKeyDown(activateKey))
        {
            isActivated = !isActivated;

            if (isActivated)
            {
                // Switch the camera to the one on the object
                mainCamera.enabled = false;
                objectWithCamera.GetComponent<Camera>().enabled = true;
            }
            else
            {
                // Switch back to the main camera
                mainCamera.enabled = true;
                objectWithCamera.GetComponent<Camera>().enabled = false;

                // Set the camera back to top-down view
                mainCamera.transform.position = topDownPosition;
                mainCamera.transform.rotation = topDownRotation;
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
            objectWithCamera.transform.Translate(moveDirection * movementSpeed * Time.deltaTime);

            // Rotate the camera
            float rotationX = Input.GetAxis("Mouse X");
            float rotationY = Input.GetAxis("Mouse Y");

            objectWithCamera.transform.Rotate(Vector3.up, rotationX * rotationSpeed * Time.deltaTime);
            objectWithCamera.transform.Rotate(Vector3.left, rotationY * rotationSpeed * Time.deltaTime);
        }
    }
}
