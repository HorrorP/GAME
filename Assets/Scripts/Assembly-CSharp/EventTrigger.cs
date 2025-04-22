using System;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
	public event Action<Collider> TriggerEnter;

	public event Action<Collider> TriggerExit;

	private void OnTriggerEnter(Collider other)
	{
		this.TriggerEnter?.Invoke(other);
	}

	private void OnTriggerExit(Collider other)
	{
		this.TriggerExit?.Invoke(other);
	}
}
