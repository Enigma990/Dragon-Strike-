using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayScript : MonoBehaviour
{
    Renderer decayRenderer;

    bool shouldDecay = false;

    // Start is called before the first frame update
    void Start()
    {
        decayRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldDecay)
        {
            decayRenderer.material.SetFloat("Vector1_8D141452", decayRenderer.material.GetFloat("Vector1_8D141452") + Time.deltaTime * 0.4f);
            Destroy(gameObject, 3);
        }
    }

    public void Decay()
    {
        shouldDecay = true;
        GetComponent<Collider>().isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
