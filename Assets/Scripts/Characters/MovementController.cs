using System;
using UnityEngine;

public class MovementController: MonoBehaviour
{
    [Header("Movement Settings")]
    public Rigidbody Ctrl;
    [SerializeField] float velocityTransitionTime = 0.2f;
    [SerializeField] float rotateTransitionTime = 0.2f;
    [SerializeField] float positionTransitionTime = 0.2f;

    // float moveTimer = 0f;
    float lookTimer = 0f;
    // Vector3 lastVelocity;
    // Vector3 currentVelocity;
    Vector3 targetVelocity;
    Vector3 targetPosition;
    bool isVelocityMode = true;
    Vector3 lastRotation;
    Vector3 currentRotationDirection;
    Vector3 targetRotation;
    // public Vector3 TargetVelocity
    // {
    //     get
    //     {
    //         return targetVelocity;
    //     }
    //     set
    //     {
    //         if (targetVelocity != value)
    //         {
    //             moveTimer = 0f;
    //             lastVelocity = currentVelocity;
    //         }
    //         targetVelocity = value;
    //     }
    // }

    void Awake()
    {
        currentRotationDirection = Vector3.right;
        targetRotation = currentRotationDirection;
        lastRotation = currentRotationDirection;
        // lastVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity();
        UpdateRotation();
    }

    void UpdateVelocity()
	{
        // moveTimer = Mathf.Clamp(moveTimer + Time.deltaTime, 0, transitionTime);
        // currentVelocity = Vector3.Lerp(lastVelocity, targetVelocity, moveTimer / transitionTime);
        // Ctrl.linearVelocity = currentVelocity;
        var a = Vector3.zero;
        if (isVelocityMode)
		{
            a = Vector3.right * (targetVelocity.x - Ctrl.linearVelocity.x) / velocityTransitionTime;
		}
        else
		{
            var curPosXZ = Vector3.Scale(transform.position, new Vector3(1,0,1));
            Ctrl.constraints &= ~RigidbodyConstraints.FreezePositionZ;

            var reqVelocity = (targetPosition - curPosXZ) / positionTransitionTime;
            var velocityDiff = reqVelocity - Ctrl.linearVelocity;
            a = velocityDiff /  positionTransitionTime;

            if (Vector3.Distance(curPosXZ, targetPosition) < 0.1f)
			{
                Ctrl.constraints |= RigidbodyConstraints.FreezePositionZ;
				isVelocityMode = true;
			}
            SetTargetRotation(Vector3.Scale(targetPosition - curPosXZ,Vector3.right).normalized);
		}
        Ctrl.AddForce(a,ForceMode.Acceleration);
	}

    public void UpdateRotation()
	{
        lookTimer = Mathf.Clamp(lookTimer + Time.deltaTime, 0, rotateTransitionTime);
        currentRotationDirection = Vector3.Lerp(lastRotation, targetRotation, lookTimer / rotateTransitionTime);
        var targetLookPos = new Vector3(currentRotationDirection.x, 0f, -Mathf.Sqrt(1 - Vector3.SqrMagnitude(currentRotationDirection)));
        // transform.LookAt(transform.position + new Vector3(currentRotation.x, 0f, -Mathf.Sqrt(1 - Vector3.SqrMagnitude(currentRotation))));
        if (targetLookPos.sqrMagnitude > 0.01f)
		{
			var rot = Quaternion.LookRotation(targetLookPos);
            Ctrl.MoveRotation(rot);
		}
	}

    public void SetTargetRotation(Vector3 direction)
    {
        direction.x = Math.Sign(direction.x);

        if (Vector3.Dot(direction, targetRotation) < 0)
        {      
            lookTimer = 0f;
            lastRotation = currentRotationDirection;
        }
        if (direction.magnitude > 0.1f)
        {
            targetRotation = Vector3.Scale(direction, Vector3.right);
        }
    }

    public void SetTargetVelocity(Vector3 velocity)
	{
        targetVelocity = velocity;
    }

    public void SetTargetPositionXZ(Vector3 targetPos)
	{
		targetPosition = Vector3.Scale(targetPos,new Vector3(1,0,1));
        isVelocityMode = false;
	}
}
