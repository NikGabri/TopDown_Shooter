using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject target;

    private void LateUpdate()
    {
        LookAtPlayer();   
    }
    public void LookAtPlayer()
    {
        if (target != null)
        {
            float target_x = Mathf.Clamp(target.transform.position.x, -6f, 6f);
            float target_y = Mathf.Clamp(target.transform.position.y, -7f, 7f);
            this.gameObject.transform.position = new Vector3(target_x, target_y, -10f);
        }
    }
}
