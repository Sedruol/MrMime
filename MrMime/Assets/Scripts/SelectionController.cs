using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private GameObject movement;
    [SerializeField] private Button btnRightArrow;
    [SerializeField] private Button btnLeftArrow;
    private int cont;
    private bool change = false;

    private void Awake()
    {
        cont = 0;
        Globals.movements.Add(new MovementData()
        {
            movementName = "koala",
            movementPicture = Resources.Load<Sprite>("Sprites/menu")
        });
        Globals.movements.Add(new MovementData()
        {
            movementName = "toro",
            movementPicture = Resources.Load<Sprite>("Sprites/cerrar")
        });
        Globals.movements.Add(new MovementData()
        {
            movementName = "leon",
            movementPicture = Resources.Load<Sprite>("Sprites/menu")
        });
        Globals.movements.Add(new MovementData()
        {
            movementName = "cocodrilo",
            movementPicture = Resources.Load<Sprite>("Sprites/cerrar")
        });
        Globals.movements.Add(new MovementData()
        {
            movementName = "serpiente",
            movementPicture = Resources.Load<Sprite>("Sprites/menu")
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Globals.movements.Count);
        //Globals.movements.Add();
        btnLeftArrow.onClick.AddListener(() => MoveMovementsLeft());
        btnRightArrow.onClick.AddListener(() => MoveMovementsRight());
    }
    public bool GetChange() { return change; }
    public int getCont() { return cont; }
    public void SetChange(bool _change) { change = _change; }
    public void SetCont(int _cont) { cont = _cont; }
    public void MoveMovementsLeft()
    {
        if (cont > 0)
        {
            SetChange(true);
            SetCont(cont - 1);
        }
    }
    public void MoveMovementsRight()
    {
        if (cont < Globals.movements.Count - 3)
        {
            SetChange(true);
            SetCont(cont + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cont > 0)
            btnLeftArrow.gameObject.SetActive(true);
        else
            btnLeftArrow.gameObject.SetActive(false);
        if (cont < Globals.movements.Count - 3)
            btnRightArrow.gameObject.SetActive(true);
        else
            btnRightArrow.gameObject.SetActive(false);
    }
}
