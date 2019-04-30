using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Turtle
{
	public TurtleTransform transform;

	private TurtleTransform initialData;
	private Stack<TurtleTransform> savedStates;

	public Turtle(Vector3 position, Quaternion rotation)
	{
		transform = new TurtleTransform()
		{
			position = position,
			rotation = rotation
		};

		initialData = transform;

		savedStates = new Stack<TurtleTransform>();
	}

	public void MoveForward(float value)
	{
		transform.position += transform.forward * value;
	}

	public void Rotate(Vector3 rotation)
	{
		transform.rotation = transform.rotation * Quaternion.Euler(rotation);
	}

	public void Rotate(Quaternion rotation)
	{
		transform.rotation = transform.rotation * rotation;
	}

	public void SaveState()
	{
		savedStates.Push(transform);
	}

	public void LoadState()
	{
		transform = savedStates.Pop();
	}

	public void DrawGizmos()
	{
		Gizmos.DrawSphere(transform.position, .25f);
		Gizmos.DrawLine(transform.position, transform.position + (transform.rotation * Vector3.forward).normalized);
	}

	public void ResetTurtle()
	{
		transform = initialData;
		savedStates.Clear();
	}
}
