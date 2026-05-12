using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] Transform LevelObject;
    [SerializeField] bool isActiveOnStart = false;
    public BoxCollider LevelBounds;


    Vector3 levelCenter;
    // z axis is not center, z is zPivot.
    public Vector3 LevelCenter
	{
		get 
        { 
            return levelCenter;
        }
        set 
        { 
            var cur = LevelBounds.transform.position + LevelBounds.center;
            cur.z = transform.position.z + ZPivot.localPosition.z;

            var dif = value - cur;
            transform.position += dif;

            levelCenter = value;
        }
	}

    public Transform ZPivot;

    public Vector3 Extends;

	void Awake()
	{
		LevelObject.gameObject.SetActive(isActiveOnStart);
	}

    public void ToggleActive(bool isActive)
    {
        LevelObject.gameObject.SetActive(isActive);
    }

	[ContextMenu("Calculate Level Size")]
    public void CalculateLevelSize()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        if (renderers.Length == 0) return;

        Bounds combinedBounds = renderers[0].bounds;

        foreach (Renderer render in renderers)
        {
            combinedBounds.Encapsulate(render.bounds);
        }
        LevelBounds.transform.position = combinedBounds.center;
        LevelBounds.size = combinedBounds.size;
        Extends = combinedBounds.size * 0.5f;

        #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            if (LevelBounds != null) EditorUtility.SetDirty(LevelBounds);

            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (stage != null)
            {
                EditorSceneManager.MarkSceneDirty(stage.scene);
            }
        #endif
    }
}
