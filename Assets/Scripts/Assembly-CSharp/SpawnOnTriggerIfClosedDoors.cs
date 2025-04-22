using UnityEngine;

public class SpawnOnTriggerIfClosedDoors : MonoBehaviour
{
	public GameObject GameObject;

	public Transform Position;

	public DoorOpeningEvent LeftDoor;

	public DoorOpeningEvent RightDoor;

	private bool _isTriggered;

	private void OnTriggerEnter(Collider other)
	{
		if (!_isTriggered)
		{
			_isTriggered = true;
			if (!LeftDoor.IsProcessed || !RightDoor.IsProcessed)
			{
				Object.Instantiate(GameObject, Position);
			}
		}
	}
}
