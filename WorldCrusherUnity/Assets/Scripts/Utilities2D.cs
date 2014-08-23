using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utilities2D {

	public static Transform GetHitFromPointer()
	{
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);
		return hit.transform;
	}

	public static Quaternion GetRotationFromVectorToVector(Vector2 fromVector, Vector2 toVector)
	{
		Vector3 r = new Vector3(0, 0, Mathf.Atan2((toVector.y - fromVector.y), (toVector.x - fromVector.x)) * Mathf.Rad2Deg);
		return Quaternion.Euler(r);
	}

}