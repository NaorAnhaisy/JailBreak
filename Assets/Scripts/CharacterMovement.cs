using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class CharacterMovement : MonoBehaviour
{
    private LifeBarManager lifeBarManagerInstance;
    private GunManager gunManagerInstance;
    CharacterController controller;
    public Animator animator;
    public Transform camera1Transform;
    public Transform camera2Transform;
    private Transform activeCameraTransform;
    public float playerSpeed = 5;

    public GameObject explosionPrefab;
    public float explosionIntensityMultiplier = 1.5f;

    public float mouseSensivity = 3;
    Vector2 look;

    bool wasWalking = false;
    Vector3 velocity;
    float mass = 1f;
    public float jumpSpeed = 5f;

    public AudioClip keyCollectSound;
    public AudioClip mineSound;
    public bool hasCollectedKeys = false;
    public float delayBeforeSceneLoad = 1f;

    public GameObject rifle;
    public GameObject pistol;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            CollectKey(other.gameObject);
        }

        if (other.CompareTag("FirstAid"))
        {
            AudioSource.PlayClipAtPoint(keyCollectSound, transform.position);
            lifeBarManagerInstance.UpdateLife(20);
            other.gameObject.SetActive(false);
            Destroy(other);
        }

        if (other.CompareTag("Boat"))
        {
            Debug.Log("Found the boat");
            AudioSource.PlayClipAtPoint(keyCollectSound, transform.position);
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(8);
        }

        if (other.CompareTag("Rifle") || other.CompareTag("Pistol"))
        {
            AudioSource.PlayClipAtPoint(keyCollectSound, transform.position);
            gunManagerInstance.UpdateSelectedGun(other.gameObject); // Store the selected gun
            Debug.Log("Selected gun: " + gunManagerInstance.selectedGun.tag);
            Cursor.lockState = CursorLockMode.None;

            // Mark GunManager as not destroyable during scene change
            DontDestroyOnLoad(GunManager.Instance);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (other.CompareTag("Mine"))
        {

            // Calculate explosion intensity based on the noise value
            float explosionIntensity = 0.5f * explosionIntensityMultiplier;

            // Play the destroy sound
            AudioSource.PlayClipAtPoint(mineSound, transform.position);

            // Trigger explosion
            TriggerExplosion(explosionIntensity);

            // Destroy the mine GameObject
            Destroy(other.gameObject);

            // update life
            lifeBarManagerInstance.UpdateLife(-50);
        }
    }

    private void TriggerExplosion(float intensity)
    {
        // Instantiate the explosion prefab at the player's position
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Adjust explosion properties based on intensity
        ParticleSystem particleSystem = explosion.GetComponent<ParticleSystem>();
        var mainModule = particleSystem.main;
        mainModule.startSizeMultiplier *= intensity;
        mainModule.startSpeedMultiplier *= intensity;

        // Destroy the explosion effect after its duration
        Destroy(explosion, mainModule.duration);
    }

    private void CollectKey(GameObject key)
    {
        // Play the key collection sound
        AudioSource.PlayClipAtPoint(keyCollectSound, transform.position);

        // Remove the key from the scene
        Destroy(key);

        // Set the flag indicating that keys have been collected
        hasCollectedKeys = true;

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;

        // Start a coroutine to delay before loading the scene
        StartCoroutine(LoadSceneWithDelay());
    }

    private System.Collections.IEnumerator LoadSceneWithDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        // hide life bar
        LifeBarManager.Instance.gameObject.SetActive(false);

        // Load the scene after the delay
        SceneManager.LoadScene(3);
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        gunManagerInstance = GunManager.Instance;
        lifeBarManagerInstance = LifeBarManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;

        // Check if character is grounded in the start
        if (!controller.isGrounded)
        {
            // Cast a ray downwards to find the ground
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                // Set character position to the ground position
                transform.position = hit.point;
            }
        }

        activeCameraTransform = camera1Transform;
        camera1Transform.gameObject.SetActive(true);
        camera2Transform.gameObject.SetActive(false);
    }

    void Update()
    {
        UpdateLook();
        UpdateMovement();
        UpdateGravity();
        UpdateActiveCamera();

        if (gunManagerInstance.selectedGun != null)
        {
            Debug.Log("Retrieved selected gun: " + gunManagerInstance.selectedGun.tag);

            if (gunManagerInstance.selectedGun.CompareTag("Rifle"))
            {
                rifle.SetActive(true);
                pistol.SetActive(false);
            }
            else if (gunManagerInstance.selectedGun.CompareTag("Pistol"))
            {
                pistol.SetActive(true);
                rifle.SetActive(false);
            }
        }
        else
        {
            Debug.Log("No selected gun found");
        }
    }

    void UpdateLook()
    {
        look.x += Input.GetAxis("Mouse X") * mouseSensivity;
        look.y += Input.GetAxis("Mouse Y") * mouseSensivity;
        look.y = Mathf.Clamp(look.y, -90, 90);
        activeCameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);
    }

    void UpdateMovement()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var input = new Vector3();
        input += transform.forward * z;
        input += transform.right * x;
        input = Vector3.ClampMagnitude(input, 1f);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y += jumpSpeed;
        }

        // Calculate the potential next position
        Vector3 nextPosition = transform.position + (input * playerSpeed + velocity) * Time.deltaTime;

        // Check if the next position is not on the sea terrain
        RaycastHit hit;
        if (Physics.Raycast(nextPosition + Vector3.up * 0.1f, Vector3.down, out hit))
        {
            // Move the character
            controller.Move((input * playerSpeed + velocity) * Time.deltaTime);

            if (!wasWalking && input.magnitude > 0.1f)
            {
                wasWalking = true;
                animator.SetBool("isWalking", true);
            }
            else if (wasWalking && input.magnitude <= 0.1f)
            {
                wasWalking = false;
                animator.SetBool("isWalking", false);
            }
        }
    }

    void UpdateGravity()
    {
        var gravity = Physics.gravity * mass * Time.deltaTime;
        velocity.y = controller.isGrounded ? -1 : velocity.y + gravity.y;

        // Prevent falling through gaps by adjusting vertical position
        if (!controller.isGrounded && velocity.y < 0f)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, 1.2f))
            {
                if (groundHit.collider.gameObject.layer != LayerMask.NameToLayer("Sea"))
                {
                    transform.position = new Vector3(transform.position.x, groundHit.point.y, transform.position.z);
                }
            }
        }
    }

    void UpdateActiveCamera()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Switch the active camera
            if (activeCameraTransform == camera1Transform)
            {
                camera1Transform.gameObject.SetActive(false);
                camera2Transform.gameObject.SetActive(true);
                activeCameraTransform = camera2Transform;
            }
            else
            {
                camera1Transform.gameObject.SetActive(true);
                camera2Transform.gameObject.SetActive(false);
                activeCameraTransform = camera1Transform;
            }
        }
    }
}
