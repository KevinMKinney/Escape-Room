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

    //Shuffles a list.
    public static void Rearrange<T>( this IList<T> list)
    {
        //Start at the list length.
        int a = list.Count;
        //Keep updating the list until all elements have been shuffled.
        while (a > 1)
        {
            a--;
            //Get a random element to swap with.
            int b = Random.Range(0, a + 1);
            //Swap value at index a with index b.
            T value = list[b];
            list[b] = list[a];  
            list[a] = value;
        }
    }
}
