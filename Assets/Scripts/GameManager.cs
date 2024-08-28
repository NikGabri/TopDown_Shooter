using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public TMP_Text Score_text;
    public static int _score = 0;
    public float bonusSpawnTime = 27f;
    public float weaponSpawnTime = 10f;
    public float enemySpawnTime = 2f;
    public Transform zoneAndBonusSpawnHolder;
    public Transform enemySpawnHolder;
    public GameObject[] Enemys;
    public GameObject[] Weapon;
    public GameObject[] Bonus;
    public GameObject[] Zone;
    public float ZoneInterval = 0;
    [SerializeField]
    public static bool[] _weaponIsExist = {false, false, false, false};
    public int dieZoneCount = 2;
    private int dieZoneSpawned = 0;
    public int lowSpeedZoneCount = 3;
    private int lowSpeedZoneSpawned = 0;

    public GameObject gameOverPanel;

    public Transform menuButton;
    public Transform restartButton;
    public Transform scoreText;
    public Transform menuButtonHolder;
    public Transform restartButtonHolder;
    public Transform scoreHolder;

    public float speedButtonAnimate = 0.5f;

    public Camera _mainCamera;
    
    
    void Start()
    {
        Time.timeScale = 1;
        _mainCamera = Camera.main;        
        _score = 0;
        StopAllCoroutines();
        StartCoroutine("SpawnEnemys");
        StartCoroutine("SpawnWeapon");
        StartCoroutine("SpawnBonus");
        SpawnZone(0);
        SpawnZone(1);
    }
      
    void Update()
    {
        ScoreView();
    }


    IEnumerator SpawnEnemys()
    {
        float spawnTime = enemySpawnTime;
        float timer = 0f;
        int flipSide = 1;
        int flipSide_2 = 1;
        while (true)
        {             
            if (timer / 10 >= 1)
            {
                spawnTime -= 0.1f;
                spawnTime = Mathf.Clamp(spawnTime, 0.5f, enemySpawnTime);
            }
                float height = _mainCamera.orthographicSize + 1;
            float width = _mainCamera.orthographicSize * _mainCamera.aspect + 1;

            System.Random rand = new System.Random();

            int enemyIndex = 0;
            if (rand.NextDouble() < 0.6)
            {
                enemyIndex = 0;
            }
            else 
            if (rand.NextDouble() < 0.9)
            {
                enemyIndex = 1;
            }
            else
            {
                enemyIndex = 2;
            }

            flipSide = (new System.Random().NextDouble()) < 0.5 ? -1 : 1;
            flipSide_2 = (new System.Random().NextDouble()) < 0.5 ? 1 : -1;

            float randWidthPos = UnityEngine.Random.Range(-width * flipSide * flipSide_2, width * flipSide_2);
            float randHeightPos = UnityEngine.Random.Range(height * flipSide *flipSide_2, height *flipSide_2);

            Instantiate(Enemys[enemyIndex], new Vector3(_mainCamera.transform.position.x + randWidthPos, _mainCamera.transform.position.y + randHeightPos, 0), Quaternion.identity, enemySpawnHolder);
            
            yield return new WaitForSeconds(spawnTime);
            timer += spawnTime;
        }
    }

    IEnumerator SpawnWeapon()
    {
        while (true)
        {
            if (!_weaponIsExist[0] || !_weaponIsExist[1] || !_weaponIsExist[2] || !_weaponIsExist[3])
            {
                int weaponIndex = 1;

                System.Random rand = new System.Random();
                if (rand.NextDouble() < 0.25 && !_weaponIsExist[0])
                {
                    weaponIndex = 0;
                }
                else
                if (rand.NextDouble() < 0.5 && !_weaponIsExist[1])
                {
                    weaponIndex = 1;
                }
                else
                if (rand.NextDouble() < 0.75 && !_weaponIsExist[2])
                {
                    weaponIndex = 2;
                }
                else
                    if (!_weaponIsExist[3])weaponIndex = 3;                

                float height = _mainCamera.orthographicSize;
                float width = _mainCamera.orthographicSize * _mainCamera.aspect;
                Vector3 InsideCameraBondsPosition = new Vector3(_mainCamera.transform.position.x + UnityEngine.Random.Range(-width, width), _mainCamera.transform.position.y + UnityEngine.Random.Range(-height, height), 0);
                if (_weaponIsExist[weaponIndex] != true)
                {

                    Instantiate(Weapon[weaponIndex], InsideCameraBondsPosition, Quaternion.identity, zoneAndBonusSpawnHolder);
                    _weaponIsExist[weaponIndex] = true;
                }
                
            }
            yield return new WaitForSeconds(weaponSpawnTime);
        }
    }
    IEnumerator SpawnBonus()
    {
        while (true)
        {
            float height = _mainCamera.orthographicSize;
            float width = _mainCamera.orthographicSize * _mainCamera.aspect;
            Vector3 InsideCameraBondsPosition = new Vector3(_mainCamera.transform.position.x + UnityEngine.Random.Range(-width, width), _mainCamera.transform.position.y + UnityEngine.Random.Range(-height, height), 0);
            Random random = new();
            Instantiate(Bonus[random.NextDouble() < 0.5 ? 0 : 1], InsideCameraBondsPosition, Quaternion.identity, zoneAndBonusSpawnHolder);

            yield return new WaitForSeconds(bonusSpawnTime);
        }
    }
    

    private void SpawnZone(int _zoneIndex)
    {
        GameObject map = GameObject.FindWithTag("Map");
        float height = map.GetComponent<RectTransform>().rect.height / 2f - ZoneInterval;
        float width = map.GetComponent<RectTransform>().rect.width / 2f - ZoneInterval;

        float x = UnityEngine.Random.Range(map.transform.position.x - UnityEngine.Random.Range(0, width), map.transform.position.x + UnityEngine.Random.Range(0, width));
        float y = UnityEngine.Random.Range(map.transform.position.y - UnityEngine.Random.Range(0, height), map.transform.position.y + UnityEngine.Random.Range(0, height));
        Vector2 SpawnPos = new(x, y);

        
        bool check = CheckSpawnPos(SpawnPos, Zone[_zoneIndex].GetComponent<RectTransform>().rect.height / 2f);
        if (check)
        {
            if (_zoneIndex == 0 ? (dieZoneSpawned < dieZoneCount)  : (lowSpeedZoneSpawned < lowSpeedZoneCount))
            {
                Instantiate(Zone[_zoneIndex], SpawnPos, Quaternion.identity, zoneAndBonusSpawnHolder);
                if (_zoneIndex == 0)
                    dieZoneSpawned++;
                else
                    lowSpeedZoneSpawned++;
                SpawnZone(_zoneIndex);
            }
        }
        else
        {
            SpawnZone(_zoneIndex);
        }
    }
    private bool CheckSpawnPos(Vector3 SpawnPos, float ZoneRadius)
    {   
        Collider2D[] colliders;
        colliders = Physics2D.OverlapCircleAll(SpawnPos, ZoneInterval + ZoneRadius);
        if (colliders.Length > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public static void AddScore(int count)
    {
        _score += count;
        if (PlayerPrefs.HasKey("recordScore"))
        {
            if (PlayerPrefs.GetInt("recordScore") < _score)
                PlayerPrefs.SetInt("recordScore", _score);
        }
        else
            PlayerPrefs.SetInt("recordScore", _score);
        
    }    
    public void NextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void ScoreView()
    {        
        if (Int32.Parse(Score_text.text.Trim("—чЄт: ".ToCharArray())) < _score)
        Score_text.text = $"—чЄт: { _score}";
    }
    public void ResetRecord()
    {   
        PlayerPrefs.SetInt("recordScore",0);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        if (_score == PlayerPrefs.GetInt("recordScore") && _score != 0)
        Score_text.text = $"Ќовый рекорд: { _score}";
        Time.timeScale = 0;
        restartButton.gameObject.SetActive(true);
        StartCoroutine(GameOverAnimate());
       
        for(int i = 0; i < _weaponIsExist.Length; i++)
        {
            _weaponIsExist[i] = false;
        }
    }
    IEnumerator GameOverAnimate()
    {
        
        while (true)
        {
            menuButton.position = Vector3.MoveTowards(menuButton.position, menuButtonHolder.position, speedButtonAnimate);
            restartButton.position = Vector3.MoveTowards(restartButton.position, restartButtonHolder.position, speedButtonAnimate);
            scoreText.position = Vector3.MoveTowards(scoreText.position, scoreHolder.position, speedButtonAnimate);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}
