using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    public CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] float defaultWalkSpeed;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float speed;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxJumps;
    [SerializeField] int gravity;
    [SerializeField] int currentPoints;

    [SerializeField] List<weaponStats> weapons = new List<weaponStats>();
    [SerializeField] GameObject fireStaff;
    [SerializeField] int castDamage;
    [SerializeField] float castRate;
    [SerializeField] int castDist;

    [SerializeField] int stamina;
    [SerializeField] int maxStamina;
    [SerializeField] int sprintDelay;
    [SerializeField] float sprintDecayRate;
    [SerializeField] int staminaToRemove;
    [SerializeField] int sprintRegenDelay;
    [SerializeField] float sprintRegenRate;
    [SerializeField] int staminaToAdd;

    [SerializeField] int rayDistance;

    Vector3 moveDirection;
    Vector3 playerVelocity;
    bool isSprinting;
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
            jumpedTimes++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

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

    //Sprint by Paul
    IEnumerator Sprint(float stamDecay)
    {
        if (canSprint && stamina > 5)
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

        castDamage = weapon.castDamage;
        castDist = weapon.castDist;
        castRate = weapon.castRate;

        //fireStaff
        fireStaff.GetComponent<MeshFilter>().sharedMesh = weapon.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        fireStaff.GetComponent<MeshRenderer>().sharedMaterial = weapon.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;

        //Add other weapons here
    }

    void selectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && selectedWeapon < weapons.Count - 1)
        {
            changeWeapon();
        }

        //Add Other Weapons Here
    }

    void changeWeapon()
    {
        castDamage = weapons[selectedWeapon].castDamage;
        castDist = weapons[selectedWeapon].castDist;
        castRate = weapons[selectedWeapon].castRate;

        fireStaff.GetComponent<MeshFilter>().sharedMesh = weapons[selectedWeapon].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        fireStaff.GetComponent<MeshRenderer>().sharedMaterial = weapons[selectedWeapon].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

}
