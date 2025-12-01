using UnityEngine.SceneManagement;

public class SceneController: ManagerBase<SceneController>
{
	// Load Scene, Not Aync
    public void LoadScene(SceneItem item)
	{
		SceneManager.LoadScene(item.SceneName, item.IsAdditiveLoad? LoadSceneMode.Additive: LoadSceneMode.Single);
	}
}
