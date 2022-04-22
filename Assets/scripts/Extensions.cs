using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static T RandomElement<T>(this IEnumerable<T> list)
    {
        return list.ElementAt(Random.Range(0, list.Count()));
    }
}
