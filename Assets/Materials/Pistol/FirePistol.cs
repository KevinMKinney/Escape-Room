using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirePistol : MonoBehaviour
{     // Update is called once per frame
    public GameObject gun;
    public GameObject muzzle;
    public float distance;
    public float DamageAmount = 5;
    public bool isfiring = false;
    public Transform player;
    void Update()
    {
     if ((Input.GetMouseButton(1)) && (!isfiring) && (GetComponentInParent<pick_up>().pickedup == 2))
        {
            StartCoroutine("GunFire");
        }   
    }
    IEnumerator GunFire()
    {
        RaycastHit Shot;
        isfiring = true;
        
        if (Physics.Raycast(player.position, player.TransformDirection(Vector3.forward), out Shot))
        {
            distance = Shot.distance;
            Shot.transform.SendMessage("TargitHit", DamageAmount, SendMessageOptions.DontRequireReceiver);
        }
        gun.GetComponent<Animation>().Play("pistilShot");
        muzzle.SetActive(true);
        //gunfire.Play();
        yield return new WaitForSeconds(0.7f);
        isfiring = false;
    }
    IEnumerator cows()
    {
        RaycastHit Shot;
        isfiring = true;
        Vector3 pos = transform.position;
        pos += transform.forward;
        if (Physics.Raycast(pos, transform.TransformDirection(Vector3.forward), out Shot))
        {
            distance = Shot.distance;
            Shot.transform.SendMessage("TargitHit", DamageAmount, SendMessageOptions.DontRequireReceiver);
            Debug.Log(Shot.transform.gameObject.name);
        }
        gun.GetComponent<Animation>().Play("pistilShot");
        muzzle.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        isfiring = false;
    }
}
