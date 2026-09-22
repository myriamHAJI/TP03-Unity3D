using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private Animator animator;
    private CharacterStats monsterStats;

    void Awake()
    {
        animator = GetComponentInParent<Animator>();
        monsterStats = GetComponentInParent<CharacterStats>();
    }

    void OnTriggerEnter(Collider other)
    {
        CharacterStats target = other.GetComponent<CharacterStats>();

        if (target == null || target == monsterStats)
            return;

        animator.SetTrigger("Attack");
        target.TakeDamage(monsterStats.attackDamage);
    }
}