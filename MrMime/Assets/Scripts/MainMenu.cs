using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button btnExit;
    [SerializeField] private GameObject pnlExit;
    [SerializeField] private Button btnLeave;
    [SerializeField] private Button btnStay;
    private bool vExit;
    //[SerializeField] private Button btnAddMovement;
    //[SerializeField] private Button btnSelectMovement;
    // Start is called before the first frame update
    void Start()
    {
        vExit = false;
        btnExit.onClick.AddListener(() => PanelExit());
        btnLeave.onClick.AddListener(() => Exit());
        btnStay.onClick.AddListener(() => PanelExit());
        pnlExit.SetActive(false);
        /*btnAddMovement.onClick.AddListener(() => GoAddMovement());
        btnSelectMovement.onClick.AddListener(() => GoSelectMovement());*/
    }
    /*public void GoAddMovement()
    {
        SceneManager.LoadScene("Add Movement 1");
    }
    public void GoSelectMovement()
    {
        SceneManager.LoadScene("Select Movement");
    }*/
    private void PanelExit()
    {
        vExit = !vExit;
        pnlExit.SetActive(vExit);
    }
    private void Exit()
    {
        Debug.Log("la aplicación se cierra");
        Application.Quit();
    }
    private void OnGUI()
    {
        if (!vExit)
        {
            if (GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.5f, Screen.width * 0.1f, Screen.height * 0.05f), "Add Movement"))
            {
                SceneManager.LoadScene("Add Movement 1");
            }
            if (GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.6f, Screen.width * 0.1f, Screen.height * 0.05f), "Select Movement"))
            {
                SceneManager.LoadScene("Select Movement");
            }
            if (GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.7f, Screen.width * 0.1f, Screen.height * 0.05f), "Try simulator"))
            {
                SceneManager.LoadScene("RobotArm");
            }
            if (GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.8f, Screen.width * 0.1f, Screen.height * 0.05f), "Simulate movement"))
            {
                SceneManager.LoadScene("Select Robot Movement");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
