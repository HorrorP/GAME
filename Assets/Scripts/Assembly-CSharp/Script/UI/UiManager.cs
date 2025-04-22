using UnityEngine;

namespace Script.UI
{
	public class UiManager : MonoBehaviour
	{
		public GameObject HowToPlayView;

		public GameObject LoadingView;

		public GameObject YouWinView;

		public ScreenFade ScreenFade;

		public AudioSource BackgroundMusic;

		public float FadeOutSeconds = 1f;

		public static UiManager Instance => GameBootstrapper.Instance.UiManager;

		public void HideLoading()
		{
			LoadingView.SetActive(value: false);
		}

		public void ShowYouWin()
		{
			YouWinView.SetActive(value: true);
		}

		public void ShowHowToPlay()
		{
			Debug.Log("ShowHowToPlay");
			HowToPlayView.SetActive(value: true);
		}

		public void HideHowToPlay()
		{
			Debug.Log("HideHowToPlay");
			HowToPlayView.SetActive(value: false);
		}

		public void FadeOut()
		{
			ScreenFade.FadeOut(FadeOutSeconds);
		}

		public void FadeIn(float FadeIn)
		{
			ScreenFade.FadeIn(FadeIn);
		}

		public void PlayBackgroundMusic()
		{
			BackgroundMusic.Play();
		}

		public void StopBackgroundMusic()
		{
			BackgroundMusic.Stop();
			BackgroundMusic.time = 0f;
		}
	}
}
