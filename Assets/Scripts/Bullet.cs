using UnityEngine;

public class Bullet : MonoBehaviour
{
    private readonly float spawnSpeed = 10f;
    public float distance = 10f;
    public int damage = 3;
    public bool isGrenade = false;
    public bool isRange = true;
    public Rigidbody2D rb;
    public GameObject GrenadeRange;
    private void Start()
    {
        OnInstantiate();
    }
    public void OnInstantiate()
    {
        Invoke(nameof(DestroyBullet), FlightTime() == 0 ? 2f : FlightTime());
    }
    float FlightTime()
    {
        float destroyTime;
        
        destroyTime = distance / spawnSpeed;
        return destroyTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") & !isGrenade)
        {
            collision.GetComponent<Enemy>().GetDamage(damage);
            DestroyBullet();
        }
    }
    void DestroyBullet()
    {
        if (isGrenade)
        {
            GameObject _grenadeRange = Instantiate(GrenadeRange, this.gameObject.transform.position, Quaternion.identity, GameObject.FindWithTag("Based").transform);
            _grenadeRange.GetComponent<GrenadeRange>().damage = damage;
        }
        
        Destroy(this.gameObject);
    }
}
