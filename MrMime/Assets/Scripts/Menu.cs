using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button btnGoHome;
    [SerializeField] private Button btnExit;
    // Start is called before the first frame update
    void Start()
    {
        btnGoHome.onClick.AddListener(() => GoHome());
        btnExit.onClick.AddListener(() => Exit());
    }

    private void GoHome()
    {
        SceneManager.LoadScene("Main Menu");
    }
    private void Exit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
