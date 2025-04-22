using System.Collections;
using Script;
using UnityEngine;

public class DoorOpeningEvent : MonoBehaviour
{
	public enum OpeningEventType
	{
		None = 0,
		KnockKnock = 1,
		RunningBro = 2,
		PlasticBag = 3
	}

	public Door Door;

	public OpeningEventType EventType;

	[Header("KnockKnock")]
	public EventTrigger KnockKnockTrigger;

	public Transform KnockKnockScreamerPosition;

	public GameObject KnockKnockScreamerPrefab;

	public float KnockKnockSoundDelayMin = 1f;

	public float KnockKnockSoundDelayMax = 6f;

	public AudioSource KnockKnockSound;

	[Header("Running Bro")]
	public Transform RunningBroPosition;

	public GameObject RunningBroPrefab;

	public Transform[] RunningBroTargets;

	[Header("Plastic Bag")]
	public Transform PlasticBagPosition;

	public GameObject PlasticBagPrefab;

	public bool IsProcessed
	{
		get
		{
			if (Door.IsLocked)
			{
				return true;
			}
			if (EventType == OpeningEventType.KnockKnock)
			{
				return true;
			}
			return Door.IsOpened;
		}
	}

	private void OnEnable()
	{
		Door.BeginOpening += OnBeginOpening;
		Door.EndOpening += OnEndOpening;
		KnockKnockTrigger.TriggerEnter += OnKnockKnockTriggerEnter;
	}

	private void OnDisable()
	{
		Door.BeginOpening -= OnBeginOpening;
		Door.EndOpening -= OnEndOpening;
		KnockKnockTrigger.TriggerEnter -= OnKnockKnockTriggerEnter;
	}

	private void OnKnockKnockTriggerEnter(Collider obj)
	{
		if (EventType == OpeningEventType.KnockKnock)
		{
			StartCoroutine(PlayKnockKnockSound());
		}
	}

	private IEnumerator PlayKnockKnockSound()
	{
		yield return new WaitForSeconds(Random.Range(KnockKnockSoundDelayMin, KnockKnockSoundDelayMax));
		KnockKnockSound.PlayWithRandomPitch(0.75f, 1.2f);
	}

	private void OnBeginOpening()
	{
		switch (EventType)
		{
		case OpeningEventType.KnockKnock:
			Object.Instantiate(KnockKnockScreamerPrefab, KnockKnockScreamerPosition);
			break;
		case OpeningEventType.RunningBro:
			Object.Instantiate(RunningBroPrefab, RunningBroPosition).GetComponent<WalkToTargets>().Targets = RunningBroTargets;
			break;
		case OpeningEventType.PlasticBag:
			Object.Instantiate(PlasticBagPrefab, PlasticBagPosition);
			break;
		}
	}

	private void OnEndOpening()
	{
	}
}
