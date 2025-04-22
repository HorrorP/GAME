using UnityEngine;

public class GameRunner : MonoBehaviour
{
	public GameObject BootstrapperPrefab;

	private void Awake()
	{
		if (GameObject.FindWithTag("GameBootstrapper") == null)
		{
			Object.Instantiate(BootstrapperPrefab);
		}
	}
}
