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
        btnAddMovement.onClick.AddListener(() => GoAddMovement());
        btnSelectMovement.onClick.AddListener(() => GoSelectMovement());
    }
    public void GoAddMovement()
    {
        SceneManager.LoadScene("Add Movement");
    }
    public void GoSelectMovement()
    {
        SceneManager.LoadScene("Select Movement");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
