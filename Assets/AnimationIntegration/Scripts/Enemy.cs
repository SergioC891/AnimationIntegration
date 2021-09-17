using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int minWorldX = -30;
    public int maxWorldX = 30;

    public int minWorldZ = -30;
    public int maxWorldZ = 30;

    public float reappearDelay = 5.0f;
    public float attackDistance = 3.0f;

    public GameObject player;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void playerAttack()
    {
        float distance = Mathf.Pow((transform.position.x - player.transform.position.x), 2) + Mathf.Pow((transform.position.z - player.transform.position.z), 2);

        if (distance <= attackDistance)
        {
            StartCoroutine(finishEnemyRoutine());
        }
    }

    IEnumerator finishEnemyRoutine()
    {
        _animator.enabled = false;
        
        yield return new WaitForSeconds(reappearDelay);

        _animator.enabled = true;

        transform.position = new Vector3(Random.Range(minWorldX, maxWorldX), 0.0f, Random.Range(minWorldZ, maxWorldZ));
    }
}
