using UnityEngine;

public class GameManager : ManagerBase<GameManager>
{
    public PlayerController Player;

    [Header("Decal Projector Settings")]
    [SerializeField] GameObject bloodDecalPrefab;
    [SerializeField] GameObject bloodOnWallDecalPrefab;
    [SerializeField] GameObject bulletHoleDecalPrefab;

    public void CreateDecalProjectorAtPoint(Transform hitTransform,Vector3 position, Vector3 normal, DecalType decalType)
    {
        GameObject decalPrefab = null;
        switch (decalType)
        {
            case DecalType.Blood:
                decalPrefab = bloodDecalPrefab;
                break;
            case DecalType.BulletHole:
                decalPrefab = bulletHoleDecalPrefab;
                break;
            case DecalType.BloodOnWall:
                decalPrefab = bloodOnWallDecalPrefab;
                break;
        }

        if (decalPrefab != null)
        {
            GameObject decalInstance = Instantiate(decalPrefab, position + normal * 0.01f, Quaternion.LookRotation(-normal), hitTransform);
        }
    }
}
