using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geyser : MonoBehaviour
{
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audGeyser;
    [Range(0, 1)][SerializeField] float audGeyserVol;
    [SerializeField] float speed;
    private Vector3 move;
    // Start is called before the first frame update
    void Start()
    {
        move = new Vector3(0.0f, speed, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            aud.PlayOneShot(audGeyser,audGeyserVol);
            gameManager.instance.playerScript.addVelocityY(speed);
        }
    }
}
