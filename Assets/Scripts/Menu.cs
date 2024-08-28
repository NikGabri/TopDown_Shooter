using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public Transform Triangle;
    public float RotateSpeed = 0.1f;
    public TMP_Text scoreRecord;

    private void Start()
    {
        if (PlayerPrefs.HasKey("recordScore")) { }
        scoreRecord.text = $"Рекорд: {PlayerPrefs.GetInt("recordScore")}";
    }
    private void Update()
    {
        RotateTriangle();
    }

    private void RotateTriangle()
    {
        Triangle.Rotate(0, 0, RotateSpeed);
    }
    public void NextScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
