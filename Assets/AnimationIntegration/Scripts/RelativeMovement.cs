using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private GameObject automatic;
    [SerializeField] private GameObject sword;

    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject pressSpaceText;
    [SerializeField] private float distanceForFinishingEnemy = 9.0f;
    [SerializeField] private float attackSpeed = 10.0f;
    [SerializeField] private float distanceForAttack = 3.0f;

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float gravity = -9.8f;
    public float finishingAnimationTimePhaseOne = .67f;
    public float finishingAnimationTimePhaseTwo = 1.0f;
    public float attackAnimationTime = 0.3f;

    private CharacterController _charController;
    private Animator _animator;

    private bool finishingProcess = false;
    private bool moveToEnemy = false;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        sword.SetActive(false);
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        if (!finishingProcess)
        {
            _animator.SetFloat("Speed", movement.sqrMagnitude);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !finishingProcess)
        {
            finishingProcess = true;
            StartCoroutine(finishingProcessRoutine());
        }

        attackRoutine();

        if (!finishingProcess)
        {
            movement.y = gravity;
            movement *= Time.deltaTime;
            _charController.Move(movement);
        }
    }

    void attackRoutine()
    {
        float distance = Mathf.Pow((transform.position.x - enemy.transform.position.x), 2) + Mathf.Pow((transform.position.z - enemy.transform.position.z), 2);

        if (distance < distanceForFinishingEnemy)
        {
            pressSpaceText.SetActive(true);

            if (moveToEnemy)
            {
                transform.LookAt(enemy.transform);
                if (distance > distanceForAttack)
                {
                    _animator.SetFloat("Speed", 3.0f);
                    transform.Translate(0, 0, attackSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            pressSpaceText.SetActive(false);
        }
    }

    IEnumerator finishingProcessRoutine()
    {
        moveToEnemy = true;

        yield return new WaitForSeconds(attackAnimationTime);

        moveToEnemy = false;

        _animator.SetFloat("Speed", 0);
        _animator.SetBool("Finishing", true);

        automatic.SetActive(false);
        sword.SetActive(true);

        yield return new WaitForSeconds(finishingAnimationTimePhaseOne);

        enemy.SendMessage("playerAttack", SendMessageOptions.DontRequireReceiver);

        yield return new WaitForSeconds(finishingAnimationTimePhaseTwo);

        _animator.SetBool("Finishing", false);

        automatic.SetActive(true);
        sword.SetActive(false);

        finishingProcess = false;
    }
}
