using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region References
    Animator animator;
    Rigidbody rb;
    Camera cam;
    PlayerStats stats;
    bool isLeftTriggerUsed = false;
    bool isRightTriggerUsed = false;
    #endregion

    #region PlayerMovement
    [Header("PlayerMovement")]
    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float vel = 0;
    Vector3 moveDir = Vector3.zero;
    Vector3 dir = Vector3.zero;
    #endregion

    #region Attack
    [Header("Attack")]
    public List<string> basicAttackComboTriggerNames;
    int basicAttackCount = 0;
    bool isAttacking = false;
    public float basicAttackCooldown = 0.5f;
    public float basicAttackStaminaDrainAmount = -5f;
    #endregion

    #region Roll
    [Header("Roll")]
    public float rollCooldown = 1f;
    public float rollStaminaDrainAmount = -10f;
    bool isRolling = false;
    #endregion

    #region Block
    [Header("Block")]
    bool isBlocking = false;
    #endregion

    #region GroundCheck
    [Header("GroundCheck")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;
    public float jumpForce = 10f;
    public float gravity = 10f;
    public float jumpStaminaDrainAmount = -5f;
    bool isGrounded = false;
    #endregion

    #region Sprint
    [Header("Sprint")]
    public float reduceSprintStamCooldown = 0.25f;
    public float sprintStamDrainAmount = -0.1f;
    bool isSprinting = false;
    bool hasReducedSprintStam = false;
    #endregion

    #region Kick
    [Header("Kick")]
    public float kickCooldown = 0.5f;
    public float kickStaminaDrainAmount = -1f;
    bool isKicking = false;
    #endregion

    #region Impact
    [Header("Impact")]
    public List<string> impactTriggerNames;
    #endregion

    #region Death
    [Header("Death")]
    public List<string> deathTriggerNames;
    bool isAlive = true;
    #endregion

    #region Stamina
    [Header("Stamina")]
    public float staminaRegenAmount = 0.5f;
    public float staminaRegenCooldown = 1f;
    public float staminaRegenDelay = 3f;
    bool isStaminaRegenOnCooldown = false;
    bool canStartStaminaRegen = false;
    bool hasRegenStarted = false;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        stats = GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
            //rb.velocity += gravity * Vector3.down * Time.deltaTime;
        }
    }

    void Update()
    {
        if (isAlive && !UIManager.instance.isMenuOpen)
        {
            if (stats.currentStam <= 0)
            {
                animator.SetBool("isBlocking", false);
                isBlocking = false;
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);

            PlayerMovement();

            Block();

            if (!isRolling)
            {
                if (!isBlocking)
                {
                    BasicAttack();
                }

                if (isGrounded)
                {
                    Interact();

                    if (!isAttacking)
                    {
                        DodgeRoll();

                        if (!isKicking)
                        {
                            Kick();
                        }
                    }

                    Jump();
                }
            }

            Sprint();

            StaminaRegen();

            if (Input.GetButtonDown("LB"))
            {
                //Debug.Log("Lstick pressed");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.ToggleOptionsMenu();

        }

        //debug cheat
        if (Input.GetKeyDown(KeyCode.R))
        {
            stats.UpdateCurrentStamValue(100);
            stats.UpdateCurrentHPValue(100);

            if (!isAlive)
            {
                animator.SetTrigger("backAlive");
                isAlive = true;
            }
        }
    }

    #region StaminaRegen
    void StaminaRegen()
    {
        if(!isSprinting && !isBlocking)
        {
            if (!canStartStaminaRegen && stats.currentStam < stats.maxStam)
            {
                canStartStaminaRegen = true;
                StartCoroutine(nameof(StartStaminaRegen));
            }

            if (!isStaminaRegenOnCooldown && stats.currentStam < stats.maxStam && hasRegenStarted)
            {
                if (!isSprinting || !isBlocking)
                {
                    isStaminaRegenOnCooldown = true;
                    stats.UpdateCurrentStamValue(staminaRegenAmount);
                    StartCoroutine(nameof(StaminaRegenCooldown));
                }
            }
        }

        if (stats.currentStam >= stats.maxStam && canStartStaminaRegen)
        {
            PauseStaminaRegen();
        }
    }
    void PauseStaminaRegen()
    {
        StopCoroutine(nameof(StartStaminaRegen));
        StopCoroutine(nameof(StaminaRegenCooldown));
        canStartStaminaRegen = false;
        isStaminaRegenOnCooldown = false;
        hasRegenStarted = false;
    }
    IEnumerator StartStaminaRegen()
    {
        yield return new WaitForSeconds(staminaRegenDelay);
        hasRegenStarted = true;
    }
    IEnumerator StaminaRegenCooldown()
    {
        yield return new WaitForSeconds(staminaRegenCooldown);
        isStaminaRegenOnCooldown = false;
    }
    #endregion

    void TakeDamage(float amount)
    {
        if (isAlive)
        {
            if (isBlocking)
            {
                stats.UpdateCurrentStamValue(amount*2);

                animator.SetTrigger("blockedImpact");
            }
            else
            {
                stats.UpdateCurrentHPValue(amount);

                if (stats.currentHP <= 0)
                {
                    isAlive = false;
                    int t = Random.Range(0, deathTriggerNames.Count);
                    animator.SetTrigger(deathTriggerNames[t]);
                }
                else
                {
                    int r = Random.Range(0, impactTriggerNames.Count);
                    animator.SetTrigger(impactTriggerNames[r]);
                }
            }
        }
    }

    #region PlayerControls
    void Sprint()
    {
        if (stats.currentStam > sprintStamDrainAmount)
        {
            if (Input.GetButtonDown("L3") || Input.GetKeyDown(KeyCode.LeftShift))
            {
                isSprinting = !isSprinting;
                PauseStaminaRegen();
            }
        }
        else if (isSprinting)
        {
            isSprinting = false;
            StopCoroutine(nameof(ReduceSprintStamCooldown)); // probably can get rid of this ?
            hasReducedSprintStam = false;
        }

        if (isSprinting && vel >= 0.8f && !hasReducedSprintStam)
        {
            hasReducedSprintStam = true;
            stats.UpdateCurrentStamValue(sprintStamDrainAmount);
            StartCoroutine(nameof(ReduceSprintStamCooldown));
        }
    }
    IEnumerator ReduceSprintStamCooldown()
    {
        yield return new WaitForSeconds(reduceSprintStamCooldown);
        hasReducedSprintStam = false;
    }

    void Kick()
    {
        if (stats.currentStam >= Mathf.Abs(kickStaminaDrainAmount))
        {
            if (Input.GetButtonDown("Fire3") || Input.GetKeyDown(KeyCode.F))
            {
                isKicking = true;
                StartCoroutine(KickCooldown());
                animator.SetTrigger("kick");
                stats.UpdateCurrentStamValue(kickStaminaDrainAmount);
                PauseStaminaRegen();
            }
        }
    }
    private IEnumerator KickCooldown()
    {
        yield return new WaitForSeconds(kickCooldown);
        isKicking = false;
    }

    void Jump()
    {
        if (stats.currentStam >= Mathf.Abs(jumpStaminaDrainAmount))
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                animator.SetTrigger("jump");
                stats.UpdateCurrentStamValue(jumpStaminaDrainAmount);
                PauseStaminaRegen();
            }
        }
    }

    void Interact()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("Fire1 pressed");
            animator.SetTrigger("interact");
        }
    }

    void Block()
    {
        if (stats.currentStam > 0)
        {
            LTriggerPulled();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                PauseStaminaRegen();
            }
            if (Input.GetKey(KeyCode.Q) || isLeftTriggerUsed)
            {
                animator.SetBool("isBlocking", true);
                isBlocking = true;
            }
            else if (Input.GetKeyUp(KeyCode.Q) || !isLeftTriggerUsed)
            {
                animator.SetBool("isBlocking", false);
                isBlocking = false;
            }
        }
    }

    void BasicAttack()
    {
        
        if (!isAttacking && stats.currentStam >= Mathf.Abs(basicAttackStaminaDrainAmount))
        {
            if (RTriggerPulled())
            {
                isAttacking = true;
                StartCoroutine(AttackCooldown());
                animator.SetTrigger(basicAttackComboTriggerNames[basicAttackCount]);
                basicAttackCount++;

                if (basicAttackCount >= basicAttackComboTriggerNames.Count)
                {
                    basicAttackCount = 0;
                }

                stats.UpdateCurrentStamValue(basicAttackStaminaDrainAmount);
                PauseStaminaRegen();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                isAttacking = true;
                StartCoroutine(AttackCooldown());
                animator.SetTrigger(basicAttackComboTriggerNames[basicAttackCount]);
                basicAttackCount++;

                if (basicAttackCount >= basicAttackComboTriggerNames.Count)
                {
                    basicAttackCount = 0;
                }

                stats.UpdateCurrentStamValue(basicAttackStaminaDrainAmount);
                PauseStaminaRegen();
            }
        }
    }
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(basicAttackCooldown);
        isAttacking = false;
    }

    void DodgeRoll()
    {
        if (stats.currentStam >= Mathf.Abs(rollStaminaDrainAmount))
        {
            if (Input.GetButtonDown("Fire2") && dir.magnitude > 0.1f)
            {
                isRolling = true;
                //Debug.Log("Fire2 pressed");
                animator.SetTrigger("roll");
                StartCoroutine(RollCooldown());
                stats.UpdateCurrentStamValue(rollStaminaDrainAmount);
                PauseStaminaRegen();
            }
        }
    }
    private IEnumerator RollCooldown()
    {
        yield return new WaitForSeconds(rollCooldown);
        isRolling = false;
    }

    void PlayerMovement()
    {
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        dir = new Vector3(hor, 0f, vert).normalized;

        vel = Mathf.Abs(hor) + Mathf.Abs(vert);

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (isSprinting)
            {
                isSprinting = false;
            }

            if (vel > 0.25f)
            {
                vel = 0.25f;
            }
        }
        else
        {
            if (isSprinting)
            {
                if (vel >= 1.2f)
                {
                    vel = 1.2f;
                }
            }
            else
            {
                if (vel > 0.8f)
                {
                    vel = 0.8f;
                }
            }
        }
        

        if (dir.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += vel * speed * Time.deltaTime * moveDir.normalized;

            animator.SetFloat("vel", vel);
        }
        else
        {
            animator.SetFloat("vel", 0);
            isSprinting = false;
        }
    }
    #endregion

    #region Triggers
    private bool LTriggerPulled()
    {
        if (!isLeftTriggerUsed && Input.GetAxis("LTrigger") >= 0.1f)
        {
            isLeftTriggerUsed = true;
            PauseStaminaRegen();
            return true;
        }
        else if (isLeftTriggerUsed && Input.GetAxis("LTrigger") < 0.1f)
        {
            isLeftTriggerUsed = false;
        }
        return false;
    }

    private bool RTriggerPulled()
    {
        if (!isRightTriggerUsed && Input.GetAxis("RTrigger") >= 0.1f)
        {
            isRightTriggerUsed = true;
            return true;
        }
        else if (isRightTriggerUsed && Input.GetAxis("RTrigger") < 0.1f)
        {
            isRightTriggerUsed = false;
        }
        return false;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            if (other.gameObject.CompareTag("Trap"))
            {
                if(!isRolling)
                {
                    TakeDamage(stats.damageTakenFromTraps);
                }
            }
        }
    }
}
