using UnityEngine;

public class BonusAndZonesControl : MonoBehaviour
{
    PlayerControl _playerScript;
    private void Start()
    {
        if ((BonusOrZone_types == Types.invulnerability)|| (BonusOrZone_types == Types.speedBoost))
            Invoke(nameof(DestroyBonus), 5f);
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }
    public enum Types
    {
        lowSpeedZone,
        dieZone,
        invulnerability,
        speedBoost
    };
    public Types BonusOrZone_types;

    private void DestroyBonus()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D _player)
    {
        if (_player.CompareTag("Player"))
        {
            switch (BonusOrZone_types)
            {
                case Types.lowSpeedZone:
                    {
                        _playerScript.isLowSpeedZone = true;
                        break;
                    }
                case Types.dieZone:
                    {
                        _playerScript.Die();
                        break;
                    }
                case Types.invulnerability:
                    {
                        _playerScript.StartCoroutine("BonusTimer", Types.invulnerability);
                        _playerScript.isInfintyHealth = true;
                        _playerScript.SetPlayerColor(0f/255f,234f/255f,11f/255f,255f);
                        Destroy(this.gameObject);
                        break;
                    }
                case Types.speedBoost:
                    {
                        _playerScript.StartCoroutine("BonusTimer", Types.speedBoost);
                        _playerScript.isSpeedBoost = true;
                        _playerScript.SetPlayerColor(227f/255f, 234f/255f, 0f/255f, 255f);                        
                        Destroy(this.gameObject);
                        break;
                    }
                default: break;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D _player)
    {
        if (_player.CompareTag("Player") && (BonusOrZone_types == Types.lowSpeedZone))
            _playerScript.isLowSpeedZone = false;
    }
}
