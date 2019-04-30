using System;
using UnityEngine;

[Serializable]
public struct LSystemData
{
	public Vector3 position;
	
	public LSystemData(Vector3 position)
	{
		this.position = position;
	}
}
