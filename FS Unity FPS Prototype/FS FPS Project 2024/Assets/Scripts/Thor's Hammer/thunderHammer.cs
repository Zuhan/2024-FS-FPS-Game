using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunderHammer : MonoBehaviour
{

    public GameObject chainLightningPrefab;
    public AudioSource audioSource;
    [SerializeField] Transform castPos;
    [SerializeField] private float fireCooldown = 0.5f;
    private float lastFireTime;

    private void Start()
    {
        enabled = false;
    }

    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            if (Input.GetMouseButtonDown(0) && Time.time - lastFireTime > fireCooldown)
            {
                CastChainLightning();
                PlayAudio();
                lastFireTime = Time.time;
                StartCoroutine(displayCooldown());
            }
        }
    }

    void CastChainLightning()
    {
        GameObject chainLightning = Instantiate(chainLightningPrefab, castPos.position, Quaternion.identity);
        Vector3 castDir = Camera.main.transform.forward;
        Quaternion startRot = Quaternion.LookRotation(castDir);

        chainLightning.transform.rotation = startRot;
        chainLightning.transform.parent = null;

        chainLightning.GetComponent<thunderMagic>().SetInitialRotation(startRot);
    }

    IEnumerator displayCooldown()
    {
        gameManager.instance.cooldownRing.SetActive(false);
        yield return new WaitForSeconds(fireCooldown);
        gameManager.instance.cooldownRing.SetActive(true);
    }

    public void EnableThunderHammer()
    {
        //Debug.Log("Thunder Hammer Enabled");
        enabled = true;
    }
    public void DisableThunderHammer()
    {
        //Debug.Log("Thunder Hammer Disabled");
        enabled = false;
    }
    void PlayAudio()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
