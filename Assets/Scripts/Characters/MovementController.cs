using UnityEngine;

public class MovementController: MonoBehaviour
{
    [Header("Movement Settings")]
    public Rigidbody Ctrl;
    [SerializeField] float velocityTransitionTime = 0.2f;
    public float VelocityTransitionTime { get{ return velocityTransitionTime; } }
    [SerializeField] float rotateTransitionTime = 0.2f;
    public float RotateTransitionTime { get{ return rotateTransitionTime; } }
    [SerializeField] float positionTransitionTime = 0.2f;
    public float PositionTransitionTime { get{ return positionTransitionTime; } }

    // float moveTimer = 0f;
    float lookTimer = 0f;
    // Vector3 lastVelocity;
    // Vector3 currentVelocity;
    Vector3 targetVelocity = Vector3.zero;
    Vector3 targetPosition = Vector3.zero;
    bool isVelocityMode = true;
    Vector3 lastRotation = Vector3.zero;
    Vector3 currentRotationDirection = Vector3.zero;
    Vector3 targetRotation = Vector3.zero;
    bool isAutoRotate = true;

    void FixedUpdate()
    {
        UpdateVelocity();
        UpdateRotation();
    }

    void UpdateVelocity()
	{
        var acceleration = Vector3.zero;
        if (isVelocityMode)
		{
            acceleration = Vector3.right * (targetVelocity.x - Ctrl.linearVelocity.x) / velocityTransitionTime;
		}
        else
		{
            var curPosXZ = Vector3.Scale(transform.position, new Vector3(1,0,1));
            Ctrl.constraints &= ~RigidbodyConstraints.FreezePositionZ;

            var reqVelocity = (targetPosition - curPosXZ) / positionTransitionTime;
            var velocityDiff = reqVelocity - Ctrl.linearVelocity;
            acceleration = velocityDiff /  positionTransitionTime;

            if (Vector3.Distance(curPosXZ, targetPosition) < 0.1f)
			{
                Ctrl.constraints |= RigidbodyConstraints.FreezePositionZ;
				isVelocityMode = true;
			}
            if (isAutoRotate)
                SetTargetRotation(Vector3.Scale(targetPosition - curPosXZ,Vector3.right).normalized);
		}
        Ctrl.AddForce(acceleration,ForceMode.Acceleration);
	}

    public void UpdateRotation()
	{
        if (lastRotation == Vector3.zero && currentRotationDirection == Vector3.zero && targetRotation == Vector3.zero) return;
        lookTimer = Mathf.Clamp(lookTimer + Time.fixedDeltaTime, 0, rotateTransitionTime);
        currentRotationDirection = Vector3.Lerp(lastRotation, targetRotation, lookTimer / rotateTransitionTime);
        var targetLookPos = new Vector3(currentRotationDirection.x, 0f, -Mathf.Sqrt(1 - Vector3.SqrMagnitude(currentRotationDirection)));

        if (targetLookPos.sqrMagnitude > 0.01f)
		{
			var rot = Quaternion.LookRotation(targetLookPos);
            Ctrl.MoveRotation(rot);
		}
	}

    // TODO: Check Logic
    public void SetTargetRotation(Vector3 direction)
    {
        direction.x = Mathf.Sign(direction.x);
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

    public void SetTargetPositionXZ(Vector3 targetPos, bool isAutoRotate = true)
	{
		targetPosition = Vector3.Scale(targetPos,new Vector3(1,0,1));
        isVelocityMode = false;
        this.isAutoRotate = isAutoRotate;
	}
}
