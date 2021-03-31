using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button btnAddMovement;
    [SerializeField] private Button btnSelectMovement;
    // Start is called before the first frame update
    void Start()
    {
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
    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width * 0.41f, Screen.height * 0.5f, 150, 50), "Add Movement"))
        {
            SceneManager.LoadScene("Add Movement 1");
        }
        if (GUI.Button(new Rect(Screen.width * 0.41f, Screen.height * 0.75f, 150, 50), "Select Movement"))
        {
            SceneManager.LoadScene("Select Movement");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
