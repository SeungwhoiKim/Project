using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float runSpeed = 5.5f;
    public float rotationSpeed = 5.0f;
    public float interactionDistance = 2.5f;
    public float interactionAngle = 45.0f;

    public float gravity = -9.81f;
    private float verticalVelocity = 0f;

    private CharacterController controller;
    private Camera mainCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        Move();
        RotateTowardsMouse();
        if (Input.GetMouseButtonDown(0))
        {
            Interact();
        }
    }

    /**
     * Method that makes player able to interact with objects that are interactable.
     */
    void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        Interactable closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                Vector3 directionToObject = (collider.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, directionToObject);
                if (angle <= interactionAngle)
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestInteractable = interactable;
                    }
                }
            }
        }

        if (closestInteractable != null)
        {
            closestInteractable.Interact();
        }
        else
        {
            Debug.Log("Too far to interact.");
        }
    }

    /**
     * Method that makes the object move using w, a, s, d key.
     */
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        Vector3 horizontalMove = move * speed;

        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        Vector3 verticalMove = Vector3.up * verticalVelocity;

        controller.Move((horizontalMove + verticalMove) * Time.deltaTime);
    }

    /**
     * Method to make the object look toward where the mouse pointer locates.
     */
    void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
