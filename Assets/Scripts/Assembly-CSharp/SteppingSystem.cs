using System;
using Script;
using UnityEngine;

public class SteppingSystem : MonoBehaviour
{
	public enum StepState
	{
		Idle = 0,
		Preparing = 1,
		MainStep = 2,
		Ending = 3,
		Canceling = 4
	}

	public float StepDistance = 0.1f;

	public float StepTime = 1f;

	[Header("Distance ratios")]
	public float PrepareStepDistanceRatios = 0.1f;

	public float MainStepDistanceRatios = 0.85f;

	public float EndingStepDistanceRatios = 0.05f;

	[Header("Time ratios")]
	public float PrepareStepTimeRatios = 0.4f;

	public float MainStepTimeRatios = 0.5f;

	public float EndingStepTimeRatios = 0.1f;

	public AudioSource PrepareSound;

	public AudioSource MainSound;

	private StepState _currentState;

	private Vector3 _startPosition;

	private float _elapsedTime;

	private float _currentStepTime;

	public float SumStepDistanceRatios => PrepareStepDistanceRatios + MainStepDistanceRatios + EndingStepDistanceRatios;

	public float SumStepTimeRatios => PrepareStepTimeRatios + MainStepTimeRatios + EndingStepTimeRatios;

	public float PrepareStepDistance => StepDistance * (PrepareStepDistanceRatios / SumStepDistanceRatios);

	public float MainStepDistance => StepDistance * (MainStepDistanceRatios / SumStepDistanceRatios);

	public float EndingStepDistance => StepDistance * (EndingStepDistanceRatios / SumStepDistanceRatios);

	public float PrepareStepTime => StepTime * (PrepareStepTimeRatios / SumStepTimeRatios);

	public float MainStepTime => StepTime * (MainStepTimeRatios / SumStepTimeRatios);

	public float EndingStepTime => StepTime * (EndingStepTimeRatios / SumStepTimeRatios);

	public StepState CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			if (_currentState != value)
			{
				_currentState = value;
				switch (_currentState)
				{
				case StepState.Preparing:
					PrepareSound.PlayWithRandomPitch(0.75f, 1.2f);
					break;
				case StepState.MainStep:
					MainSound.PlayWithRandomPitch(0.75f, 1.2f);
					break;
				}
				this.StepStateChanged?.Invoke();
			}
		}
	}

	public event Action StepStateChanged;

	private void Start()
	{
		_startPosition = base.transform.position;
	}

	private void Update()
	{
		switch (CurrentState)
		{
		case StepState.Preparing:
			PerformMovement(_startPosition, _startPosition + base.transform.forward * PrepareStepDistance, PrepareStepTime, StepState.MainStep);
			break;
		case StepState.MainStep:
			PerformMovement(_startPosition + base.transform.forward * PrepareStepDistance, _startPosition + base.transform.forward * (PrepareStepDistance + MainStepDistance), MainStepTime, StepState.Ending);
			break;
		case StepState.Ending:
			PerformMovement(_startPosition + base.transform.forward * (PrepareStepDistance + MainStepDistance), _startPosition + base.transform.forward * (PrepareStepDistance + MainStepDistance + EndingStepDistance), EndingStepTime, StepState.Idle);
			break;
		case StepState.Canceling:
			PerformMovement(base.transform.position, _startPosition, PrepareStepTime, StepState.Idle);
			break;
		case StepState.Idle:
			CurrentState = StepState.Idle;
			break;
		}
	}

	public void StartStep()
	{
		if (CurrentState == StepState.Idle)
		{
			CurrentState = StepState.Preparing;
			_startPosition = base.transform.position;
			_elapsedTime = 0f;
		}
	}

	public void ForceStop()
	{
		CurrentState = StepState.Idle;
	}

	public void CancelStep()
	{
	}

	private void PerformMovement(Vector3 start, Vector3 end, float duration, StepState nextState)
	{
		_elapsedTime += Time.deltaTime;
		float t = Mathf.Clamp01(_elapsedTime / duration);
		t = Mathf.SmoothStep(0f, 1f, t);
		base.transform.position = Vector3.Lerp(start, end, t);
		if (t >= 1f)
		{
			CurrentState = nextState;
			_elapsedTime = 0f;
		}
	}
}
