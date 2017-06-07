using UnityEngine;

/// <summary>
/// Unitys colour is not serialized by default.
/// This causes problems saving colours or sending them over network.
/// This class implicity converts to and from unitys colour making it
/// easy to use when you need to serialize colour.
/// 
/// for example
/// 
/// Color a = color.red;
/// SerializedColor b = a;
/// a = b;
/// if(a == b)
///		print("This is true");
///	if(b == color.red)
///		print("This is also true");
/// </summary>

[System.Serializable]
public struct SerializedColor
{
	public float r;
	public float g;
	public float b;
	public float a;

	public SerializedColor(float r, float g, float b, float a)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.a = a;
	}

	public static implicit operator Color(SerializedColor c)
	{
		return new Color(c.r, c.g, c.b, c.a);
	}

	public static implicit operator SerializedColor(Color c)
	{
		return new SerializedColor(c.r, c.g, c.b, c.a);
	}

}
