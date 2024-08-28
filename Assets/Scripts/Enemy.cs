using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject Player_target;
    public int _health = 100;
    public int Health
    {
        private set
        {
            _health = value;
            _health = Mathf.Clamp(_health, 0, 100);
        }
        get
        {
            return _health;
        }
    }

    public HealthBar _healthBar;

    public enum Types
    {
        Soldier = 7,
        Speedy = 12,
        Armored = 30
    }
    public Types EnemyType;

     public float speed = 2f;

    private void Start()
    {
        Player_target = GameObject.FindWithTag("Player");
        _healthBar.gameObject.GetComponent<Slider>().maxValue = _health;
    }

    private void FixedUpdate()
    {

        if (Player_target != null)
        {
            MoveToTarget(Vector3.MoveTowards( this.gameObject.transform.position, Player_target.transform.position, speed * Time.deltaTime));
            this.gameObject.transform.up =  (Player_target.transform.position - this.gameObject.transform.position).normalized;
        }
        HealthIndication();
    }

    public void MoveToTarget(Vector3 target)
    {
        this.GetComponent<Rigidbody2D>().MovePosition(target);

    }
    public void HealthIndication()
    {
     _healthBar.healthValue = Health;
    }

    

    public void GetDamage(int damage)
    {
        Health -= damage;
        if (Health == 0)
        {
            GameManager.AddScore((int)EnemyType);
            Destroy(this.gameObject);
        }
    }

}
