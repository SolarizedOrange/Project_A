using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] HitBoxType type;
    CharacterBase character;

    Quaternion originalRotation;
    void Awake()
    {
        character = GetComponentInParent<CharacterBase>();
        originalRotation = transform.rotation;
    }
    public void OnHit(float damage)
    {
        Debug.Log("HIT");
        character.OnDamage(type,damage);
    }

	void Update()
	{
        transform.rotation = originalRotation;
	}
}
