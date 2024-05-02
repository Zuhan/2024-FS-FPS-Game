using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    public CharacterController controller;
    [SerializeField] AudioSource aud;

    [Header("----- Player Stats-----")]
    [SerializeField] int HP;
    [SerializeField] float defaultWalkSpeed;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float speed;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxJumps;
    [SerializeField] int gravity;
    [SerializeField] int currentPoints;

    [Header("----- Weapon Stats -----")]
    [SerializeField] public List<weaponStats> weapons = new List<weaponStats>();
    [SerializeField] GameObject fireStaff;
    [SerializeField] GameObject Slingshot;
    [SerializeField] int castDamage;
    [SerializeField] float castRate;
    [SerializeField] int castDist;

    [Header("----- Sprinting Stats -----")]
    [SerializeField] int stamina;
    [SerializeField] int maxStamina;
    [SerializeField] int sprintDelay;
    [SerializeField] float sprintDecayRate;
    [SerializeField] int staminaToRemove;
    [SerializeField] int sprintRegenDelay;
    [SerializeField] float sprintRegenRate;
    [SerializeField] int staminaToAdd;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;

    [SerializeField] int rayDistance;

    Dictionary<string, GameObject> weaponSlots = new Dictionary<string, GameObject>();

    Vector3 moveDirection;
    Vector3 playerVelocity;
    bool isSprinting;
    bool playingSteps;
    bool canSprint;
    int jumpedTimes;
    int sprintDecayTimes;
    int interactDelay;
    int hpOrig;
    int selectedWeapon;

    // Start is called before the first frame update
    void Start()
    {
        hpOrig = HP;
        updatePlayerUI();
        stamina = maxStamina;
        canSprint = true;
        currentPoints = gameManager.instance.points;

        weaponSlots.Add("Fire Staff", fireStaff);
        weaponSlots.Add("Slingshot", Slingshot);
        loadStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.red);
            selectWeapon();
            movement();
        }
    }
    void movement()
    {
        //Movement

        if (controller.isGrounded)
        {
            jumpedTimes = 0;
            playerVelocity = Vector3.zero;
        }

        moveDirection = (Input.GetAxis("Horizontal") * transform.right)
                        + (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDirection * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpedTimes < maxJumps)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (controller.isGrounded && moveDirection.normalized.magnitude > 0.3f && !playingSteps)
        {
            StartCoroutine(playSteps());
        }

        //Sprinting implemented by Paul
        if (Input.GetButtonDown("Sprint") && !isSprinting)
        {
            StartCoroutine(Sprint(sprintDecayRate));
        }

        if (stamina <= 0)
        {
            canSprint = false;
            StartCoroutine(StaminaRegen(sprintRegenRate));
        }
        if (Input.GetButtonDown("Interact"))
        {
            StartCoroutine(interact());
        }
    }
    IEnumerator playSteps()
    {
        playingSteps = true;

        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        if (!isSprinting)
            yield return new WaitForSeconds(0.5f);
        else
            yield return new WaitForSeconds(0.3f);

        playingSteps = false;
    }

    //Sprint by Paul
    IEnumerator Sprint(float stamDecay)
    {
        if (canSprint && stamina > 5 && controller.isGrounded)
        {
            isSprinting = true;
            speed *= sprintMultiplier;
            while (stamina > 0)
            {
                stamina -= staminaToRemove;
                yield return new WaitForSeconds(stamDecay);
            }
            isSprinting = false;
            speed = defaultWalkSpeed;
        }
    }

    //Stamina Handler by Paul
    IEnumerator StaminaRegen(float stamRegen)
    {
        if (!isSprinting)
        {
            while (stamina < maxStamina)
            {
                stamina += staminaToAdd;
                yield return new WaitForSeconds(stamRegen);
            }
        }
        canSprint = true;
    }

    //interact handler by Ben
    IEnumerator interact()
    {
        interactDelay = 1;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, 5))
        {
            
            IfInteract interact = hit.collider.GetComponent<IfInteract>();
            if (hit.transform != transform && interact != null)
            {
                interact.interact();
            }
        }
        yield return new WaitForSeconds(interactDelay);
    }
    //checks if you are near an object that you can interact with
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("pickup"))
        {
            IPickup pickup = other.GetComponent<IPickup>();
            pickup.pickup();
        }
    }

    //taking damage function
    public void TakeDamage(int amount)
    {
        HP -= amount;
        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
        updatePlayerUI();
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            gameManager.instance.lose();
        }
    }
    //hit indicator ienumerator
    IEnumerator flashDamage()
    {
        gameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageScreen.SetActive(false);
    }

    //updating ui
    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / hpOrig;
    }

    public void getWeaponStats(weaponStats weapon)
    {
        weapons.Add(weapon);
        selectedWeapon = weapons.Count - 1;

        if (weapons.Count == 1)
        {
            selectedWeapon = 0; // Automatically select the newly added weapon if it's the first one
            changeWeapon();
        }
        else
        {
            selectedWeapon = weapons.Count - 1;
        }

        castDamage = weapon.castDamage;
        castDist = weapon.castDist;
        castRate = weapon.castRate;
        saveStats();
        Debug.Log("Weapon added to List: " + weapon.name);
    }

    void selectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Find the Slingshot in the weapons list
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == "Slingshot")
                {
                    selectedWeapon = i;
                    changeWeapon();
                    return;
                }
            }
            Debug.Log("Slingshot not found in weapons list.");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Find the fire staff in the weapons list
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == "Fire Staff")
                {
                    selectedWeapon = i;
                    changeWeapon();
                    return;
                }
            }
            Debug.Log("Fire Staff not found in weapons list.");
        }
    }

    void changeWeapon()
    {
        Debug.Log("Weapon Changed");

        castDamage = weapons[selectedWeapon].castDamage;
        castDist = weapons[selectedWeapon].castDist;
        castRate = weapons[selectedWeapon].castRate;

        // Iterate through all weapons
        foreach (var kvp in weaponSlots)
        {
            string weaponName = kvp.Key;
            GameObject weaponObject = kvp.Value;

            // Check if the current weapon matches the selected weapon
            if (weaponName == weapons[selectedWeapon].name)
            {
                // Set the mesh and material of the currently equipped weapon
                weaponObject.GetComponent<MeshFilter>().sharedMesh = weapons[selectedWeapon].weaponModel.GetComponent<MeshFilter>().sharedMesh;
                weaponObject.GetComponent<MeshRenderer>().sharedMaterial = weapons[selectedWeapon].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

                // Enable the Fire Staff script only if the selected weapon is the Fire Staff
                if (weaponName == "Fire Staff")
                {
                    fireStaff.GetComponent<fireStaff>().EnableFireStaff();
                }
                else
                {
                    fireStaff.GetComponent<fireStaff>().DisableFireStaff();
                }

                // Enable the Slingshot script only if the selected weapon is the Slingshot
                if (weaponName == "Slingshot")
                {
                    Slingshot.GetComponent<slingshot>().EnableSlingshot();
                    Slingshot.GetComponent<slingshot>().EnableAudio();
                }
                else
                {
                    Slingshot.GetComponent<slingshot>().DisableSlingshot();
                    Slingshot.GetComponent<slingshot>().DisableAudio();
                }
            }
            else
            {
                // Reset the mesh and material of other weapons
                weaponObject.GetComponent<MeshFilter>().sharedMesh = null;
                weaponObject.GetComponent<MeshRenderer>().sharedMaterial = null;
            }
        }
    }

    GameObject GetWeaponObject(string weaponName)
    {
        if (weaponSlots.ContainsKey(weaponName))
        {
            return weaponSlots[weaponName];
        }
        else
        {
            Debug.LogError("Unknown weapon: " + weaponName);
            return null;
        }
    }
    public void saveStats()
    {
        playerStats.weapons = weapons;
    }
    public void loadStats()
    {
        weapons = playerStats.weapons;
    }
}
