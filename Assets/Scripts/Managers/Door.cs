using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
	public Vector3 ClosePosition;
	public Vector3 OpenPosition;
	public void MoveDoor(bool isOpen)
	{
		transform.DOKill();
		if (isOpen)
		{
			transform.DOLocalMove(OpenPosition, 1f);
		}
		else
		{
			transform.DOLocalMove(ClosePosition, 1f);
		}
	}
}