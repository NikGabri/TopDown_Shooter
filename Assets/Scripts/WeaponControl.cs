using UnityEngine;
using UnityEngine.UI;
public class WeaponControl : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject baseCanvas;
    private float nextFireTime;
    public float fireRate = 2f;
    public int damage;
   
    public enum Types
    {
        pistol,
        rifle,
        shotgun,
        grenadeLauncher
    };
    public Types WeaponType;
    public void Shoot()
    {
        if ( Time.time > nextFireTime)
        {
            nextFireTime = Time.time + (1f / fireRate);
            if (WeaponType == Types.pistol)
            {
                GameObject bullet = Instantiate(bulletPrefab, this.gameObject.transform.position, GameObject.FindWithTag("Player").transform.rotation, baseCanvas.transform);
                bullet.GetComponent<Image>().color = this.GetComponent<Image>().color;
                float spawnSpeed = 10f;
                bullet.GetComponent<Bullet>().rb.velocity = bullet.transform.up.normalized * spawnSpeed;
                bullet.GetComponent<Bullet>().damage = damage;
            }
            if (WeaponType == Types.rifle)
            {
                GameObject bullet = Instantiate(bulletPrefab, this.gameObject.transform.position, GameObject.FindWithTag("Player").transform.rotation, baseCanvas.transform);
                bullet.GetComponent<Image>().color = this.GetComponent<Image>().color;
                float spawnSpeed = 10f;
                bullet.GetComponent<Bullet>().rb.velocity = bullet.transform.up.normalized * spawnSpeed;
                bullet.GetComponent<Bullet>().damage = damage;
            }
            if (WeaponType == Types.shotgun)
            {
                float deltaSpread = -10;
                for (int x = 0; x < 5; x++)
                {
                    
                    Quaternion bulRot = Quaternion.Euler(0, 0, GameObject.FindWithTag("Player").transform.rotation.eulerAngles.z + deltaSpread );
                    deltaSpread += 4f;
                    GameObject bullet = Instantiate(bulletPrefab, this.gameObject.transform.position, bulRot, baseCanvas.transform);
                    float spawnSpeed = 10f;
                    bullet.GetComponent<Bullet>().rb.velocity = bullet.transform.up.normalized * spawnSpeed;
                    bullet.GetComponent<Image>().color = this.GetComponent<Image>().color;
                    bullet.GetComponent<Bullet>().distance = 7f;
                    bullet.GetComponent<Bullet>().damage = damage;
                }
            }
            if (WeaponType == Types.grenadeLauncher)
            {
                Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;
                GameObject bullet = Instantiate(bulletPrefab, this.gameObject.transform.position, Quaternion.Euler(0f, 0f, rotateZ - 90f), baseCanvas.transform);
                Bullet _bulletController = bullet.GetComponent<Bullet>();
                bullet.GetComponent<Image>().color = this.GetComponent<Image>().color;
                float spawnSpeed = 10f;
                _bulletController.rb.velocity = bullet.transform.up.normalized * spawnSpeed;
                _bulletController.isGrenade = true;
                _bulletController.distance = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),transform.position);
                bullet.GetComponent<Bullet>().damage = damage;
            }
        }
    }
    
     
    
}
