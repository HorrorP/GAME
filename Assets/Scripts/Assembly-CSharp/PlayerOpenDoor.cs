using System;
using UnityEngine;

public class PlayerOpenDoor : MonoBehaviour
{
	public float RayDistance = 3f;

	public Camera PlayerCamera;

	private Door _door;

	private LayerMask _doorLayer;

	public event Action BeginOpening;

	public event Action EndOpening;

	private void Awake()
	{
		_doorLayer = LayerMask.GetMask("Door");
	}

	private void OnEnable()
	{
		InputManager.Instance.UsePerformed += OnUse;
	}

	private void OnDisable()
	{
		InputManager.Instance.UsePerformed -= OnUse;
	}

	private void OnUse()
	{
		if (!Physics.Raycast(new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward), out var hitInfo, RayDistance, _doorLayer))
		{
			return;
		}
		_door = hitInfo.collider.GetComponent<Door>();
		if (!(_door == null))
		{
			this.BeginOpening?.Invoke();
			_door.Open(delegate
			{
				this.EndOpening?.Invoke();
			});
		}
	}
}
