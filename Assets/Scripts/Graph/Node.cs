using System;
using System.Collections.Generic;

[Serializable]
public class Node<T>
{
	public List<Node<T>> edges;
	public T value;

    public Node(T value)
	{
		edges = new List<Node<T>>();
		this.value = value;
	}
}
