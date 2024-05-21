using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour, IPickup
{
    [SerializeField] AudioClip audPickup;
    [Range(0,1)][SerializeField] float audVol;
    [Header("---Bool for determining if key is for one scene or multiple scenes---")]
    [SerializeField] bool isSingleScene;
    [SerializeField] conditionalDoor door;
    //pickup inherit from interface
    //handles single and multiple scene use cases
    public void Start()
    {
        if (!isSingleScene)
        {
            door = null;
        }
    }
    public void pickup()
    {
        if (isSingleScene)
        {
            StartCoroutine(playSound());
            door.updateKey();
            gameObject.SetActive(false);
        }
        else
        {
            playerStats.keys.Add(gameObject);
            StartCoroutine(playSound());
            gameObject.SetActive(false);
        }
    }
    IEnumerator playSound()
    {
        AudioSource.PlayClipAtPoint(audPickup,transform.position,audVol);
        yield return new WaitForSeconds(0f);
    }
}
