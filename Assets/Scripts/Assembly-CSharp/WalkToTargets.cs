using UnityEngine;

public class WalkToTargets : MonoBehaviour
{
	public float Speed = 3f;

	public Transform[] Targets;

	private int _currentTarget;

	private void Update()
	{
		base.transform.position = Vector3.MoveTowards(base.transform.position, Targets[_currentTarget].position, Speed * Time.deltaTime);
		if (!(Vector3.Distance(base.transform.position, Targets[_currentTarget].position) >= 0.01f))
		{
			if (Targets.Length > _currentTarget + 1)
			{
				_currentTarget++;
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
