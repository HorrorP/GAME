using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CorridorDecalSpawnerManager : MonoBehaviour
{
	private enum DecalOnCorridorGeneration
	{
		None = 0,
		Right = 1,
		Left = 2,
		All = 3
	}

	[Serializable]
	public struct DecalSquare
	{
		public Transform Position;

		public Vector2 Size;

		public float DecalSize;
	}

	private const float GizmoWidth = 0.05f;

	public CorridorDecalsScriptableObject CorridorDecals;

	public DecalProjector DecalProjectorPrefab;

	public float MaxRandomRotation = 10f;

	public bool ShowGizmo;

	public DecalSquare DecalSquaresRight;

	public DecalSquare DecalSquaresLeft;

	public void Awake()
	{
		switch ((DecalOnCorridorGeneration)UnityEngine.Random.Range(0, 4))
		{
		case DecalOnCorridorGeneration.Right:
			InstantiateDecalAtRandomPosition(DecalSquaresRight);
			break;
		case DecalOnCorridorGeneration.Left:
			InstantiateDecalAtRandomPosition(DecalSquaresLeft);
			break;
		case DecalOnCorridorGeneration.All:
			InstantiateDecalAtRandomPosition(DecalSquaresRight);
			InstantiateDecalAtRandomPosition(DecalSquaresLeft);
			break;
		}
	}

	private void InstantiateDecalAtRandomPosition(DecalSquare decalSquares)
	{
		DecalProjector decalProjector = UnityEngine.Object.Instantiate(DecalProjectorPrefab, decalSquares.Position);
		decalProjector.material = CorridorDecals.DecalsMaterials[UnityEngine.Random.Range(0, CorridorDecals.DecalsMaterials.Length)];
		float num = (decalSquares.Size.x - decalSquares.DecalSize) * 0.5f;
		float num2 = (decalSquares.Size.y - decalSquares.DecalSize) * 0.5f;
		decalProjector.transform.localPosition = new Vector3(UnityEngine.Random.Range(0f - num, num), UnityEngine.Random.Range(0f - num2, num2), 0f);
		decalProjector.transform.Rotate(0f, 0f, UnityEngine.Random.Range(0f - MaxRandomRotation, MaxRandomRotation));
	}

	private void OnDrawGizmos()
	{
		if (ShowGizmo)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawCube(DecalSquaresRight.Position.transform.position, new Vector3(0.05f, DecalSquaresRight.Size.y, DecalSquaresRight.Size.x));
			Gizmos.DrawCube(DecalSquaresLeft.Position.transform.position, new Vector3(0.05f, DecalSquaresLeft.Size.y, DecalSquaresLeft.Size.x));
		}
	}
}
