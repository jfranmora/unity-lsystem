using System;
using UnityEngine;

[Serializable]
public class LSystemNode : Node<LSystemData>
{
	public LSystemNode(Vector3 value) : base(new LSystemData(value)) { }

	public void DrawGizmos()
	{
		Gizmos.DrawWireSphere(value.position, .1f);

		foreach (var edge in edges)
		{
			Gizmos.DrawLine(this.value.position, edge.value.position);

			((LSystemNode)edge).DrawGizmos();
		}
	}
}
