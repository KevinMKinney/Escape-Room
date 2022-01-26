using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifeTime = 500;
    public float life = 0;
    public float growUpAge = 100;
    bool dieing;
    // Update is called once per frame

    private void Start()
    {
        Vector3 vec = transform.position;
        vec.y -= 1;
        transform.position = vec;
        lifeTime += Random.Range(-50.5f, 1550.4f);
    }
    void Update()
    {
        life += 0.1f;
        //Debug.Log(life);
        if ((life > lifeTime) && (!dieing))
        {
            dieing = true;
            Destroy(GetComponent<CharacterNavigationControllor>());
            Destroy(GetComponent<goastPickup>());
            Destroy(GetComponent<WaypointNavagiator>());
            GetComponent<ParticleSystem>().Stop();
            BabyGoastHadler.Die();
        }
        if (life > lifeTime + 490)
        {
            Destroy(this.gameObject);
        }
        if (life < growUpAge + 0.1f)
        {
            Vector3 vec = Vector3.zero;
            vec.x = life / growUpAge;
            vec.z = life / growUpAge;
            vec.y = life / growUpAge;
            transform.localScale = vec;
        }
    }
}
