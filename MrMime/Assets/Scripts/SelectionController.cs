using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private GameObject movement;
    [SerializeField] private Button btnRightArrow;
    [SerializeField] private Button btnLeftArrow;
    [SerializeField] private Button btnMovement1;
    [SerializeField] private Button btnMovement2;
    [SerializeField] private Button btnMovement3;
    [SerializeField] private GameObject CanvasMovements;
    [SerializeField] private GameObject esqueleto;
    [SerializeField] private GameObject circle;
    private int cont;
    private bool change = false;
    private bool movement1;
    private bool movement2;
    private bool movement3;
    private float[] xs = new float[100];
    private float[] ys = new float[100];
    private float[] zs = new float[100];
    private bool muevete;
    private int cantMoves;
    private int temp;
    private string path;
    private string[] lines;
    private int contMove;

    private void Awake()
    {
        CanvasMovements.SetActive(true);
        contMove = 0;
        cont = 0;
        cantMoves = 0;
        temp = 0;
        muevete = false;
        if (Globals.movements.Count == 0)
        {
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
        }
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
        /*for(int i = 0; i < 17; i++)
        {
            GameObject g = Instantiate(circle, new Vector3(-50, -10, -20), Quaternion.identity, 
                esqueleto.transform);
        }*/
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
        if (!movement3)
        {
            movement1 = false;
            movement2 = false;
            movement3 = true;
        }
        else
            movement3 = false;
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
            CanvasMovements.SetActive(false);
            path = "C:/Users/m/Downloads/output.txt";

            //StreamReader reader = new StreamReader(path);
            //Debug.Log(reader.ReadToEnd()+"10");
            //reader.Close();
            lines = File.ReadAllLines(path);
            cantMoves = 0;
            for (int a = 0; a < lines.Length; a++)
            {
                if (lines[a] == ".")
                {
                    cantMoves++;
                }
            }
            temp = lines.Length / cantMoves;
            Debug.Log(cantMoves);
            muevete = true;
            /*for (int i = 0; i < cantMoves; i++)
            {
                for (int j = 0; j < temp; j++)
                {
                    if (lines[j] != ".")
                    {
                        xs[j] = float.Parse(lines[temp * i + j].Split(' ')[0]);
                        ys[j] = float.Parse(lines[temp * i + j].Split(' ')[1]);
                        zs[j] = float.Parse(lines[temp * i + j].Split(' ')[2]);
                        graphic(temp - 1);
                        //cantPoints++;
                        //Debug.Log("x:" + xs[j] + " y:" + ys[j] + " z:" + zs[j]);
                    }
                }
            }*/
        }
        else if (movement2)
        {
            CanvasMovements.SetActive(false);
            path = "C:/Users/m/Downloads/output.txt";

            lines = File.ReadAllLines(path);
            cantMoves = 0;
            for (int a = 0; a < lines.Length; a++)
            {
                if (lines[a] == ".")
                {
                    cantMoves++;
                }
            }
            temp = lines.Length / cantMoves;
            Debug.Log(cantMoves);
            muevete = true;
        }
        else if (movement3)
        {
            CanvasMovements.SetActive(false);
            path = "C:/Users/m/Downloads/output.txt";

            lines = File.ReadAllLines(path);
            cantMoves = 0;
            for (int a = 0; a < lines.Length; a++)
            {
                if (lines[a] == ".")
                {
                    cantMoves++;
                }
            }
            temp = lines.Length / cantMoves;
            Debug.Log(cantMoves);
            muevete = true;
        }
    }
    public void graphic(int cantPoints)
    {
        for (int j = 0; j < cantPoints; j++)
        {
            esqueleto.transform.GetChild(j).transform.position = new Vector3(xs[j] / 100, ys[j] / 100, (zs[j] / 100) + 10);
        }
        muevete = true;
    }
    private void FixedUpdate()
    {
        //---usar-verano---
        /*if (muevete)
        {
            if (contMove == cantMoves)
            {
                muevete = false;
                contMove = 0;
            }
            else
            {
                muevete = false;
                for (int j = 0; j < temp; j++)
                {
                    if (lines[j] != ".")
                    {
                        xs[j] = float.Parse(lines[temp * contMove + j].Split(' ')[0]);
                        ys[j] = float.Parse(lines[temp * contMove + j].Split(' ')[1]);
                        zs[j] = float.Parse(lines[temp * contMove + j].Split(' ')[2]);
                    }
                }
                contMove++;
                graphic(temp - 1);
            }
        }*/
        //---fin---
        //no usar lo de abajo
        /*if (muevete && cantMoves > 0)
        {
            for (int j = 0; j < temp - 1; j++)
            {
                if (xs[j].ToString() != "")
                    esqueleto.transform.GetChild(j).transform.position = 
                        new Vector3(xs[j] / 100, ys[j] / 100, (zs[j] / 100) + 10);
            }
            cantMoves--;
        }
        else
            muevete = false;*/
    }

    public void MoveMovementsRight()
    {
        if (cont < Globals.movements.Count - 3)
        {
            SetChange(true);
            SetCont(cont + 1);
        }
    }
    //usar en verano
    /*private void OnGUI()
    {
        if (CanvasMovements.activeSelf)
        {
            if (GUI.Button(new Rect(Screen.width * 0.37f, Screen.height - 60, 150, 50), "Execute Movement"))
            {
                ReadTxt();
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width * 0.37f, Screen.height - 60, 150, 50), "Back"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }*/
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
