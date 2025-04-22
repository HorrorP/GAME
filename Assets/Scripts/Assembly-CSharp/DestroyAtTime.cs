using System.Collections;
using UnityEngine;

public class DestroyAtTime : MonoBehaviour
{
	public float TimeToDestroy = 3f;

	private void Start()
	{
		StartCoroutine(DestroyAtTime());
		IEnumerator DestroyAtTime()
		{
			yield return new WaitForSeconds(TimeToDestroy);
			Object.Destroy(base.gameObject);
		}
	}
}
