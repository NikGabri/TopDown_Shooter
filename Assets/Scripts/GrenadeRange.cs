using UnityEngine;

public class GrenadeRange : MonoBehaviour
{
    public int damage;
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().GetDamage(damage);
        }
    }
}
