using System;
using System.Collections;
using UnityEngine;

public class PlayerDirectionState : MonoBehaviour
{
	public enum DirectionState
	{
		Forward = 0,
		Back = 1,
		Left = 2,
		Right = 3
	}

	public delegate void BeginRotatingEvent(DirectionState from, DirectionState to);

	public delegate void EndRotatingEvent(DirectionState updatedState);

	public float RotationSpeed = 2f;

	private Coroutine _rotationCoroutine;

	private Quaternion _forwardRotation;

	public DirectionState Direction { get; private set; }

	public event BeginRotatingEvent BeginRotating;

	public event EndRotatingEvent EndRotating;

	private void StartRotating(DirectionState targetDirection)
	{
		if (Direction != targetDirection)
		{
			if (_rotationCoroutine != null)
			{
				StopCoroutine(_rotationCoroutine);
				this.EndRotating?.Invoke(Direction);
				_rotationCoroutine = null;
			}
			_rotationCoroutine = StartCoroutine(RotateToTarget());
		}
		IEnumerator RotateToTarget()
		{
			this.BeginRotating?.Invoke(Direction, targetDirection);
			Quaternion targetRotation = targetDirection switch
			{
				DirectionState.Forward => _forwardRotation, 
				DirectionState.Back => _forwardRotation * Quaternion.Euler(0f, 180f, 0f), 
				DirectionState.Left => _forwardRotation * Quaternion.Euler(0f, -90f, 0f), 
				DirectionState.Right => _forwardRotation * Quaternion.Euler(0f, 90f, 0f), 
				_ => throw new ArgumentOutOfRangeException(), 
			};
			while (targetRotation != base.transform.rotation)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
				yield return null;
			}
			_rotationCoroutine = null;
			Direction = targetDirection;
			this.EndRotating?.Invoke(Direction);
		}
	}

	private void Awake()
	{
		_forwardRotation = base.transform.rotation;
	}

	private void OnEnable()
	{
		InputManager.Instance.LookPerformed += OnLook;
		_rotationCoroutine = null;
	}

	private void OnDisable()
	{
		InputManager.Instance.LookPerformed -= OnLook;
	}

	private void OnLook(Vector2 input)
	{
		if (_rotationCoroutine == null)
		{
			if (input.x >= 0.5f)
			{
				StartRotating(DirectionState.Right);
			}
			else if (input.x <= -0.5f)
			{
				StartRotating(DirectionState.Left);
			}
			else if (input.y >= 0.5f)
			{
				StartRotating(DirectionState.Forward);
			}
			else if (input.y <= -0.5f)
			{
				StartRotating(DirectionState.Back);
			}
		}
	}
}
