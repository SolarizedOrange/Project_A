using System;
using System.Collections;
using UnityEngine;

public class PlayerCover: PlayerComponent
{
    [SerializeField] public float CoverEnterDelay = 3f;
    [SerializeField] public float CoverExitDelay = 3f;
    Vector3 originPos;
    Collider currentCover;
    Coroutine enterRoutine;
    Coroutine exitRoutine;
    bool isInTransition = false;
    readonly int CoverHash = Animator.StringToHash("IsCover");

    public void OnCover()
    {
        if ((object)enterRoutine == null && (object)exitRoutine == null)
        {
            if (PlayerCtrl.IsCover)
            {
                isInTransition = true;
                exitRoutine = StartCoroutine(ExitCoverRoutine(CoverExitDelay));
            }
            else
            {
                CheckCover();
            }
        }
    }

    public void CheckCover()
    {
        float minDist = float.MaxValue;
        Collider closestCover = null;
        Collider[] colliders;

        colliders = Physics.OverlapSphere(PlayerCtrl.transform.position, 3f, (int)Layers.PlayerCoverable);

        foreach (var cover in colliders)
        {
            float dist = Vector3.SqrMagnitude(PlayerCtrl.transform.position - cover.transform.position);

            if (minDist > dist)
            {
                closestCover = cover;
                originPos = cover.transform.position;
                originPos.z = 0;
                PlayerCtrl.MoveCtrl.SetTargetPositionXZ(originPos);
                // PlayerCtrl.MoveCtrl.SetTargetRotation(Vector3.right);
                minDist = dist;
            }
        }
        if ((object)closestCover == null)
        {
            Debug.Log("Fallback to Idle");
            return;
        }
        currentCover = closestCover;
        isInTransition = true;
        PlayerCtrl.IsCover = true;
        Debug.Log("Initiate Cover");
        // TODO: handle cover sqeuence with closestCover 
    }

    IEnumerator EnterCoverRoutine(float t)
    {
        yield return new WaitForSeconds(CoverEnterDelay);
        isInTransition = false;
        enterRoutine = null;
        Debug.Log("ENTER");
        
    }
    IEnumerator ExitCoverRoutine(float t)
    {
        PlayerCtrl.MoveCtrl.SetTargetPositionXZ(originPos);
        PlayerCtrl.Animator.SetBool(CoverHash, false);
        yield return new WaitForSeconds(CoverExitDelay);
        isInTransition = false;
        PlayerCtrl.IsCover = false;
        exitRoutine = null;
        Debug.Log("EXIT");
    }

    public void UpdateCover()
    {
        if (PlayerCtrl.IsCover && (object)exitRoutine == null)
        {
            if (Math.Abs((PlayerCtrl.transform.position - originPos).sqrMagnitude) > 0.2f)
            {
                if (PlayerCtrl.IsAiming)
                {
                    PlayerCtrl.Animator.SetBool(CoverHash, false);
                    PlayerCtrl.MoveCtrl.SetTargetPositionXZ(originPos);
                }

            }
            else
            {
                if (!PlayerCtrl.IsAiming)
                {
                    PlayerCtrl.MoveCtrl.SetTargetPositionXZ(currentCover.transform.position);
                    PlayerCtrl.Animator.SetBool(CoverHash, true);
                }
            }

            if(isInTransition && (object)enterRoutine == null)
            {
                enterRoutine = StartCoroutine(EnterCoverRoutine(CoverEnterDelay));
            }
        }
    }
}
