using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LSystem : MonoBehaviour
{
	public List<LSystemRule> rules = new List<LSystemRule>();
	public string startAxiom = "X";
	private string currentPath;

	[Header("Generation settings")]
	public int seed;
	[Range(1, 6)] public int iterations = 3;
	[Range(0f, 1f)] public float variance;
	public float angle = 25f;
	public float width = .1f;
	public FloatRange branchLength = new FloatRange(.5f, 1f);

	[Header("Step by step")]
	public bool drawStepByStep;
	public int stepCount = 1;
	public float stepDelay = .1f;

	[Header("Prefab references")]
	public bool generateTree;
	public TreeElement branch;

	[Header("Debug")]
	public bool debugTurtle;
	public bool debugTreeGraph;

	private Dictionary<char, string> ruleDict = new Dictionary<char, string>();

	private Vector3 initialPosition;
	private Quaternion initialRotation;

	private Turtle turtle;
	private LSystemNode treeRoot = null;
	private LSystemNode _currentNode;
	private System.Random _randomGenerator;

	#region Unity Events

	private void Awake()
	{
		seed = Random.Range(0, 999999);

		initialPosition = transform.position;
		initialRotation = transform.rotation;
	}

	private void OnDrawGizmos()
	{
		if (debugTreeGraph && treeRoot != null)
		{
			Gizmos.color = Color.black;
			treeRoot.DrawGizmos();
		}

		if (debugTurtle && turtle != null)
		{
			Gizmos.color = Color.red;
			turtle.DrawGizmos();
		}
	}

	#endregion

	#region Public API

	public void DoGenerateLSystem()
	{
		GenerateLSystem();
	}

	public void DoRandomGenerateLSystem()
	{
		seed = Random.Range(0, 999999);

		GenerateLSystem();
	}

	#endregion

	#region Helpers

	private void BuildRuleDictionary()
	{
		ruleDict.Clear();
		foreach (var rule in rules)
		{
			ruleDict.Add(rule.a, rule.b);
		}
	}

	private void GenerateLSystem()
	{
		BuildRuleDictionary();

		ClearLSystem();
		ProcessLSystem();

		StopCoroutine(nameof(DrawLSystemRoutine));

		if (drawStepByStep)
		{
			StartCoroutine(nameof(DrawLSystemRoutine));
		}
		else
		{
			DrawLSystem();
		}
	}

	private void ProcessLSystem()
	{
		currentPath = startAxiom;

		// Cache vars
		StringBuilder stringBuilder = new StringBuilder();
		char currentChar;

		// DO L-System
		for (int i = 0; i < iterations; i++)
		{
			char[] currentPathChars = currentPath.ToCharArray();
			for (int j = 0; j < currentPathChars.Length; j++)
			{
				currentChar = currentPathChars[j];
				if (ruleDict.ContainsKey(currentChar))
				{
					stringBuilder.Append(ruleDict[currentChar]);
				}
				else
				{
					stringBuilder.Append(currentChar);
				}

				currentPath = stringBuilder.ToString();

			}

			stringBuilder.Clear();
		}
	}

	private void ClearLSystem()
	{
		for (var i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	private void InitializeDrawSystem()
	{
		// Initialize random engine
		_randomGenerator = new System.Random(seed);

		turtle = new Turtle(Vector3.zero, Quaternion.identity);
		treeRoot = new LSystemNode(Vector3.zero);
	}

	private void DrawLSystem()
	{
		InitializeDrawSystem();

		// Initialize graph
		Stack<LSystemNode> nodeStack = new Stack<LSystemNode>();
		_currentNode = treeRoot;

		// Draw
		for (int i = 0; i < currentPath.Length; i++)
		{
			DoStep(currentPath[i], nodeStack);
		}
	}

	private IEnumerator DrawLSystemRoutine()
	{
		InitializeDrawSystem();

		// Initialize graph
		Stack<LSystemNode> nodeStack = new Stack<LSystemNode>();
		_currentNode = treeRoot;

		// Draw
		for (int i = 0; i < currentPath.Length; i++)
		{
			DoStep(currentPath[i], nodeStack);

			if (drawStepByStep && i % stepCount == 0)
			{
				yield return new WaitForSeconds(stepDelay);
			}
		}
	}

	private void DoStep(char input, Stack<LSystemNode> nodeStack)
	{
		switch (input)
		{
			case 'F':
				float length = Mathf.Lerp(branchLength.min, branchLength.max, (float)_randomGenerator.NextDouble());
				Vector3 start = turtle.transform.position;
				turtle.MoveForward(length);
				Vector3 end = turtle.transform.position;

				DoForwardGraph(start, end);
				if (generateTree) DoForwardTree(start, end);

				break;

			case 'X':
				break;

			case '+':
				turtle.Rotate(Vector3.up * angle * (1f + variance * Mathf.Lerp(-1f, 1f, (float)_randomGenerator.NextDouble())));
				break;

			case '-':
				turtle.Rotate(Vector3.down * angle * (1f + variance * Mathf.Lerp(-1f, 1f, (float)_randomGenerator.NextDouble())));
				break;

			case '[':
				turtle.SaveState();
				nodeStack.Push(_currentNode);
				break;

			case ']':
				turtle.LoadState();
				_currentNode = nodeStack.Pop();
				break;
		}
	}

	/// <summary>
	/// Instantiate new tree element
	/// </summary>
	private void DoForwardTree(Vector3 start, Vector3 end)
	{
		TreeElement currentElement = Instantiate(branch, turtle.transform.position, turtle.transform.rotation);
		currentElement.transform.SetParent(transform);

		// Setup line renderer
		currentElement.lineRenderer.useWorldSpace = true;
		currentElement.lineRenderer.startWidth = width;
		currentElement.lineRenderer.endWidth = width;
		currentElement.lineRenderer.SetPosition(0, start);
		currentElement.lineRenderer.SetPosition(1, end);
		currentElement.lineRenderer.sharedMaterial = currentElement.material;
	}

	/// <summary>
	/// Generate new graph node
	/// </summary>
	private void DoForwardGraph(Vector3 start, Vector3 end)
	{
		// Create new node
		LSystemNode node = new LSystemNode(end);
		_currentNode.edges.Add(node);
		_currentNode = node;
	}

	#endregion

	#region Editor Helpers

	public void SetupExample1()
	{
		rules.Clear();

		rules.Add(new LSystemRule()
		{
			a = 'F',
			b = "FF-F-F-F-FF"
		});

		startAxiom = "F-F-F-F";

		variance = 0;
		iterations = 4;
		angle = 90;
		branchLength = new FloatRange(1, 1);

		GenerateLSystem();
	}

	public void SetupTreeDemo1()
	{
		rules.Clear();

		rules.Add(new LSystemRule()
		{
			a = 'F',
			b = "F[+F]F[-F]F"
		});

		startAxiom = "F";

		variance = .35f;
		iterations = 4;
		angle = 26;
		branchLength = new FloatRange(.5f, 1);

		GenerateLSystem();
	}

	public void SetupTreeDemo2()
	{
		rules.Clear();

		rules.Add(new LSystemRule()
		{
			a = 'F',
			b = "F[+F]F[-F][F]"
		});

		startAxiom = "F";

		variance = .35f;
		iterations = 5;
		angle = 20;
		branchLength = new FloatRange(.5f, 1);

		GenerateLSystem();
	}

	public void SetupTreeDemo3()
	{
		rules.Clear();

		rules.Add(new LSystemRule()
		{
			a = 'F',
			b = "FF-[-F+F+F]+[+F-F-F]"
		});

		startAxiom = "F";

		variance = .35f;
		iterations = 4;
		angle = 23;
		branchLength = new FloatRange(.5f, 1);

		GenerateLSystem();
	}

	public void SetupTreeDemo4()
	{
		rules.Clear();

		rules.Add(new LSystemRule()
		{
			a = 'X',
			b = "F[+X]F[-X]+X"
		});

		rules.Add(new LSystemRule()
		{
			a = 'F',
			b = "FF"
		});

		startAxiom = "X";

		variance = .35f;
		iterations = 6;
		angle = 20;
		branchLength = new FloatRange(.5f, 1);

		GenerateLSystem();
	}

	public void SetupTreeDemo5()
	{
		rules.Clear();

		rules.Add(new LSystemRule()
		{
			a = 'X',
			b = "F[+X][-X]FX"
		});

		rules.Add(new LSystemRule()
		{
			a = 'F',
			b = "FF"
		});

		startAxiom = "X";

		variance = .35f;
		iterations = 6;
		angle = 25;
		branchLength = new FloatRange(.5f, 1);

		GenerateLSystem();
	}

	public void SetupTreeDemo6()
	{
		rules.Clear();

		rules.Add(new LSystemRule()
		{
			a = 'X',
			b = "F-[[X]+X]+F[+FX]-X"
		});

		rules.Add(new LSystemRule()
		{
			a = 'F',
			b = "FF"
		});

		startAxiom = "X";

		variance = .35f;
		iterations = 5;
		angle = 25;
		branchLength = new FloatRange(.5f, 1);

		GenerateLSystem();
	}

	#endregion
}