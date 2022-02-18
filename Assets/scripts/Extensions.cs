using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    /* 
    * Written this semester.
    */
    //Returns a random element in a list. This function is generic so the list can hold any type of element.
    //Lists and arrays implement IEnumerable, so this function can be used on both.
    public static T RandomElement<T>(this IEnumerable<T> list)
    {
        return list.ElementAt(Random.Range(0, list.Count()));
    }
}
