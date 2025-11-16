using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] HitBoxType type;
    CharacterBase character;

    void Awake()
    {
        character = GetComponentInParent<CharacterBase>();
    }
    public void OnHit()
    {
        Debug.Log("HIT");
        character.OnDamage(type);
    }
}
