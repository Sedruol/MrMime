using RosSharp.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    SimulationManager instance;
    public string content;
    public string title;
    public Text titleText;
    public GameObject finish;
    public Controller rightArm;
    public Controller leftArm;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        content = PlayerPrefs.GetString("sim");
        title = PlayerPrefs.GetString("title");
        titleText.text = title;
        string[] arrLeftRight = content.Split(char.Parse("a"));
        //right arm
        print(arrLeftRight[0]);
        string[] arrRight = arrLeftRight[0].Split(char.Parse("\n"));
        
        Vector3[] rightPoints = new Vector3[arrRight.Length-1];
        for(int i = 0; i < arrRight.Length - 1; i++)
        {
            string[] numbers = arrRight[i].Split(char.Parse(","));
            
            rightPoints[i] = new Vector3(float.Parse(numbers[0]), float.Parse(numbers[1]), float.Parse(numbers[2]));
            print(rightPoints[i]);
        }
        print("============================================");
        //left arm
        string[] arrLeft = arrLeftRight[1].Split(char.Parse("\n"));
        Vector3[] leftPoints = new Vector3[arrLeft.Length-1];
        for (int i = 0; i < arrLeft.Length-1; i++)
        {
            string[] numbers = arrLeft[i+1].Split(char.Parse(","));
            leftPoints[i] = new Vector3(float.Parse(numbers[0]), float.Parse(numbers[1]), float.Parse(numbers[2]));
            print(leftPoints[i]);
        }
        print(leftPoints[0]);
        print(leftPoints[leftPoints.Length-1]);
        rightArm.points = rightPoints;
        leftArm.points = leftPoints;
        //rightArm.pointString = arrLeftRight[0];

        //leftArm.pointString = arrLeftRight[1];


    }

    public void BackScene()
    {
        SceneManager.LoadScene("Select Robot Movement");
    }
    private void Update()
    {
        if (rightArm.f)
        {
            finish.SetActive(true);
        }
    }

}
