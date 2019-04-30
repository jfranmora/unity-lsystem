using System;
using UnityEngine;

[Serializable]
public struct TurtleTransform
{
	public Vector3 position;
	public Quaternion rotation;

	public Vector3 forward
	{
		get { return (rotation * Vector3.forward).normalized; }
	}

	public Vector3 back
	{
		get { return -forward; }
	}

	public Vector3 right
	{
		get { return (rotation * Vector3.right).normalized; }
	}

	public Vector3 left
	{
		get { return -right; }
	}

	public Vector3 up
	{
		get { return (rotation * Vector3.up).normalized; }
	}

	public Vector3 down
	{
		get { return -up; }
	}
}