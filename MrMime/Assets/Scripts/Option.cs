using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public string content;
    public void LoadSimulation()
    {
        
        PlayerPrefs.SetString("sim", content);
        SceneManager.LoadScene("RobotSimulation");
    }
}
