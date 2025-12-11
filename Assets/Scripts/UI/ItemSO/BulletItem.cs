using UnityEngine;

[CreateAssetMenu(fileName = "BulletItem",menuName ="Shop/BulletItem")]
public class BulletItem : ItemBase
{
    [Header("Bullet Info")]
	public WeaponType BulletType;
	public int AmmoPerTrade;
}
