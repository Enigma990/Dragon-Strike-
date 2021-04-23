using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] GameObject arrowPool = null;

    Rigidbody arrowRb;
    float pushBackForce = 10f;
    bool decayArrow = false;

    public TrailRenderer arrowTrail;

    // Start is called before the first frame update
    void Start()
    {
        arrowRb = GetComponent<Rigidbody>();
        arrowRb.isKinematic = true;
        arrowTrail = GetComponent<TrailRenderer>();
        arrowTrail.enabled = false;

        arrowPool = GameObject.FindGameObjectWithTag("Respawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RemoveArrow(float removeTime)
    {
        yield return new WaitForSeconds(removeTime);
        arrowRb.useGravity = true;
        arrowRb.isKinematic = true;
        gameObject.transform.SetParent(arrowPool.transform);
        arrowTrail.enabled = false;
        gameObject.SetActive(false);
    }

    public void BasicAttack(float arrowSpeed)
    {
        arrowRb.isKinematic = false;
        arrowRb.transform.SetParent(null);
        arrowRb.AddForce(-transform.forward * arrowSpeed, ForceMode.Impulse);
        pushBackForce = arrowSpeed * 10;

        decayArrow = arrowSpeed > 32 ? true : false;
    }

    public void DragonStrike(float arrowSpeed, GameObject dragon)
    {
        arrowRb.isKinematic = false;
        arrowRb.useGravity = false;
        arrowRb.transform.SetParent(null);
        arrowRb.AddForce(-transform.forward * arrowSpeed, ForceMode.Impulse);

        StartCoroutine(Dragon(dragon));
    }

    IEnumerator Dragon(GameObject dragon)
    {
        StartCoroutine(RemoveArrow(1f));
        yield return new WaitForSeconds(0.2f);

        Instantiate(dragon, arrowRb.transform.position,Quaternion.LookRotation(-arrowRb.transform.forward));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //Vector3 dir = collision.GetContact(0).point - transform.position;
            //dir = dir.normalized;

            if (decayArrow)
            {
                collision.gameObject.GetComponent<DecayScript>().Decay();
                StartCoroutine(RemoveArrow(0));
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * pushBackForce, ForceMode.Impulse);
                StartCoroutine(RemoveArrow(3f));
            }
        }
    }
}