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

	public static List<T> Shuffle<T>(this List<T> list)
	{
		List<T> shuffledList = new List<T>(0);
		int nListCount = list.Count;
		int nElementIndex;
		for (int i = 0; i < nListCount; i++)
		{
			nElementIndex = Random.Range(0, list.Count);
			shuffledList.Add(list[nElementIndex]);
			list.RemoveAt(nElementIndex);
		}

		return shuffledList;
	}
}