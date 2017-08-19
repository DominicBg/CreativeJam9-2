using UnityEngine;

public static class Vector234Extensions
{
	// Vector2
	public static Vector2 SetX(this Vector2 aVec, float aXValue)
	{
		aVec.x = aXValue;
		return aVec;
	}
	public static Vector2 SetY(this Vector2 aVec, float aYValue)
	{
		aVec.y = aYValue;
		return aVec;
	}

	public static Vector2 SetAbsolute(this Vector2 vec)
	{
		vec.x = Mathf.Abs(vec.x);
		vec.y = Mathf.Abs(vec.y);
		return vec;
	}
	// Vector3
	public static Vector3 SetX(this Vector3 aVec, float aXValue)
	{
		aVec.x = aXValue;
		return aVec;
	}
	public static Vector3 SetY(this Vector3 aVec, float aYValue)
	{
		aVec.y = aYValue;
		return aVec;
	}
	public static Vector3 SetZ(this Vector3 aVec, float aZValue)
	{
		aVec.z = aZValue;
		return aVec;
	}

	public static Vector3 SetAbsolute(this Vector3 vec)
	{
		vec.x = Mathf.Abs(vec.x);
		vec.y = Mathf.Abs(vec.y);
		vec.z = Mathf.Abs(vec.z);
		return vec;
	}

	// Vector4
	public static Vector4 SetX(this Vector4 aVec, float aXValue)
	{
		aVec.x = aXValue;
		return aVec;
	}
	public static Vector4 SetY(this Vector4 aVec, float aYValue)
	{
		aVec.y = aYValue;
		return aVec;
	}
	public static Vector4 SetZ(this Vector4 aVec, float aZValue)
	{
		aVec.z = aZValue;
		return aVec;
	}
	public static Vector4 SetW(this Vector4 aVec, float aWValue)
	{
		aVec.w = aWValue;
		return aVec;
	}
	//Color
	public static Color Clamp(this Color color)
	{
		color.r = Mathf.Clamp01(color.r);
		color.g = Mathf.Clamp01(color.g);
		color.b = Mathf.Clamp01(color.b);
		color.a = Mathf.Clamp01(color.a);

		return color;
	}
	public static Color ClampMaxAlpha(this Color color)
	{
		color.r = Mathf.Clamp01(color.r);
		color.g = Mathf.Clamp01(color.g);
		color.b = Mathf.Clamp01(color.b);
		color.a = 1;
		
		return color;
	}
	public static float GetLuminance(this Color color)
	{
		return (0.2126f * color.r) + (0.7152f * color.g) + (0.722f * color.b);
	}
	public static int MinimumOne(this int number)
	{
		if(number < 1)
			number = 1;
		return number;
	}
	public static int Minimum(this int number, int min)
	{
		if(number < min)
			number = min;
		return number;
	}
}