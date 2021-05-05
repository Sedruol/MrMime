using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace RosSharp.Control
{
    public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };
    public enum ControlType { PositionControl };

    public class Controller : MonoBehaviour
    {
        private ArticulationBody[] articulationChain;
        // Stores original colors of the part being highlighted
        private Color[] prevColor;
        private int previousIndex;
        public Controller SM;
        [InspectorReadOnly(hideInEditMode: true)]
        public int timeDelay = 0;
        public string selectedJoint;
        public Vector3[] points;
        public bool type = false;
        public int it = 0;
        public int verifyCount = 0;
        public bool canChange = true;
        //
        public bool directionTypeSA = false;
        public bool directionTypeSE = false;
        public bool directionTypeEH = false;
        //
        public bool blockSA = false;
        public bool blockSE = false;
        public bool blockEH = false;
        Vector3 actualPoint = new Vector3(0f,0f,-1f);
        [HideInInspector]
        public int selectedIndex;
        public bool left;
        public ControlType control = ControlType.PositionControl;
        public float stiffness;
        public float damping;
        public float forceLimit;
        public float speed = 5f; // Units: degree/s
        public float torque = 100f; // Units: Nm or N
        public float acceleration = 5f;// Units: m/s^2 / degree/s^2

        [Tooltip("Color to highlight the currently selected join")]
        public Color highLightColor = new Color(255, 0, 0, 255);

        void Start()
        {
            
            previousIndex = selectedIndex = 1;
            this.gameObject.AddComponent<FKRobot>();
            articulationChain = this.GetComponentsInChildren<ArticulationBody>();
            int defDyanmicVal = 10;
            foreach (ArticulationBody joint in articulationChain)
            {
                joint.gameObject.AddComponent<JointControl>();
                joint.jointFriction = defDyanmicVal;
                joint.angularDamping = defDyanmicVal;
                ArticulationDrive currentDrive = joint.xDrive;
                currentDrive.forceLimit = forceLimit;
                joint.xDrive = currentDrive;
            }
            DisplaySelectedJoint(selectedIndex);
            StoreJointColors(selectedIndex);
        }

        void FixedUpdate()
        {
            bool SelectionInput1;
            bool SelectionInput2;
            if (left)
            {
                SelectionInput1 = Input.GetKeyDown("a");
                SelectionInput2 = Input.GetKeyDown("d");
            }
            else
            {
                SelectionInput1 = Input.GetKeyDown("right");
                SelectionInput2 = Input.GetKeyDown("left");
            }

            if (SelectionInput2)
            {
                if (selectedIndex == 1)
                {
                    selectedIndex = articulationChain.Length - 1;
                }
                else
                {
                    selectedIndex = selectedIndex - 1;
                }
                Highlight(selectedIndex);
            }
            else if (SelectionInput1)
            {
                if (selectedIndex == articulationChain.Length - 1)
                {
                    selectedIndex = 1;
                }
                else
                {
                    selectedIndex = selectedIndex + 1;
                }
                Highlight(selectedIndex);
            }


            if(type)SimulatePoints();
            else UpdateDirection(selectedIndex);
        }

        private void SimulatePoints()
        {
            print(it + "vs" + points.Length);
            timeDelay++;
            if (canChange)
            {
                SetDirections();
            }
            else
            {
                VerifyMovements();
            }
            if (verifyCount >= 6 && timeDelay >= 10 && it+1<= points.Length-1)
            {
                timeDelay = 0;
                verifyCount = 0;
                it++;
                print("cambio a " + it.ToString());
                canChange = true;
                blockSA = false;
                blockSE = false;
                blockEH = false;
            }
        }
        private void SetDirections()
        {
            //Seteo las direcciones de los 3 rotores
            ArticulationBody actualJoint;
            Vector3 newPoint = points[it];
            double angle = 0;
            //primer rotor
            //Obtengo los valores necesarios para comparar

            actualJoint = articulationChain[1];
            angle = AngleBetweenPoints(actualPoint, new Vector3(0f, newPoint.y, newPoint.z));
            //if (actualPoint.z > newPoint.z) angle *= -1;
            //print(actualJoint.xDrive.target + " vs " + angle);
            if (actualJoint.xDrive.target <= angle)
            {
               
                directionTypeSA = true;
            }
            else
            {
                directionTypeSA = false;

            }
            if (double.IsNaN(angle))
            {
                FinishMove();
                print("block SA");
                blockSA = true;
            }
            
            
            //segundo rotor
            //Obtengo los valores necesarios para comparar

            actualJoint = articulationChain[2];
            angle = AngleShoulderElbow(newPoint);
            print(actualJoint.xDrive.target + "vs" + angle);
            if (actualJoint.xDrive.target  <= angle)
            {
                directionTypeSE = true;
            }
            else
            {
                directionTypeSE = false;

            }

            //tercer rotor
            //Obtengo los valores necesarios para comparar

            actualJoint = articulationChain[3];
            angle = AngleElbowHand(newPoint);

            if (actualJoint.xDrive.target <= angle)
            {
                directionTypeEH = true;
            }
            else
            {
                directionTypeEH = false;

            }
            canChange = false;
        }
        private void VerifyMovements()
        {
            ArticulationBody actualJoint;
            Vector3 newPoint = points[it];
            double angle = 0;
            JointControl current;
            //Se mueven los rotores hasta que lleguen al punto
            //primer rotor
            actualJoint = articulationChain[1];
            current = articulationChain[1].GetComponent<JointControl>();
            angle = AngleBetweenPoints(actualPoint, new Vector3(0f,newPoint.y,newPoint.z));
            
            //if (actualPoint.z > newPoint.z) angle *= -1;
            //print(angle +" vs "+ actualJoint.xDrive.target );
            if (!blockSA)
            {
                if (directionTypeSA)
                {
                    current.direction = RotationDirection.Positive;
                    if (actualJoint.xDrive.target >= angle || actualJoint.xDrive.target >= 174)
                    {
                        
                        current.direction = RotationDirection.None;
                        FinishMove();
                        print("block SA");
                        blockSA = true;
                        //actualPoint = newPoint;
                        //print(actualPoint);
                    }
                }
                else
                {
                    current.direction = RotationDirection.Negative;
                    if (actualJoint.xDrive.target <= angle || actualJoint.xDrive.target <= -173)
                    {
                        current.direction = RotationDirection.None;
                        FinishMove();
                        print("block SA");
                        blockSA = true;
                    }
                }
            }
            //segundo rotor
            actualJoint = articulationChain[2];
            angle = AngleShoulderElbow(newPoint);
            current = articulationChain[2].GetComponent<JointControl>();
            if (!blockSE)
            {
                if (directionTypeSE)
                {
                    current.direction = RotationDirection.Positive;
                    if (actualJoint.xDrive.target >= angle || actualJoint.xDrive.target >=34)
                    {
                        current.direction = RotationDirection.None;
                        FinishMove();
                        print("block SE");
                        blockSE = true;
                    }
                }
                else
                {
                    current.direction = RotationDirection.Negative;
                    if (actualJoint.xDrive.target <= angle || actualJoint.xDrive.target <= -107)
                    {
                        current.direction = RotationDirection.None;
                        FinishMove();
                        print("block SE");
                        blockSE = true;
                    }
                }
            }
            //tercer rotor
            actualJoint = articulationChain[3];
            current = articulationChain[3].GetComponent<JointControl>();
            angle = AngleElbowHand(newPoint);
            if (!blockEH)
            {
                if (directionTypeEH)
                {
                    current.direction = RotationDirection.Positive;
                    if (actualJoint.xDrive.target >= angle || actualJoint.xDrive.target >= 88)
                    {
                        current.direction = RotationDirection.None;
                        FinishMove();
                        blockEH = true;
                        print("block EH");
                    }
                }
                else
                {
                    current.direction = RotationDirection.Negative;
                    if (actualJoint.xDrive.target <= angle || actualJoint.xDrive.target <= -78)
                    {
                        current.direction = RotationDirection.None;
                        FinishMove();
                        blockEH = true;
                        print("block EH");
                    }
                }
            }
        }

        private void FinishMove()
        {
            verifyCount++;
            SM.verifyCount++;
        }
        private Vector3 UnitaryVector(Vector3 p)
        {
            double mag = Math.Sqrt(Math.Pow(p.x, 2) + Math.Pow(p.y, 2) + Math.Pow(p.z, 2));
            p.x = p.x / (float)mag;
            p.y = p.y / (float)mag;
            p.z = p.z / (float)mag;
            return p;
        }
        private double AngleBetweenPoints(Vector3 p1, Vector3 p2)
        {
            p1 = UnitaryVector(p1);
            p2 = UnitaryVector(p2);
            double num = (p1.x * p2.x) + (p1.y * p2.y) + (p1.z * p2.z);

            double dem = Math.Sqrt(Math.Pow(p1.x, 2) + Math.Pow(p1.y, 2) + Math.Pow(p1.z, 2)) * Math.Sqrt(Math.Pow(p2.x, 2) + Math.Pow(p2.y, 2) + Math.Pow(p2.z, 2));
            double res = Math.Acos((num) / (dem));
            return res*180f/Math.PI;
        }
        private double AngleShoulderElbow(Vector3 p)
        {
            double distance = DistanceBetweenPoints(new Vector3(0f,0f,0f), p);
            //print("distance "+distance);
            double angleSE = Math.Asin(distance/2)*180f/Math.PI;
            //print("angulo calculado "+angleSE);
            double angleChange = AngleBetweenPoints(new Vector3(-1f, 0f, 0f), p);
            //print("cambio en el angulo " + angleChange);
            double res = 90 - angleChange - angleSE;
            //print("resultado: " + res);
            return res;
        }
        private double AngleElbowHand(Vector3 p)
        {
            double distance = DistanceBetweenPoints(new Vector3(0f, 0f, 0f), p);
            double angleEH = Math.Asin(distance / 2) * 180f / Math.PI*2;
            return angleEH - 90;
        }
        private double DistanceBetweenPoints(Vector3 p1, Vector3 p2)
        {
            double res = Math.Sqrt(Math.Pow((p2.x - p1.x),2)+ Math.Pow((p2.y - p1.y), 2)+ Math.Pow((p2.z - p1.z), 2));
            return res;
        }


        /// <summary>
        /// Highlights the color of the robot by changing the color of the part to a color set by the user in the inspector window
        /// </summary>
        /// <param name="selectedIndex">Index of the link selected in the Articulation Chain</param>
        private void Highlight(int selectedIndex)
        {
            if (selectedIndex == previousIndex)
            {
                return;
            }

            // reset colors for the previously selected joint
            ResetJointColors(previousIndex);

            // store colors for the current selected joint
            StoreJointColors(selectedIndex);

            DisplaySelectedJoint(selectedIndex);
            Renderer[] rendererList = articulationChain[selectedIndex].transform.GetChild(0).GetComponentsInChildren<Renderer>();

            // set the color of the selected join meshes to the highlight color
            foreach (var mesh in rendererList)
            {
                if (IsHDR())
                {
                    mesh.material.SetColor("_BaseColor", highLightColor);
                }
                else
                {
                    mesh.material.color = highLightColor;
                }
            }
        }

        void DisplaySelectedJoint(int selectedIndex)
        {
            selectedJoint = articulationChain[selectedIndex].name + " (" + selectedIndex + ")";
        }
        

            


        
        /// <summary>
        /// Sets the direction of movement of the joint on every update
        /// </summary>
        /// <param name="jointIndex">Index of the link selected in the Articulation Chain</param>
        private void UpdateDirection(int jointIndex)
        {
            bool SelectionInput1;
            bool SelectionInput2;
            if (left)
            {
                SelectionInput1 = Input.GetKey("w");
                SelectionInput2 = Input.GetKey("s");
            }
            else
            {
                SelectionInput1 = Input.GetKey("up");
                SelectionInput2 = Input.GetKey("down");
            }

            ArticulationBody actualJoint = articulationChain[jointIndex];
            JointControl current = articulationChain[jointIndex].GetComponent<JointControl>();
            if (previousIndex != jointIndex)
            {
                JointControl previous = articulationChain[previousIndex].GetComponent<JointControl>();
                previous.direction = RotationDirection.None;
                previousIndex = jointIndex;
            }

            if (current.controltype != control)
                UpdateControlType(current);

            if (SelectionInput1 )
            {
                current.direction = RotationDirection.Positive;
            }
            else if (SelectionInput2)
            {
                current.direction = RotationDirection.Negative;
            }
            else
            {
                current.direction = RotationDirection.None;
            }
            print(actualJoint.xDrive.target);


        }

        /// <summary>
        /// Stores original color of the part being highlighted
        /// </summary>
        /// <param name="index">Index of the part in the Articulation chain</param>
        private void StoreJointColors(int index)
        {
            Renderer[] materialLists = articulationChain[index].transform.GetChild(0).GetComponentsInChildren<Renderer>();
            prevColor = new Color[materialLists.Length];
            for (int counter = 0; counter < materialLists.Length; counter++)
            {
                if (IsHDR())
                {
                    prevColor[counter] = materialLists[counter].material.GetColor("_BaseColor");
                }
                else
                {
                    prevColor[counter] = materialLists[counter].sharedMaterial.GetColor("_Color");
                }
            }
        }

        /// <summary>
        /// Resets original color of the part being highlighted
        /// </summary>
        /// <param name="index">Index of the part in the Articulation chain</param>
        private void ResetJointColors(int index)
        {
            Renderer[] previousRendererList = articulationChain[index].transform.GetChild(0).GetComponentsInChildren<Renderer>();
            for (int counter = 0; counter < previousRendererList.Length; counter++)
            {
                if (IsHDR())
                {
                    previousRendererList[counter].material.SetColor("_BaseColor", prevColor[counter]);
                }
                else
                {
                    previousRendererList[counter].material.color = prevColor[counter];
                }
            }
        }

        public void UpdateControlType(JointControl joint)
        {
            joint.controltype = control;
            if (control == ControlType.PositionControl)
            {
                ArticulationDrive drive = joint.joint.xDrive;
                drive.stiffness = stiffness;
                drive.damping = damping;
                joint.joint.xDrive = drive;
            }
        }

        /// Checks if current render pipeline is HDR 
        /// Used for setting the color of highlighted joint
        private bool IsHDR()
        {
            //TODO: should we also return true for Universal Render pipeline?
            return GraphicsSettings.renderPipelineAsset != null && GraphicsSettings.renderPipelineAsset.GetType().ToString().Contains("HighDefinition");
        }

        public void OnGUI()
        {
           
        }
    }
}
