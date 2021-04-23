using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DestroyDragon : MonoBehaviour
{
    Rigidbody dragonRb;
    Renderer[] renderers;

    float speed = 0.08f;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        dragonRb = GetComponent<Rigidbody>();

        Destroy(gameObject, 10f);

    }
    private void Update()
    {
        dragonRb.velocity += transform.forward * speed;

        foreach(Renderer r in renderers)
        {
            Material[] mat = r.materials;

            foreach(Material m in mat)
            {
                m.SetFloat("Vector1_DA0D6C7", m.GetFloat("Vector1_DA0D6C7") + .5f * Time.deltaTime);
            }
        }
    }

}