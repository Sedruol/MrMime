using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
    [SerializeField] private Text txtTitle;
    [SerializeField] private Text txtInfo;
    [SerializeField] private Button btnRightArrow;
    [SerializeField] private Button btnLeftArrow;
    private int cont;
    private bool change;
    // Start is called before the first frame update
    void Start()
    {
        cont = 0;
        change = false;
        btnLeftArrow.onClick.AddListener(() => MoveLeft());
        btnRightArrow.onClick.AddListener(() => MoveRight());
        btnLeftArrow.gameObject.SetActive(false);
        btnRightArrow.gameObject.SetActive(true);
        txtTitle.text = "Information Menu";
        txtInfo.text = "With the help of Mr Mime, you will be able to teach movements to robotic arm simulators in a simple way";
    }

    private void MoveLeft()
    {
        if (cont > 0)
        {
            cont--;
            change = true;
        }
    }

    private void MoveRight()
    {
        if (cont < 3)
        {
            cont++;
            change = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (change)
        {
            change = false;
            switch (cont)
            {
                case 0:
                    btnLeftArrow.gameObject.SetActive(false);
                    btnRightArrow.gameObject.SetActive(true);
                    txtTitle.text = "Information Menu";
                    txtInfo.text = "With the help of Mr Mime, you will be able to teach movements to robotic arm simulators in " +
                        "a simple way.";
                    break;
                case 1:
                    btnLeftArrow.gameObject.SetActive(true);
                    btnRightArrow.gameObject.SetActive(true);
                    txtTitle.text = "Add Movement";
                    txtInfo.text = "You must see the full body of a single person in the videos that you want to add and it is " +
                        "recommended that these last approximately 5 seconds. It could added in two ways:" +
                        "\n-Record Video: You can record a video with a web camera that is connected to the computer." +
                        "\n-Upload Video: You can upload a video from the computer.";
                    break;
                case 2:
                    btnLeftArrow.gameObject.SetActive(true);
                    btnRightArrow.gameObject.SetActive(true);
                    txtTitle.text = "Try Simulator";
                    txtInfo.text = "In this menu, you can freely move the robotic arm simulators as follows:"+
                        "\n-Right Arm: You can use the left and right arrow keys to change the selected joint of the simulator. " +
                        "Also, you can use the up and down arrow keys to rotate the selected joint of the simulator. The " +
                        "selected joint will be painted a reddish color."+
                        "\n-Left Arm: You can use the 'A' and 'D' keys to change the selected joint of the simulator. Also, you " +
                        "can use the 'W' and 'S' keys to rotate the selected joint of the simulator. The selected joint will " +
                        "be painted a bluedish color.";
                    break;
                case 3:
                    btnLeftArrow.gameObject.SetActive(true);
                    btnRightArrow.gameObject.SetActive(false);
                    txtTitle.text = "Simulate Movement";
                    txtInfo.text = "In this menu, you can choose any of the stored movements to be reproduced by the robotic " +
                        "arm simulators.";
                    break;
            }
        }
    }
}
