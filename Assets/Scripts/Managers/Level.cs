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

    public Vector3 Extends;

    Vector3 LevelPositionOffset;

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

    [ContextMenu("Calculate Level Size")]
    public void CalculateLevelSize()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds combinedBounds = renderers[0].bounds;

        foreach (Renderer render in renderers)
        {
            combinedBounds.Encapsulate(render.bounds);
        }

        LevelBounds.size = combinedBounds.size;
        LevelBounds.transform.position = combinedBounds.center;
        Extends = Vector3.Scale(LevelBounds.transform.localScale, LevelBounds.size) * 0.5f;
    }
}
