using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] Transform LevelObject;
    public BoxCollider LevelBounds;

    public Vector3 LevelPosition
	{
		get 
        { 
            return LevelBounds.transform.position; 
        }
        set 
        { 
            transform.position = value - LevelPositionOffset;
        }
	}

    [SerializeField] public Vector3 Extends;

    Vector3 LevelPositionOffset;

	void OnValidate()
	{
        Extends = LevelBounds.transform.localScale * 0.5f;
	}

	void Awake()
	{
		LevelObject.gameObject.SetActive(false);
        LevelPositionOffset = LevelBounds.transform.localPosition;
	}

    public void ToggleActive(bool isActive)
    {
        LevelObject.gameObject.SetActive(isActive);
    }

	void OnTriggerEnter(Collider other)
	{
        var character = other.GetComponentInParent<CharacterBase>();
		if (character != null)
        {
            character.transform.SetParent(LevelObject.transform);
        }
	}
}
