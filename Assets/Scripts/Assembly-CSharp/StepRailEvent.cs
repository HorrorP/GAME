using System;
using Script.Rail;

public class StepRailEvent : ICancelableRailEvent, IRailEvent, IDisposable
{
	private readonly SteppingSystem _steppingSystem;

	public bool IsPerformed { get; private set; }

	public StepRailEvent(SteppingSystem steppingSystem)
	{
		_steppingSystem = steppingSystem;
		_steppingSystem.StepStateChanged += OnStepStateChanged;
	}

	public void Perform()
	{
		_steppingSystem.StartStep();
	}

	private void OnStepStateChanged()
	{
		if (_steppingSystem.CurrentState == SteppingSystem.StepState.Idle)
		{
			IsPerformed = true;
		}
	}

	public void RequestCancel()
	{
		_steppingSystem.CancelStep();
	}

	public void Dispose()
	{
		_steppingSystem.StepStateChanged -= OnStepStateChanged;
	}
}
