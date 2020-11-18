using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementDisplay : MonoBehaviour
{
    public Text nameMovement1;
    public Text nameMovement2;
    public Text nameMovement3;
    public Image imageMovement1;
    public Image imageMovement2;
    public Image imageMovement3;
    private MovementData movementData;
    public SelectionController selectionController;

    void Start()
    {
        movementData = GetComponent<MovementData>();
        selectionController = selectionController.GetComponent<SelectionController>();
        DisplayMovements();
    }
    
    public void DisplayMovements()
    {
        movementData = Globals.movements[selectionController.getCont()];
        nameMovement1.text = movementData.getMovementName();
        imageMovement1.sprite = movementData.getMovementPicture();
        movementData = Globals.movements[selectionController.getCont() + 1];
        nameMovement2.text = movementData.getMovementName();
        imageMovement2.sprite = movementData.getMovementPicture();
        movementData = Globals.movements[selectionController.getCont() + 2];
        nameMovement3.text = movementData.getMovementName();
        imageMovement3.sprite = movementData.getMovementPicture();

        /*if (gameObject.name == "Movement 1")
        {
            movementData = Globals.movements[selectionController.getCont()];
            //movementData.setMovementName(Globals.movements[0].getMovementName());
            //movementData.setMovementPicture(Globals.movements[0].getMovementPicture());
        }
        else if (gameObject.name == "Movement 2")
            movementData = Globals.movements[selectionController.getCont() + 1];
        else if (gameObject.name == "Movement 3")
            movementData = Globals.movements[selectionController.getCont() + 2];
        nameMovement1.text = movementData.getMovementName();
        imageMovement1.sprite = movementData.getMovementPicture();*/
    }

    // Update is called once per frame
    void Update()
    {
        if (selectionController.GetChange() == true)
        {
            selectionController.SetChange(false);
            DisplayMovements();
        }
    }
}
