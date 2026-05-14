using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MovingBox : MonoBehaviour
{
    [SerializeField] CharacterBase player;
    [SerializeField] Transform[] patrolPoint;
    [SerializeField] float stepSize = 1.0f;
    [SerializeField] float stepDuration = 0.3f;
    [SerializeField] float interval = 2.2f;
    [SerializeField] float arrivalDistance = 0.1f;
    [SerializeField] Ease returnEase = Ease.InOutSine;
    Vector3 originPos;
    Sequence stepSequence;
    
	void Start()
	{
		player = GameManager.Instance.Player;
		originPos = transform.position;
        StartStepping();
	}
    public bool CoverCheck(out float moveDir)
    {
        var res = player.IsCover;
        moveDir = (player.transform.position - transform.position).x > 0 ? -1f : 1f;
        return res;
    }



    public void StartStepping()
    {
        stepSequence?.Kill();
        stepSequence = DOTween.Sequence();

        stepSequence
            .AppendCallback(MoveStep)
            .AppendInterval(interval) 
            .SetLoops(-1);    
    }

    void MoveStep()
    {
        if (CoverCheck(out float moveDir) == false) return;
        var toPoint = patrolPoint.FirstOrDefault(point => (point.position - transform.position).x * moveDir > 0);
        Vector3 toTarget = toPoint ? toPoint.position - transform.position : Vector3.zero;
        float distance = toTarget.magnitude;

        if (distance <= arrivalDistance)
        {
            ReturnToOrigin();
            return;
        }

        float moveAmount = Mathf.Min(stepSize, distance);
        Vector3 nextPos = transform.position + toTarget.normalized * moveAmount;

        transform.DOKill(false);
        transform.DOMove(nextPos, stepDuration).SetEase(Ease.OutCubic);
    }

    void StopStepping()
    {
        stepSequence?.Kill();
        stepSequence = null;
    }

    public void ReturnToOrigin()
    {
        if (CoverCheck(out _)) return;

        StopStepping();
        transform.DOKill();

        float distanceToOrigin = Vector3.Distance(transform.position, originPos);
        float stepSpeed = stepSize / stepDuration;
        transform.DOMove(originPos, distanceToOrigin / stepSpeed).SetEase(returnEase);
    }
}
