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

    [SerializeField] int rayDistance;

    [SerializeField] int currentPoints;

    [SerializeField] GameObject cube;

    Vector3 moveDirection;
    Vector3 playerVelocity;
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
        currentPoints = gameManager.instance.points;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.red);
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
        if (Input.GetButtonDown("Interact"))
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
        Debug.Log("HandlePointChange Called");
        currentPoints = currentPoints + newPoints;
        Debug.Log("Player gained " + newPoints + " points.");
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
        }
        yield return new WaitForSeconds(interactDelay);
    }
    //checks if you are near an object that you can interact with
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interact"))
        {
            gameManager.instance.showInteractText();
        }
        else if (other.gameObject.CompareTag("pickup"))
        {
            IPickup pickup = other.GetComponent<IPickup>();
            pickup.pickup();
        }
    }
    //checks if you left the area of an object that you can interact with
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interact"))
        {
            gameManager.instance.hideInteractText();
        }
    }
}
