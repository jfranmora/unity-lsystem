using UnityEngine;

public class TurtleTest : MonoBehaviour
{
	public Turtle turtle;

	private void Awake()
	{
		turtle = new Turtle(Vector3.zero, Quaternion.identity);
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.W))
		{
			turtle.MoveForward(1f);
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			turtle.MoveForward(-1f);
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			turtle.Rotate(new Vector3(0, 90, 0));
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			turtle.Rotate(new Vector3(0, -90, 0));
		}
	}

	private void OnDrawGizmos()
	{
		if (turtle != null)
		{
			turtle.DrawGizmos();
		}
	}
}
