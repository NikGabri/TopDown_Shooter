using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public int healthValue;
    private void Update()
    {
        this.GetComponent<Slider>().value = Mathf.Clamp(healthValue, 0, 100);
        transform.position = new Vector2(transform.parent.position.x, transform.parent.position.y + 1f);
        transform.up = Vector2.up;
    }
}
