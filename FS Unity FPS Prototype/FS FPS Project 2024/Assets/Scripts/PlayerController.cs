using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    public CharacterController controller;
    [SerializeField] AudioSource aud;

    [Header("----- Player Stats-----")]
    [SerializeField] float HP;
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
    [SerializeField] GameObject ThunderHammer;
    [SerializeField] GameObject explosionStaff;
    [SerializeField] int castDamage;
    [SerializeField] float castRate;
    [SerializeField] int castDist;

    [Header("----- Sprinting Stats -----")]
    [SerializeField] float stamina;
    [SerializeField] int maxStamina;
    [SerializeField] int sprintDelay;
    [SerializeField] float sprintDecayRate;
    [SerializeField] int runCost;
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
    [SerializeField] AudioClip[] audDrink;
    [Range(0, 1)] [SerializeField] float audDrinkVol;

    [SerializeField] int rayDistance;

    Dictionary<string, GameObject> weaponSlots = new Dictionary<string, GameObject>();

    Vector3 moveDirection;
    Vector3 playerVelocity;
    bool isSprinting;
    bool playingSteps;
    bool canSprint;
    bool staminaGen;
    public bool godModeActive = false;
    public bool noClipModeActive = false;
    int jumpedTimes;
    int sprintDecayTimes;
    int interactDelay;
    float hpOrig;
    int selectedWeapon;
    float setSpeed;
    public Coroutine recharge;
    int amountOfCooldowns;
    Vector3 previouspos;

    // Start is called before the first frame update
    void Start()
    {
        HP = playerStats.hp;
        hpOrig = HP;
        updatePlayerUI();
        stamina = maxStamina;
        updateStaminaUI();
        canSprint = true;
        currentPoints = gameManager.instance.points;
        setSpeed = speed;
        weaponSlots.Add("Fire Staff", fireStaff);
        weaponSlots.Add("Slingshot", Slingshot);
        weaponSlots.Add("Thunder Hammer", ThunderHammer);
        weaponSlots.Add("Explosion_Staff", explosionStaff);
        loadStats();
        selectedWeapon = playerStats.currentWeapon;
        if (weapons.Count > 0)
        {
            changeWeapon();
        }
        gameObject.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                usePotion();
            }
            selectWeapon();
            movement();
            Sprint();
        }   
    }

    void movement()
    {
        previouspos = transform.position;

        //Movement
        if (noClipModeActive)
        {
            float moveVertical = Input.GetAxis("Vertical");
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveUp = Input.GetAxis("Jump");
            float moveDown = Input.GetAxis("Crouch");

            Vector3 moveDirection = transform.TransformDirection(new Vector3(moveHorizontal, moveUp - moveDown, moveVertical));
            transform.position += moveDirection * speed * Time.deltaTime;
        }
        else
        {
            if (controller.isGrounded)
            {
                jumpedTimes = 0;
                playerVelocity.y -= playerVelocity.y;
                //playerVelocity = Vector3.zero;
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
        }

        if (Input.GetButtonDown("Interact"))
        {
            StartCoroutine(interact());
        }
    }

    public void ToggleNoclipMode()
    {
        noClipModeActive = !noClipModeActive;
    }

    void Sprint()
    {
        if (amountOfCooldowns == 0)
        {
            canSprint = stamina >= 0;
        }

        if (canSprint)
        {
            if (Input.GetButtonDown("Sprint") && transform.position != previouspos)
            {
                isSprinting = true;
                speed *= sprintMultiplier;
            }
            else if (Input.GetButtonUp("Sprint") || transform.position == previouspos)
            {
                isSprinting = false;
                speed = setSpeed;
            }
            if (isSprinting)
            {
                stamina -= runCost * Time.deltaTime;
                if (stamina < 0) stamina = 0;
                if(stamina <= 0)
                {
                    isSprinting=false;
                    speed /= sprintMultiplier;
                }
                updateStaminaUI();
            }
            else
            {
                if (stamina < maxStamina)
                {
                    if (recharge != null) StopCoroutine(recharge);
                    recharge = StartCoroutine(StaminaRegen(staminaToAdd));
                    updateStaminaUI();

                }
            }
        }
        else
        {
            if (isSprinting)
            {
                isSprinting = false;
                speed = setSpeed;
            }
        }
    }

    //Stamina Handler by Paul
    IEnumerator StaminaRegen(float stamRegen)
    {
        while (stamina < maxStamina)
        {
            stamina += stamRegen / 10f;
            if (stamina > maxStamina) { stamina = maxStamina; }
            yield return new WaitForSeconds(.1f);
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
    public void TakeDamage(float amount)
    {
        if (!godModeActive)
        {
            HP -= amount;
            playerStats.hp -= amount;
            aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);
            updatePlayerUI();
            StartCoroutine(flashDamage());
            if (HP <= 0)
            {
                gameManager.instance.lose();
            }
        }
    }
    //hit indicator ienumerator
    IEnumerator flashDamage()
    {
        gameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerDamageScreen.SetActive(false);
    }
    IEnumerator flashHeal()
    {
        gameManager.instance.playerHealScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerHealScreen.SetActive(false);
    }
    //updating ui
    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / getMaxHP();
        
    }
    void updateStaminaUI()
    {
        gameManager.instance.playerStaminaPool.fillAmount = stamina / maxStamina;
    }

    public void spawnPlayer()
    {
        HP = hpOrig;
        playerStats.hp = HP;
        stamina = maxStamina;
        updatePlayerUI();
        selectedWeapon = playerStats.currentWeapon;
        if(weapons.Count > 0)
        {
            changeWeapon();
        }
        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
        gameObject.transform.parent = null;
    }

    public void getWeaponStats(weaponStats weapon)
    {
        weapons.Add(weapon);
        selectedWeapon = weapons.Count - 1;

        if (weapons.Count == 1)
        {
            selectedWeapon = 0;
            changeWeapon();
        }
        else
        {
            selectedWeapon = weapons.Count - 1;
        }

        castDamage = weapon.castDamage;
        castDist = weapon.castDist;
        castRate = weapon.castRate;
        playerStats.currentWeapon = selectedWeapon;
        saveStats();
    }

    public void AddedSpeed(float AddedSpeed)
    {
        speed += AddedSpeed;
    }

    void selectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == "Slingshot")
                {
                    selectedWeapon = i;
                    playerStats.currentWeapon = i;
                    changeWeapon();
                    return;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == "Fire Staff")
                {
                    selectedWeapon = i;
                    playerStats.currentWeapon = i;
                    changeWeapon();
                    return;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == "Thunder Hammer")
                {
                    selectedWeapon = i;
                    playerStats.currentWeapon = i;
                    changeWeapon();
                    return;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].name == "Explosion_Staff")
                {
                    selectedWeapon = i;
                    playerStats.currentWeapon = i;
                    changeWeapon();
                    return;
                }
            }
        }
    }

    void changeWeapon()
    {

        castDamage = weapons[selectedWeapon].castDamage;
        castDist = weapons[selectedWeapon].castDist;
        castRate = weapons[selectedWeapon].castRate;

        foreach (var kvp in weaponSlots)
        {
            string weaponName = kvp.Key;
            GameObject weaponObject = kvp.Value;

            if (weaponName == weapons[selectedWeapon].name)
            {
                weaponObject.GetComponent<MeshFilter>().sharedMesh = weapons[selectedWeapon].weaponModel.GetComponent<MeshFilter>().sharedMesh;
                weaponObject.GetComponent<MeshRenderer>().sharedMaterial = weapons[selectedWeapon].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

                if (weaponName == "Fire Staff")
                {
                    fireStaff.GetComponent<fireStaff>().EnableFireStaff();
                }
                else
                {
                    fireStaff.GetComponent<fireStaff>().DisableFireStaff();
                }

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

                if (weaponName == "Thunder Hammer")
                {
                    ThunderHammer.GetComponent<thunderHammer>().EnableThunderHammer();
                }
                else
                {
                    ThunderHammer.GetComponent<thunderHammer>().DisableThunderHammer();
                }

                if (weaponName == "Explosion_Staff")
                {
                    explosionStaff.GetComponent<explosionStaff>().EnableExplosionStaff();
                }
                else
                {
                    explosionStaff.GetComponent<explosionStaff>().DisableExplosionStaff();
                }
            }
            else
            {
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
    public void addHP(float amount)
    {
        if (HP + amount > getMaxHP())
        {
            HP = getMaxHP();
            playerStats.hp = HP;
        }
        else
        {
            HP+= amount;
            playerStats.hp = HP;
        }
        StartCoroutine(flashHeal());
        updatePlayerUI();
    }
    public void addVelocityY(float amount)
    {
        if (controller.isGrounded)
        {
            playerVelocity.y = amount;
        }
        else
        {
            playerVelocity.y = amount;
        }
        if(playerVelocity.y > 0)
        {
            jumpedTimes=1;
        }
    }
    public float getMaxHP()
    {
        return playerStats.maxHP;
    }
    public void usePotion()
    {
        if (playerStats.potions.Count > 0 && HP < getMaxHP())
        {
            playerStats.potions[0].pickup();
            aud.PlayOneShot(audDrink[Random.Range(0, audDrink.Length)],audDrinkVol);
            playerStats.potions.RemoveAt(0);
            gameManager.instance.updatePotionUi();
            updatePlayerUI();
        }
    }
    public void SprintingHit(bool rUSprinting)
    {
        isSprinting = false;
        canSprint = rUSprinting;
    }
    public float CurrentSpeed()
    {
        return speed;
    }
    public int CooldownReturn()
    {
        return amountOfCooldowns;
    }
    public void CooldownIncrement(int amount)
    {
        amountOfCooldowns += amount;
    }
}
