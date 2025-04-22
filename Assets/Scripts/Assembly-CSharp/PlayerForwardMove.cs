using System;
using Script.Rail;
using UnityEngine;

public class PlayerForwardMove : MonoBehaviour
{
	public float RailEventActivationDistance = 3f;

	public Camera PlayerCamera;

	public SteppingSystem SteppingSystem;

	private LayerMask _doorLayer;

	private IRailEvent _currentRailEvent;

	private bool _havePlayerInput;

	public event Action BeginRailEvent;

	public event Action EndRailEvent;

	private void Awake()
	{
		_doorLayer = LayerMask.GetMask("Door");
	}

	private void OnEnable()
	{
		InputManager.Instance.UsePerformed += OnUse;
		InputManager.Instance.UseCanceled += OnStopUse;
	}

	private void OnDisable()
	{
		InputManager.Instance.UsePerformed -= OnUse;
		InputManager.Instance.UseCanceled -= OnStopUse;
		_havePlayerInput = false;
		(_currentRailEvent as ICancelableRailEvent)?.RequestCancel();
	}

	private void Update()
	{
		if (_havePlayerInput && _currentRailEvent == null)
		{
			_currentRailEvent = FindNextGameObjectRailEvent() ?? new StepRailEvent(SteppingSystem);
			_currentRailEvent.Perform();
			this.BeginRailEvent?.Invoke();
		}
		else
		{
			(_currentRailEvent as ICancelableRailEvent)?.RequestCancel();
		}
		IRailEvent currentRailEvent = _currentRailEvent;
		if (currentRailEvent != null && currentRailEvent.IsPerformed)
		{
			this.EndRailEvent?.Invoke();
			(_currentRailEvent as IDisposable)?.Dispose();
			_currentRailEvent = null;
		}
	}

	private IRailEvent FindNextGameObjectRailEvent()
	{
		if (!Physics.Raycast(new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward), out var hitInfo, RailEventActivationDistance, _doorLayer))
		{
			return null;
		}
		IRailEvent component = hitInfo.collider.GetComponent<IRailEvent>();
		if (!component.IsPerformed)
		{
			return component;
		}
		return null;
	}

	private void OnUse()
	{
		_havePlayerInput = true;
	}

	private void OnStopUse()
	{
		_havePlayerInput = false;
	}
}
