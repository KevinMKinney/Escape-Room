using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goastPickup : MonoBehaviour
{
    // Start is called before the first frame update
    Transform trans;
    pick_up pick;
    public float anger;
    float neverforget = 1;
    ParticleSystem part;
    public Color col;
    public Color normcol;
    bool speedbost;
    bool speedbosty;
    float cloneSleep = 10;
    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log(other.gameObject.name);
        if (other.GetComponent<pick_up>() != null)
        {
            if ((other.GetComponent<pick_up>().pickedup == 5) && (other.GetComponent<pick_up>().firstPickUp = true))
            {
                if (trans != null)
                {
                    trans.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    trans.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    Vector3 pos = transform.position;
                    pos.y -= 1;
                    //pos += transform.forward / 2;
                    trans.position = pos;
                }
                trans = other.transform;
                pick = other.GetComponent<pick_up>();
            }
        }
        if (other.transform.CompareTag("Player"))
        {
            anger += 5;
        }
        if ((other.GetComponent<goastPickup>() != null) && (GetComponent<ghost>().life > GetComponent<ghost>().growUpAge))
        {
            if (anger < 8 && cloneSleep < 0)
            {
                BabyGoastHadler.HandleGoastCollision(this);
            }
        }
    }
    public int Clone()
    {
        cloneSleep = 20;
        Debug.Log("cloneing!!!!.........");
        int b = Random.Range(1, 5);
        for (int i = 0; i < b; i++)
        {
            Vector3 pos = transform.position;
            pos.y += Random.Range(-0.7777f, 0.7777f);
            GameObject clone = Instantiate(this.gameObject, pos, transform.rotation);

            clone.GetComponent<CharacterNavigationControllor>().inharitSpeed(GetComponent<CharacterNavigationControllor>().movementSpeed);
        }
        return b;
    }
    void TargitHit(float DamageAmount)
    {
        anger += DamageAmount;
    }
    private void Start()
    {
        part = GetComponent<ParticleSystem>();
    }
    public void Pickup(pick_up pik)
    {
        if (trans != null)
        {
            trans.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            trans.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Vector3 pos = transform.position;
            trans.GetComponent<Rigidbody>().isKinematic = false;
            trans.GetComponent<Collider>().enabled = true;
            pos.y -= 1;
            //pos += transform.forward / 2;
            trans.position = pos;
        }
        pik.rb.isKinematic = true;
        pik.GetComponent<Collider>().enabled = false;
        trans = pik.transform;
        pick = pik;
        if (!speedbost)
        {
            speedbost = true;
            StartCoroutine("cow", 4);
        }
    }
    IEnumerator cow(float speedinboost)
    {
        GetComponent<CharacterNavigationControllor>().movementSpeed += speedinboost;
        yield return new WaitForSeconds(15);
        GetComponent<CharacterNavigationControllor>().movementSpeed -= speedinboost;
        speedbost = false;
        speedbosty = false;
    }
    private void Update()
    {
        //Debug.Log(cloneSleep -= 0.01f);
        cloneSleep -= 0.01f;
        if (anger > neverforget)
        {
            anger -= 0.002f;
        }
        ParticleSystem.MainModule psmain = part.main;
        if (anger > 10.01)
        {
            psmain.startColor = col;
            if ((!speedbosty))
            {
                speedbosty = true;
                GetComponent<CharacterNavigationControllor>().movementSpeed += 1.4f;
            }
            if (Random.Range(0, 50) == 0 && trans != null)
            {
               // Debug.Log("how");
                if (pick.transform.childCount > 0 && pick.transform.GetChild(0).GetComponent<FirePistol>() != null)
                {
                    pick.transform.GetChild(0).GetComponent<FirePistol>().StartCoroutine("cows");
                }
            }
        } else
        {
            if (anger > 7.5)
            {
                if (Random.Range(0, 50) == 0 && trans != null)
                {
                    // Debug.Log("how");
                    if (pick.transform.childCount > 0)
                    {
                        if (pick.transform.GetChild(0).GetComponent<FirePistol>() != null)
                        {
                            pick.transform.GetChild(0).GetComponent<FirePistol>().StartCoroutine("cows");
                        }
                    }
                }
            }
            if ((speedbosty))
            {
                speedbosty = false;
                GetComponent<CharacterNavigationControllor>().movementSpeed -= 1.4f;
            }
            psmain.startColor = normcol;
        }
       // Debug.Log(anger);
        if (trans != null)
        {
            if (pick.pickedup == 5)
            {
                Vector3 pos = transform.position;
                pos.y -= 1;
                //pos += transform.forward/2;
                trans.position = pos;
            } else
            {
                anger += 5;
                trans = null;
            } 
        }
    }
}
