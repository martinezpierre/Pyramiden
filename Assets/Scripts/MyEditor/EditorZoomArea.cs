using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	public static class RectExtensions
	{
		public static Rect ScaleSizeBy(this Rect rect, float scale) { return rect.ScaleSizeBy(scale, rect.center); }

		public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint) 
		{
			Rect result = rect;

			//"translate" the top left to something like an origin
			result.x -= pivotPoint.x;
			result.y -= pivotPoint.y;

			//Scale the rect
			result.xMin *= scale;
			result.yMin *= scale;
			result.xMax *= scale;
			result.yMax *= scale;
		
			//"translate" the top left back to its original position
			result.x += pivotPoint.x;
			result.y += pivotPoint.y;

			return result;
		}
	}

	public class EditorZoomArea
	{
		private static Stack<Matrix4x4> previousMatrices = new Stack<Matrix4x4>();

		public static Rect Begin(float zoomScale, Rect screenCoordsArea)
		{
			GUI.EndGroup();

			Rect clippedArea = screenCoordsArea.ScaleSizeBy(1.0f / zoomScale, screenCoordsArea.center);
			clippedArea.y += 21;

			GUI.BeginGroup(clippedArea);

			previousMatrices.Push(GUI.matrix);
			Matrix4x4 translation = Matrix4x4.TRS(clippedArea.center, Quaternion.identity, Vector3.one);
			Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
			GUI.matrix = translation * scale * translation.inverse;

			return clippedArea;
		}

		public static void End()
		{
			GUI.matrix = previousMatrices.Pop();
			GUI.EndGroup();
			GUI.BeginGroup(new Rect(0, 21, Screen.width, Screen.height));
		}
	}