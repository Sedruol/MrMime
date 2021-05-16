using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button btnGoHome;
    [SerializeField] private Button btnExit;
    [SerializeField] private GameObject pnlExit;
    [SerializeField] private Button btnLeave;
    [SerializeField] private Button btnStay;
    // Start is called before the first frame update
    void Start()
    {
        btnGoHome.onClick.AddListener(() => GoHome());
        btnExit.onClick.AddListener(() => PanelExit());
        btnLeave.onClick.AddListener(() => Exit());
        btnStay.onClick.AddListener(() => PanelExit());
    }

    private void GoHome()
    {
        SceneManager.LoadScene("Main Menu");
    }
    private void PanelExit()
    {
        Globals.vExit = !Globals.vExit;
        pnlExit.SetActive(Globals.vExit);
    }
    private void Exit()
    {
        Debug.Log("la aplicación se cierra");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
