using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var minProperty = property.FindPropertyRelative("min");
		var maxProperty = property.FindPropertyRelative("max");

		EditorGUI.BeginProperty(position, label, property);

		var newRange = FloatRangeField(position, label, new Vector2(minProperty.floatValue, maxProperty.floatValue));
		minProperty.floatValue = newRange.x;
		maxProperty.floatValue = newRange.y;

		EditorGUI.EndProperty();
	}

	private static Vector2 FloatRangeField(Rect position, GUIContent label, Vector2 range)
	{
		position = EditorGUI.PrefixLabel(position, label);

		var content = new GUIContent[]
		{
			new GUIContent("Min"), new GUIContent("Max")
		};

		float[] currentValues = new float[]
		{
			range.x,
			range.y
		};

		EditorGUI.BeginChangeCheck();
		MultiFloatField(position, content, 33, currentValues);
		if (EditorGUI.EndChangeCheck())
		{
			range.x = currentValues[0];
			range.y = currentValues[1];
		}

		return range;
	}

	private static void MultiFloatField(Rect position, GUIContent[] subLabels, float labelWidth, float[] values)
	{
		float width = (position.width - (float)(2 - 1) * 2f) / (float)2;
		Rect pos = new Rect(position)
		{
			width = width
		};

		float labelWidth2 = EditorGUIUtility.labelWidth;
		int indentLevel = EditorGUI.indentLevel;
		EditorGUIUtility.labelWidth = labelWidth;
		EditorGUI.indentLevel = 0;
		for (int i = 0; i < values.Length; i++)
		{
			values[i] = EditorGUI.FloatField(pos, subLabels[i], values[i]);
			pos.x += width + 2f;
		}

		EditorGUIUtility.labelWidth = labelWidth2;
		EditorGUI.indentLevel = indentLevel;
	}
}
