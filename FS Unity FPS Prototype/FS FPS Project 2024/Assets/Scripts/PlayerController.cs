using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] int HP;
    [SerializeField] float defaultWalkSpeed;
    [SerializeField] float sprintMultiplier;
    [SerializeField] float speed;
    [SerializeField] int jumpHeight;
    [SerializeField] int maxJumps;
    [SerializeField] int gravity;

    [SerializeField] int stamina;
    [SerializeField] int maxStamina;
    [SerializeField] int sprintDelay;
    [SerializeField] float sprintDecayRate;
    [SerializeField] int staminaToRemove;
    [SerializeField] int sprintRegenDelay;
    [SerializeField] float sprintRegenRate;
    [SerializeField] int staminaToAdd;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    [SerializeField] int currentPoints;

    [SerializeField] GameObject cube;

    Vector3 moveDirection;
    Vector3 playerVelocity;
    bool isShooting;
    bool isSprinting;
    bool canSprint;
    int jumpedTimes;
    int sprintDecayTimes;
    int interactDelay;
    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;
        canSprint = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
        movement();

        
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

        // Shooting 
        if (Input.GetButton("Shoot") && !isShooting)
        {
            StartCoroutine(Shoot());
        }

        //Sprinting implemented by Paul
        if(Input.GetButtonDown("Sprint") && !isSprinting)
        {
            StartCoroutine(Sprint(sprintDecayRate));
        }

        if(stamina <= 0)
        {
            canSprint = false;
            StartCoroutine(StaminaRegen(sprintRegenRate));
        }
        if (Input.GetButtonDown("Fire3"))
        {
            StartCoroutine(interact());
        }
    }
    private void OnEnable()
    {
        PointsManager.Instance.OnPointChange += HandlePointChange;
    }

    private void OnDisable()
    {
        PointsManager.Instance.OnPointChange -= HandlePointChange;
    }
    

    // Basic shooting added by Matt
    IEnumerator Shoot()
    {
        isShooting = true;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
        {

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null && hit.transform != transform)
            {
                dmg.TakeDamage(shootDamage);
            }
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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


    private void HandlePointChange(int newPoints)
    {
        currentPoints += newPoints;
    }

    //interact handler by Ben
    IEnumerator interact()
    {
        interactDelay = 1;
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f,0.5f)),out hit,5))
        {
            IfInteract interact = hit.collider.GetComponent<IfInteract>();
            interact.interact();
            gameManager.Instance.hideInteractText();
        }
        yield return new WaitForSeconds(interactDelay);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interact"))
        {
            gameManager.Instance.showInteractText();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interact"))
        {
            gameManager.Instance.hideInteractText();
        }
    }
}
