using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nukeExplosion : MonoBehaviour
{
    [SerializeField] float duration;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy gameObject after [SerializeField] duration
        Destroy(gameObject, duration);
    }
}
