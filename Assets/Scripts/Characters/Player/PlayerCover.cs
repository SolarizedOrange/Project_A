using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCover: PlayerComponent
{
    [SerializeField] public float CoverEnterDelay = 3f;
    [SerializeField] public float CoverExitDelay = 3f;
    Vector3 originPos;
    Collider currentCover;

    bool isInTransition = false;
    Coroutine enterCoroutine;
    Coroutine exitCoroutine;

    public void OnCover()
    {
        if (enterCoroutine == null && exitCoroutine == null)
        {
            if (PlayerCtrl.IsCover)
            {
                isInTransition = true;
                exitCoroutine = StartCoroutine(ExitCover(CoverExitDelay));
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
        if (closestCover == null)
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

    IEnumerator EnterCover(float t)
    {
        yield return new WaitForSeconds(CoverEnterDelay);
        isInTransition = false;
        enterCoroutine = null;
        Debug.Log("ENTER");
        
    }
    IEnumerator ExitCover(float t)
    {
        PlayerCtrl.MoveCtrl.SetTargetPositionXZ(originPos);
        PlayerCtrl.Animator.SetBool("IsCover", false);
        yield return new WaitForSeconds(CoverExitDelay);
        isInTransition = false;
        PlayerCtrl.IsCover = false;
        PlayerCtrl.Animator.SetLayerWeight(2, 0);
        exitCoroutine = null;
        Debug.Log("EXIT");
    }

    public void Update()
    {
        if (PlayerCtrl.IsCover && exitCoroutine == null)
        {
            if (PlayerCtrl.IsAiming)
            {
                PlayerCtrl.MoveCtrl.SetTargetPositionXZ(originPos);
                PlayerCtrl.Animator.SetBool("IsCover", false);
                PlayerCtrl.Animator.SetLayerWeight(2, 0);
            }
            else if(Math.Abs((PlayerCtrl.transform.position - originPos).sqrMagnitude) < 0.2f)
            {
                PlayerCtrl.MoveCtrl.SetTargetPositionXZ(currentCover.transform.position);
                PlayerCtrl.Animator.SetBool("IsCover", true);
                PlayerCtrl.Animator.SetLayerWeight(2, 1);
            }
            if(isInTransition && enterCoroutine == null)
            {
                enterCoroutine = StartCoroutine(EnterCover(CoverEnterDelay));
            }
        }
    }

    // IEnumerator WaitTimer(float t)
    // {
    //     exitCover = false;
    //     enterCover = false;
    //     yield return new WaitForSeconds(CoverExitDelay);
    //     PlayerCtrl.IsCover = false;
    //     Debug.Log("EXIT");
    // }

}
