using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneWayDoor : MonoBehaviour
{
    [SerializeField] BoxCollider col;
    [SerializeField] Material material;
    // Start is called before the first frame update
    void Start()
    {
        /*Color b = material.color;
        b.a = 55;
        material.color = b;*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            col.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            col.enabled = true;
        }
    }
    public void changeMaterial(int alpha)
    {
        Color c = material.color;
        c.a = alpha;
        material.color = c;
    }
}
