using System;
using System.Collections;
using Script;
using UnityEngine;

public class Door : MonoBehaviour
{
	public Transform RotationPivot;

	public Transform HandleRotationPivot;

	public float AccelerationDuration = 1f;

	public float ClosedAccelerationDuration = 0.1f;

	[Header("Handle")]
	public float HandleStartAcceleration = 0.5f;

	public float HandleEndAcceleration = 0.15f;

	public float HandleAngle = 60f;

	public int ClosedDoorRotation = 7;

	public bool IsLocked;

	[Header("Audio")]
	public AudioSource UseHandleAudio;

	public AudioSource OpenDoorAudio;

	public AudioSource ClosedDoorAudio;

	private bool _isInteracted;

	public bool IsOpened { get; private set; }

	public event Action BeginOpening;

	public event Action EndOpening;

	public void Open(Action onFinish = null)
	{
		if (_isInteracted)
		{
			onFinish?.Invoke();
			return;
		}
		_isInteracted = true;
		StartCoroutine(DoorAnimation(onFinish));
	}

	private IEnumerator DoorAnimation(Action onFinish)
	{
		this.BeginOpening?.Invoke();
		UseHandleAudio.PlayWithRandomPitch();
		Quaternion startHandle = HandleRotationPivot.transform.rotation;
		Quaternion endHandle = startHandle * Quaternion.Euler(0f - HandleAngle, 0f, 0f);
		yield return RotateObject(HandleRotationPivot, startHandle, endHandle, HandleStartAcceleration);
		yield return RotateObject(HandleRotationPivot, endHandle, startHandle, HandleEndAcceleration);
		if (IsLocked)
		{
			Quaternion startRotation = RotationPivot.transform.rotation;
			Quaternion openedRotation = startRotation * Quaternion.Euler(0f, ClosedDoorRotation, 0f);
			ClosedDoorAudio.PlayWithRandomPitch();
			yield return RotateObject(RotationPivot, startRotation, openedRotation, ClosedAccelerationDuration);
			yield return RotateObject(RotationPivot, openedRotation, startRotation, ClosedAccelerationDuration);
		}
		else
		{
			OpenDoorAudio.PlayWithRandomPitch();
			Quaternion rotation = RotationPivot.transform.rotation;
			Quaternion to = rotation * Quaternion.Euler(0f, 90f, 0f);
			yield return RotateObject(RotationPivot, rotation, to, AccelerationDuration);
			IsOpened = true;
		}
		onFinish?.Invoke();
		this.EndOpening?.Invoke();
	}

	private IEnumerator RotateObject(Transform target, Quaternion from, Quaternion to, float accelerationDuration)
	{
		float _timeElapsed = 0f;
		while (_timeElapsed < accelerationDuration)
		{
			_timeElapsed += Time.deltaTime;
			float t = Mathf.Pow(Mathf.Clamp01(_timeElapsed / accelerationDuration), 2f);
			target.transform.rotation = Quaternion.Lerp(from, to, t);
			yield return null;
		}
		target.transform.rotation = to;
	}
}
