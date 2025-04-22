using System.Collections;
using Script;
using UnityEngine;

public class KillPlayerAtTime : MonoBehaviour
{
	public float TimeToKillSeconds = 1.9f;

	public AudioSource PlayAtKill;

	private void Start()
	{
		GameObject obj = GameObject.FindGameObjectWithTag("Player");
		obj.GetComponent<PlayerEnableMediator>().DisableAll();
		PlayerDie component = obj.GetComponent<PlayerDie>();
		StartCoroutine(KillAtTime(component));
		IEnumerator KillAtTime(PlayerDie die)
		{
			yield return new WaitForSeconds(TimeToKillSeconds);
			PlayAtKill.PlayWithRandomPitch();
			die.Die();
		}
	}
}
