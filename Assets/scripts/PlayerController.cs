using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public EnemyAiTutorial Enemy;
    public int MaxHealth = 100, Health;

    //[SerializeField] private floathealDuration = 1f; // Duration of the heal animation

    void Start()
    {
        Health = MaxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Enemy.TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) Invoke(nameof(DestroyPlayer), 0.5f);
    }

    private void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    public void HealthPack()
    {
        int targetHealth = Mathf.Min((int)(MaxHealth * 0.75f), MaxHealth); // 75% of MaxHealth
        StartCoroutine(SmoothHeal(targetHealth,1f));
    }

    public void Medpack()
    {
        StartCoroutine(SmoothHeal(MaxHealth, 1f)); // 100% of MaxHealth
    }

    public void PainKiller()
    {
        int targetHealth = Mathf.Min(Health + (int)(MaxHealth * 0.25f), MaxHealth); // Increase by 25%
        StartCoroutine(SmoothHeal(targetHealth, 5f));
    }

    public void EnergyDrink()
    {
        int targetHealth = Mathf.Min(Health + (int)(MaxHealth * 0.10f), MaxHealth); // Increase by 10%
        StartCoroutine(SmoothHeal(targetHealth, 5f));
    }

    private IEnumerator SmoothHeal(int targetHealth, float healDuration)
    {
        float elapsedTime = 0f;
        int startHealth = Health;

        while (elapsedTime < healDuration)
        {
            elapsedTime += Time.deltaTime;
            Health = (int)Mathf.Lerp(startHealth, targetHealth, elapsedTime / healDuration);
            yield return null; // Wait for the next frame
        }

        Health = targetHealth; // Ensure exact final value
    }
}
