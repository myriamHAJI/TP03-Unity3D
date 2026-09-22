using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int attackDamage = 10;

    public int CurrentHealth { get; private set; }

    void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
{
    if (CurrentHealth <= 0)
        return;

    CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);
    Debug.Log(name + " : " + CurrentHealth + "/" + maxHealth + " PV");
}
}