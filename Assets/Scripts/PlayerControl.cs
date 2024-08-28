using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private int _health = 100;
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

    public GameObject[] Weapons_Array;

    public float MoveSpeed = 4f;
    public float RotateSpeed = 180f; //degrees in sec
    public float SpeedFromBonus = 1f;

    private Vector3 deltaPosition;

    public GameObject Weapon;
    public GameObject Cross;
    public HealthBar _healthBar;

    public bool isInfintyHealth = false;
    public bool isSpeedBoost = false;
    public bool isLowSpeedZone = false;

    public Color _playerColor;

    public GameObject pistol_bullet;

    public GameManager _gameController;

    private void FixedUpdate()
    {
        MovePlayer();
        HealthIndication();
        Cross.transform.position = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        OnShoot();
    }

    public void MovePlayer()
    {
        SpeedFromBonus = isSpeedBoost? 1.5f : (isLowSpeedZone? 0.6f : 1);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");            
        deltaPosition = new(horizontalInput, verticalInput, 0f);
        this.GetComponent<Rigidbody2D>().MovePosition(Vector3.MoveTowards(this.transform.position, this.transform.position + deltaPosition, MoveSpeed * SpeedFromBonus * Time.deltaTime));
        RotatePlayer();
    }
    public void RotatePlayer()
    {
        Vector3 lastPosition = this.transform.position;
        Vector3 mouseClickPosition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (!Input.GetMouseButton(0))
        {
            if (lastPosition != this.transform.position + deltaPosition)
            {
                Quaternion target = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2((this.transform.position.y + deltaPosition.y) - lastPosition.y,( this.transform.position.x + deltaPosition.x) - lastPosition.x) * Mathf.Rad2Deg - 90);
                this.transform.rotation = (Quaternion.RotateTowards(transform.rotation, target, RotateSpeed * Time.fixedDeltaTime));
            }
        }
        else
        {
            Quaternion target = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(mouseClickPosition.y - transform.position.y, mouseClickPosition.x - transform.position.x) * Mathf.Rad2Deg - 90);
            this.transform.rotation =  (Quaternion.RotateTowards(transform.rotation, target, RotateSpeed * Time.fixedDeltaTime));
        }
    }
    public void HoldWeapon()
    {
        if (Weapon != null)
        {
            Weapon.transform.position = this.transform.GetChild(0).position;
            Weapon.transform.up = this.transform.up;
        }
    }
    public void HealthIndication()
    {
        if (!isInfintyHealth)
            _healthBar.healthValue = Health;
        else _healthBar.healthValue = 100;
    }
    public void GetDamage(int damage)
    {
        if (!isInfintyHealth)
        Health -= damage;
    }
    public void Die()
    {
        if (!isInfintyHealth)
        {
            Health = 0;
            Destroy(Weapon);
            _gameController.GameOver();
            Destroy(this.gameObject);            
        }
    }
    public void OnShoot()
    {
        if (Input.GetMouseButton(0))
        {
            if (Weapon != null)
            {
               Weapon.GetComponent<WeaponControl>().Shoot();
            }
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {

            if (Weapon != null)
            {
                GameManager._weaponIsExist[((int)Weapon.GetComponent<WeaponControl>().WeaponType)] = false;
                Weapon.SetActive(false);
            }
            Weapon = Weapons_Array[((int)other.GetComponent<WeaponControl>().WeaponType)];
            Weapon.SetActive(true);
            GameManager._weaponIsExist[((int)other.GetComponent<WeaponControl>().WeaponType)] = true;
            Destroy(other.gameObject);
           
        }
        if (other.CompareTag("Enemy"))
        {
            Die();
        }
    }

    public void SetPlayerColor(float r, float g, float b, float a)
    {
        this.gameObject.GetComponent<Image>().color = new(r, g, b, a);
    }
    public void SetPlayerColor(Color newColor)
    {
        this.gameObject.GetComponent<Image>().color = newColor;
    }

    IEnumerator BonusTimer(BonusAndZonesControl.Types Bonus_type)
    {
        yield return new WaitForSeconds(10f);
        if (Bonus_type == BonusAndZonesControl.Types.speedBoost)
        {
            isSpeedBoost = false;
            SetPlayerColor(_playerColor);
        }
        if (Bonus_type == BonusAndZonesControl.Types.invulnerability)
        {
            isInfintyHealth = false;
            SetPlayerColor(_playerColor);
        }
        StopAllCoroutines();     
    }
}
