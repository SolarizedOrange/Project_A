using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] Transform LevelObject;
    public Collider LevelBounds;

    public Vector3 LevelPosition
	{
		get 
        { 
            return LevelBounds.transform.position; 
        }
        set 
        { 
            LevelBounds.transform.position = value;
            LevelObject.position = value + LevelPositionOffset; 
        }
	}

    Vector3 LevelPositionOffset;

	void Awake()
	{
		LevelObject.gameObject.SetActive(false);
        LevelPositionOffset = LevelObject.position - LevelBounds.transform.position;
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
