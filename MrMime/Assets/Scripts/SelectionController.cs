using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private GameObject movement;
    [SerializeField] private Button btnRightArrow;
    [SerializeField] private Button btnLeftArrow;
    [SerializeField] private Button btnMovement1;
    [SerializeField] private Button btnMovement2;
    [SerializeField] private Button btnMovement3;
    private int cont;
    private bool change = false;
    private bool movement1;
    private bool movement2;
    private bool movement3;

    private void Awake()
    {
        cont = 0;
        Globals.movements.Add(new MovementData()
        {
            movementName = "caminar",
            movementPicture = Resources.Load<Sprite>("Sprites/caminar")
        });
        Globals.movements.Add(new MovementData()
        {
            movementName = "correr",
            movementPicture = Resources.Load<Sprite>("Sprites/correr")
        });
        Globals.movements.Add(new MovementData()
        {
            movementName = "estirar",
            movementPicture = Resources.Load<Sprite>("Sprites/estirar")
        });
        Globals.movements.Add(new MovementData()
        {
            movementName = "marcha",
            movementPicture = Resources.Load<Sprite>("Sprites/marcha")
        });
        Globals.movements.Add(new MovementData()
        {
            movementName = "saludar",
            movementPicture = Resources.Load<Sprite>("Sprites/saludar")
        });
        movement1 = false;
        movement2 = false;
        movement3 = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Globals.movements.Count);
        //Globals.movements.Add();
        btnLeftArrow.onClick.AddListener(() => MoveMovementsLeft());
        btnRightArrow.onClick.AddListener(() => MoveMovementsRight());
        btnMovement1.onClick.AddListener(() => ExecuteMovement1());
        btnMovement2.onClick.AddListener(() => ExecuteMovement2());
        btnMovement3.onClick.AddListener(() => ExecuteMovement3());
    }
    public void ExecuteMovement1() {
        if (!movement1)
        {
            movement1 = true;
            movement2 = false;
            movement3 = false;
        }
        else
            movement1 = false;
        Debug.Log(movement1);
    }
    public void ExecuteMovement2()
    {
        if (!movement2)
        {
            movement1 = false;
            movement2 = true;
            movement3 = false;
        }
        else
            movement2 = false;
    }
    public void ExecuteMovement3()
    {
        if (!movement2)
        {
            movement1 = false;
            movement2 = false;
            movement3 = true;
        }
        else
            movement2 = false;
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

    public void ReadTxt()
    {
        if (movement1)
        {
            string path = "C:/Users/m/Documents/holi.txt";

            StreamReader reader = new StreamReader(path);
            Debug.Log(reader.ReadToEnd()+"10");
            reader.Close();
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
    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width/2, Screen.height - 60, 150, 50), "Execute Movement"))
        {
            ReadTxt();
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
