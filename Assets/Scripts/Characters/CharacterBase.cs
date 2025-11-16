using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class CharacterBase : MonoBehaviour
{
    [Header("CharacterBase")]
    public CharacterController MoveCtrl;
    public WeaponBase CurrentWeapon;
    public bool IsCover;
    public float AttackDistance;

    protected virtual void Awake()
    {
        MoveCtrl = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
