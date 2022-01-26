using UnityEngine;

public class randomstart : MonoBehaviour
{
    // Start is called before the first frame update
    public int range = 5;
    void Start()
    {
        transform.Translate(Random.Range(-range,range), 0 , Random.Range(-range, range));
    }
}
