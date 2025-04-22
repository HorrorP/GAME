using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
	private InputSystem _inputSystem;

	public static InputManager Instance => GameBootstrapper.Instance.InputManager;

	public event Action<Vector2> LookPerformed;

	public event Action UsePerformed;

	public event Action UseCanceled;

	private void OnEnable()
	{
		if (_inputSystem == null)
		{
			_inputSystem = new InputSystem();
		}
		_inputSystem.Enable();
		_inputSystem.Player.Look.performed += OnLookPerformed;
		_inputSystem.Player.Use.performed += OnUsePerformed;
		_inputSystem.Player.Use.canceled += OnUseCanceled;
		_inputSystem.Player.Exit.performed += OnExit;
		Cursor.visible = false;
	}

	private void OnDisable()
	{
		_inputSystem.Disable();
		_inputSystem.Player.Look.performed -= OnLookPerformed;
		_inputSystem.Player.Use.performed -= OnUsePerformed;
		_inputSystem.Player.Use.canceled -= OnUseCanceled;
		_inputSystem.Player.Exit.performed -= OnExit;
	}

	private void OnUsePerformed(InputAction.CallbackContext obj)
	{
		this.UsePerformed?.Invoke();
	}

	private void OnUseCanceled(InputAction.CallbackContext obj)
	{
		this.UseCanceled?.Invoke();
	}

	private void OnLookPerformed(InputAction.CallbackContext context)
	{
		this.LookPerformed?.Invoke(context.ReadValue<Vector2>());
	}

	private void OnExit(InputAction.CallbackContext obj)
	{
		Application.Quit();
	}
}
