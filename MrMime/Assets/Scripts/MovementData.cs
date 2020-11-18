using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementData : MonoBehaviour
{
    public string movementName;
    public Sprite movementPicture;

    public string getMovementName() { return movementName; }
    public Sprite getMovementPicture() { return movementPicture; }
    public void setMovementName(string name) { movementName = name; }
    public void setMovementPicture(Sprite sprite) { movementPicture = sprite; }
}
