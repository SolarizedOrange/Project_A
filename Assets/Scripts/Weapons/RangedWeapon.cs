using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

class RaycastComparator : System.Collections.Generic.IComparer<RaycastHit>
{
    public int Compare(RaycastHit a, RaycastHit b)
    {
        return a.distance.CompareTo(b.distance);
    }
}

public abstract class RangedWeapon: WeaponBase
{
#region WeaponSettings
    [Header("Weapon Settings")]
    [HideInInspector] public Transform MuzzlePosition;
    [SerializeField] int maxPenetration = 3;
    [SerializeField] bool IsFullAuto = true;
    public bool IsReloading;
    protected int ammo;
    public int Ammo { get{ return ammo; }}
#endregion
#region Debug
    [Header("Test Ray Setting")]
    [SerializeField] Color debugRayColor = Color.red;
    [SerializeField] Color debugPointColor = Color.blue;
#endregion

    public override void InitWeapon(CharacterBase character)
    {
        base.InitWeapon(character);
        ammo = Stat.Capacity;
    }

    void OnDisable()
    {
        IsReloading = false;
    }

    public override bool Attack(bool hasJustAttacked)
    {
        if (CanAttack() && (IsFullAuto || hasJustAttacked))
        {
            Fire();
            IsReloading = false;
            lastAttackTime = Time.time;
            return true;
        }
        return false;
    }
    public void Fire()
    {
        ammo--;
        for (int i = 0; i < Stat.ShotCount; i++)
        {   
            Vector3 rayPos = 0.1f * (1.0f - Stat.Accuracy) * Random.insideUnitSphere;
            rayPos.z *= 0.5f;
            rayPos = (MuzzlePosition.forward + rayPos).normalized;
            Ray ray = new Ray(MuzzlePosition.position, rayPos);

            var comp = new RaycastComparator();
            var hits = new RaycastHit[maxPenetration];
            int hitCount = Physics.RaycastNonAlloc(ray, hits, Stat.AttackRange, (int)Layers.Default | (int)Layers.HitCollider);

            if (hitCount > 0)
            {
                Array.Sort(hits, 0, hitCount, comp);
                Debug.DrawRay(MuzzlePosition.position, rayPos * Stat.AttackRange, debugRayColor, 5.0f);
                for (int j = 0; j < hitCount; j++)
                {
                    var res = hits[j].collider;
                    Debug.DrawLine(
                        hits[j].point - Camera.main.transform.up * 0.5f,
                        hits[j].point + Camera.main.transform.up * 0.5f,
                        debugPointColor,
                        5.0f
                    );
                    Debug.DrawLine(
                        hits[j].point - Camera.main.transform.right * 0.5f,
                        hits[j].point + Camera.main.transform.right * 0.5f,
                        debugPointColor,
                        5.0f
                    );
                    if (res.TryGetComponent<HitBox>(out var hitBox))
                    {
                        // var buff = Character.GetWeaponBuffMul(GetWeaponType(), WeaponStatType.Damage);
                        hitBox.OnHit(Stat.Damage);
                    }
                }
            }

            var ragdollHits = new RaycastHit[maxPenetration];
            int ragdollHitCount = Physics.RaycastNonAlloc(ray, ragdollHits, Stat.AttackRange, (int)Layers.Default | (int)Layers.RagdollCollider);
            if (ragdollHitCount > 0)
            {
                Array.Sort(ragdollHits, 0, ragdollHitCount, comp);
                bool isHitCharacter = false;

                for (int j = 0; j < ragdollHitCount; j++)
                {
                    if ((1 << ragdollHits[j].collider.gameObject.layer) == (int)Layers.RagdollCollider)
                    {
                        ragdollHits[j].rigidbody.AddForce(MuzzlePosition.forward * Stat.Damage, ForceMode.Impulse);

                        isHitCharacter = true;
                        // create hit decal projector at point
                        GameManager.Instance.CreateDecalProjectorAtPoint(hits[j].transform,hits[j].point, hits[j].normal, DecalType.Blood);
                    }
                    // case: wall
                    else
                    {
                        GameManager.Instance.CreateDecalProjectorAtPoint(hits[j].transform,hits[j].point, hits[j].normal, isHitCharacter ? DecalType.BloodOnWall : DecalType.BulletHole);
                        break;
                    }
                }
            }
        }
    }
    public override bool CanAttack()
    {
        if (!IsReloading && ammo > 0 && ((Time.time - lastAttackTime) >= Stat.AttackRate))
        {
            return true;
        }
        return false;
    }
    public virtual void Reload(ObserverInt ammoInventory)
    {
        if (!IsReloading)
        {
            if (ammoInventory.Value > 0 && ammo < Stat.Capacity)
            {
                IsReloading = true;
                StartCoroutine(ReloadRoutine(ammoInventory));
            }
        }
    }

    protected virtual IEnumerator ReloadRoutine(ObserverInt ammoInventory)
    {
        yield return new WaitForSeconds(Stat.ReloadTime);
        ammoInventory.Value = ammoInventory.Value + ammo;
        ammo = Mathf.Min(Stat.Capacity, ammoInventory.Value);
        ammoInventory.Value = Mathf.Max(ammoInventory.Value - Stat.Capacity, 0);
        IsReloading = false;
    }
}
