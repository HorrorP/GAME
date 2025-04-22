using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
	public TextMeshProUGUI TextMesh;

	public string LoadingText = "Loading";

	private Coroutine _loadingCoroutine;

	private void OnEnable()
	{
		_loadingCoroutine = StartCoroutine(MakeAnimation());
		IEnumerator MakeAnimation()
		{
			string[] loadingTexts = new string[3]
			{
				LoadingText + ".",
				LoadingText + "..",
				LoadingText + "..."
			};
			int currentIndex = 0;
			while (true)
			{
				yield return new WaitForSeconds(0.5f);
				TextMesh.text = loadingTexts[currentIndex];
				currentIndex++;
				if (currentIndex >= loadingTexts.Length)
				{
					currentIndex = 0;
				}
			}
		}
	}

	private void OnDisable()
	{
		StopCoroutine(_loadingCoroutine);
	}
}
