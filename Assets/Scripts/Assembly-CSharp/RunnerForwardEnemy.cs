using UnityEngine;

public class RunnerForwardEnemy : MonoBehaviour
{
	public float Speed;

	private void Update()
	{
		base.transform.position += base.transform.forward * (Speed * Time.deltaTime);
	}
}
