[System.Serializable]
public struct FloatRange
{
	public float min;
	public float max;

	public FloatRange(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	public float GetRandom()
	{
		return UnityEngine.Random.Range(min, max);
	}

	public bool IsInRange(float value)
	{
		return max < value && min > value;
	}
}
