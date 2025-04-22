using System.Collections;
using UnityEngine;

public class LoopRotateWhileAudioPlay : MonoBehaviour
{
	public AudioSource AudioSource;

	public float RotationSpeed = 10f;

	public float RotationAngle = 30f;

	public float TimeToTalk = 1.5f;

	public float ReturnDuration = 0.2f;

	private Quaternion _initialRotation;

	private Coroutine _rotationCoroutine;

	private void Start()
	{
		_initialRotation = base.transform.localRotation;
	}

	public IEnumerator PlayWithRotationCoroutine()
	{
		AudioSource.Play();
		float timer = 0f;
		while (TimeToTalk > timer)
		{
			float x = Mathf.Abs(Mathf.Sin(timer * RotationSpeed) * RotationAngle);
			base.transform.localRotation = _initialRotation * Quaternion.Euler(x, 0f, 0f);
			timer += Time.deltaTime;
			yield return null;
		}
		Quaternion currentRotation = base.transform.localRotation;
		timer = 0f;
		while (timer < ReturnDuration)
		{
			timer += Time.deltaTime;
			base.transform.localRotation = Quaternion.Slerp(currentRotation, _initialRotation, timer / ReturnDuration);
			yield return null;
		}
		base.transform.localRotation = _initialRotation;
	}
}
