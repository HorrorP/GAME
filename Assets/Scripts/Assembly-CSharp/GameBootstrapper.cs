using Script.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrapper : MonoBehaviour
{
	public UiManager UiManager;

	public InputManager InputManager;

	public static GameBootstrapper Instance { get; private set; }

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
		Instance = this;
	}

	private void Start()
	{
		SceneManager.LoadScene("Scenes/Game");
		UiManager.HideLoading();
		UiManager.ShowHowToPlay();
	}
}
