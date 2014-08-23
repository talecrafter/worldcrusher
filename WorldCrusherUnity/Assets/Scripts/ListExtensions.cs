using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ListExtensions
{
	public static T PickRandom<T>(this List<T> source)
	{
		if (source.Count == 0)
			return default(T);

		int i = Random.Range(0, source.Count);
		return source[i];
	}
}