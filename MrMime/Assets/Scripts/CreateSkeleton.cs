using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSkeleton : MonoBehaviour
{
    [SerializeField] private GameObject joints;
    private LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ConectJoints()
    {
        switch (transform.name)
        {
            case "JointPelvisCaderaDer":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(0).transform.position);
                line.SetPosition(1, joints.transform.GetChild(1).transform.position);
                break;
            case "JointCaderaRodillaDer":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(1).transform.position);
                line.SetPosition(1, joints.transform.GetChild(2).transform.position);
                break;
            case "JointRodillaTobilloDer":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(2).transform.position);
                line.SetPosition(1, joints.transform.GetChild(3).transform.position);
                break;
            case "JointPelvisCaderaIzq":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(0).transform.position);
                line.SetPosition(1, joints.transform.GetChild(4).transform.position);
                break;
            case "JointCaderaRodillaIzq":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(4).transform.position);
                line.SetPosition(1, joints.transform.GetChild(5).transform.position);
                break;
            case "JointRodillaTobilloIzq":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(5).transform.position);
                line.SetPosition(1, joints.transform.GetChild(6).transform.position);
                break;
            case "JointCaderaEstomago":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(0).transform.position);
                line.SetPosition(1, joints.transform.GetChild(7).transform.position);
                break;
            case "JointEstomagoCuello":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(7).transform.position);
                line.SetPosition(1, joints.transform.GetChild(8).transform.position);
                break;
            case "JointCuelloCabeza":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(8).transform.position);
                line.SetPosition(1, joints.transform.GetChild(9).transform.position);
                break;
            case "JointCabezaFrente":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(9).transform.position);
                line.SetPosition(1, joints.transform.GetChild(10).transform.position);
                break;
            case "JointCuelloHombroIzq":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(8).transform.position);
                line.SetPosition(1, joints.transform.GetChild(11).transform.position);
                break;
            case "JointHombroCodoIzq":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(11).transform.position);
                line.SetPosition(1, joints.transform.GetChild(12).transform.position);
                break;
            case "JointCodoMuñecaIzq":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(12).transform.position);
                line.SetPosition(1, joints.transform.GetChild(13).transform.position);
                break;
            case "JointCuelloHombroDer":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(8).transform.position);
                line.SetPosition(1, joints.transform.GetChild(14).transform.position);
                break;
            case "JointHombroCodoDer":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(14).transform.position);
                line.SetPosition(1, joints.transform.GetChild(15).transform.position);
                break;
            case "JointCodoMuñecaDer":
                line = GetComponent<LineRenderer>();
                line.SetPosition(0, joints.transform.GetChild(15).transform.position);
                line.SetPosition(1, joints.transform.GetChild(16).transform.position);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(joints.transform.childCount > 0)
        {
            ConectJoints();
            /*line = transform.GetChild(0).GetComponent<LineRenderer>();
            line.SetPosition(0, joints.transform.GetChild(0).transform.position);
            line.SetPosition(1, joints.transform.GetChild(1).transform.position);*/
        }
    }
}
