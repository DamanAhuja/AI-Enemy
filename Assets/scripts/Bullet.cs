using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        GetComponent<Collider>().enabled = false; // Disable collider to prevent multiple triggers
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true; // Stop movement

        if (collision.gameObject.CompareTag("Head"))
        {
            Debug.Log("Head");
            GameStatistics.Instance.HeadshotCount += 1;
            ApplyDamage(collision.gameObject, 50);
        }
        else if (collision.gameObject.CompareTag("Torso"))
        {
            Debug.Log("Torso");
            GameStatistics.Instance.BodyshotCount += 1;
            ApplyDamage(collision.gameObject, 30);
        }
        else if (collision.gameObject.CompareTag("Leg"))
        {
            Debug.Log("Leg");
            ApplyDamage(collision.gameObject, 10);
        }

        Destroy(gameObject, 0.1f);  // Small delay before destruction
    }


    void ApplyDamage(GameObject target, int damage)
    {
        GameStatistics.Instance.ShotsHit += 1;
        EnemyAiTutorial enemy = target.GetComponentInParent<EnemyAiTutorial>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            GameStatistics.Instance.Damage += damage;
        }
    }

}
