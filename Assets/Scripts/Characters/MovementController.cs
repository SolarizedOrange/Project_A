using UnityEngine;

public class MovementController: MonoBehaviour
{
    public CharacterController Ctrl;
    [SerializeField] float transitionTime = 0.2f;
    float moveTimer = 0f;
    float lookTimer = 0f;
    Vector3 lastVelocity;
    Vector3 currentVelocity;
    Vector3 targetVelocity;

    Vector3 lastRotation;
    Vector3 currentRotation;
    Vector3 targetRotation;
    public Vector3 TargetVelocity
    {
        get
        {
            return targetVelocity;
        }
        set
        {
            if (targetVelocity != value)
            {
                moveTimer = 0f;
                lastVelocity = currentVelocity;
            }
            targetVelocity = value;
        }
    }

    void Awake()
    {
        currentRotation = Vector3.right;
        targetRotation = currentRotation;
        lastRotation = currentRotation;
        lastVelocity = Vector3.zero;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer = Mathf.Clamp(moveTimer + Time.deltaTime, 0, transitionTime);
        lookTimer = Mathf.Clamp(lookTimer + Time.deltaTime, 0, transitionTime);
        currentVelocity = Vector3.Lerp(lastVelocity, targetVelocity, moveTimer / transitionTime);
        currentRotation = Vector3.Lerp(lastRotation, targetRotation, lookTimer / transitionTime);
        Ctrl.SimpleMove(currentVelocity);
        transform.LookAt(transform.position + new Vector3(currentRotation.x, 0f, -Mathf.Sqrt(1 - Vector3.SqrMagnitude(currentRotation))));
        // transform.rotate
    }

    public void UpdateRotation(Vector3 direction)
    {
        if (Vector3.Dot(direction, targetRotation) <= 0)
        {      
            lookTimer = 0f;
            lastRotation = currentRotation;
        }
        if (direction.magnitude > 0.1f)
        {
            targetRotation = direction;
        }
    }

}
