using Script;
using UnityEngine;

public class CorridorGenerator : MonoBehaviour
{
	public GameObject CorridorBlockPrefab;

	public Transform StarBlockPosition;

	public Transform StarBlockEndPosition;

	public Transform EndRoomOffsetPoint;

	public GameObject EndRoomPrefab;

	public DoorNumber StartDoor;

	public CorridorBlock StartCorridorBlock;

	public PlayerEnableMediator Player;

	public DummyTimer DummyTimer;

	public int MinBlocksCount = 30;

	public int MaxBlocksCount = 50;

	public float BlockLength => StarBlockEndPosition.transform.position.z - StarBlockPosition.transform.position.z;

	private void Awake()
	{
		int num = Random.Range(MinBlocksCount, MaxBlocksCount);
		int num2 = num * 2 + 1;
		StartDoor.SetNumber(num2);
		StartCorridorBlock.RightDoor.SetNumber(num2 - 1);
		StartCorridorBlock.LeftDoor.SetNumber(num2 - 2);
		GenerateCorridorBoxes(num);
		GenerateEndRoom(num);
	}

	private void GenerateEndRoom(int blocksCount)
	{
		Vector3 position = StarBlockPosition.position;
		float num = EndRoomOffsetPoint.position.z - position.z;
		position.AddZ(BlockLength * (float)blocksCount + num);
		Object.Instantiate(EndRoomPrefab, position.AddZ(BlockLength * (float)blocksCount - num), Quaternion.Euler(0f, 180f, 0f)).GetComponent<YouWinCutsceneOnDoorOpen>().Init(Player, DummyTimer);
	}

	private void GenerateCorridorBoxes(int blocksCount)
	{
		for (int i = 1; i < blocksCount; i++)
		{
			CorridorBlock component = Object.Instantiate(CorridorBlockPrefab, StarBlockPosition.position.AddZ(BlockLength * (float)i), Quaternion.identity).GetComponent<CorridorBlock>();
			int num = (blocksCount - i) * 2 - 1;
			component.RightDoor.SetNumber(num);
			component.LeftDoor.SetNumber(num + 1);
			DoorOpeningEvent component2 = component.LeftDoor.GetComponent<DoorOpeningEvent>();
			DoorOpeningEvent component3 = component.RightDoor.GetComponent<DoorOpeningEvent>();
			if (i + 1 != blocksCount)
			{
				GenerateRandomEventDoorValues(component2);
				GenerateRandomEventDoorValues(component3);
			}
			else
			{
				component2.Door.IsLocked = true;
				component3.Door.IsLocked = true;
			}
		}
	}

	private static void GenerateRandomEventDoorValues(DoorOpeningEvent randomEventDoor)
	{
		int num = Random.Range(0, 11);
		if (num == 9)
		{
			randomEventDoor.Door.IsLocked = true;
			return;
		}
		if (num == 10)
		{
			randomEventDoor.EventType = DoorOpeningEvent.OpeningEventType.PlasticBag;
			return;
		}
		if (num >= 6)
		{
			num -= 3;
		}
		if (num >= 3)
		{
			num -= 3;
		}
		randomEventDoor.EventType = (DoorOpeningEvent.OpeningEventType)num;
	}
}
