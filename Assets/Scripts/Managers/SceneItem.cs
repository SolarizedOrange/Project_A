using UnityEngine;

[CreateAssetMenu(fileName = "SceneItem")]
public class SceneItem : ScriptableObject
{
	// TODO: Change SceneName to Addressable Scene Reference
	public string SceneName;
	public bool IsAdditiveLoad = false;
}