using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gameSpeed = 1;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void OnValidate()
    {
        Time.timeScale = gameSpeed;
    }
}
