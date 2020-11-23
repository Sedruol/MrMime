using UnityEngine;
using UnityEngine.SceneManagement;//koala lo añadió
using System.Diagnostics;
using System;//koala añadió
using UnityEngine.UI;//koala añadió

namespace RockVR.Video.Demo
{
    public class VideoCaptureUI : MonoBehaviour
    {
        private bool isPlayVideo = false;
        [SerializeField] private S3Conection s3Conection;//koala añadió
        [SerializeField] private GameObject PanelFondo;//koala añadió
        [SerializeField] private Text txtInfo;//koala añadió
        [SerializeField] private Button btnOk;//koala añadió
        private string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);//koala añadió
        private void Awake()
        {
            Application.runInBackground = true;
            isPlayVideo = false;
            //koala añadió
            myDocumentsPath += "/RockVR/Video/";
            myDocumentsPath=myDocumentsPath.Replace(@"\", "/");
        }
        private void OnGUI()
        {
            if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.NOT_START)
            {
                if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Start Capture"))
                {
                    VideoCaptureCtrl.instance.StartCapture();
                }
            }
            else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STARTED)
            {
                if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Stop Capture"))
                {
                    VideoCaptureCtrl.instance.StopCapture();
                }
                if (GUI.Button(new Rect(180, Screen.height - 60, 150, 50), "Pause Capture"))
                {
                    VideoCaptureCtrl.instance.ToggleCapture();
                }
            }
            else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.PAUSED)
            {
                if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Stop Capture"))
                {
                    VideoCaptureCtrl.instance.StopCapture();
                }
                if (GUI.Button(new Rect(180, Screen.height - 60, 150, 50), "Continue Capture"))
                {
                    VideoCaptureCtrl.instance.ToggleCapture();
                }
            }
            else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.STOPPED)
            {
                if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Processing"))
                {
                    // Waiting processing end.
                }
            }
            else if (VideoCaptureCtrl.instance.status == VideoCaptureCtrl.StatusType.FINISH)
            {
                if (!isPlayVideo)
                {
                    if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "View Video"))
                    {
//#if UNITY_5_6_OR_NEWER
                        // Set root folder.
                        isPlayVideo = true;
                        VideoPlayer.instance.SetRootFolder();
                        // Play capture video.
                        VideoPlayer.instance.PlayVideo();
                    }
                }
                else
                {
                    ///////////////////////////
                    //-------original-------//
                    /////////////////////////
                    //if(GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Next Video"))
                    //{
                        // Turn to next video.
                    //    VideoPlayer.instance.NextVideo();
                        // Play capture video.
                    //    VideoPlayer.instance.PlayVideo();
//#else
                        // Open video save directory.
//                        Process.Start(PathConfig.saveFolder);
//#endif
                    //}
                    //agregado por koala
                    if (GUI.Button(new Rect(/*465*/Screen.width - 175, Screen.height - 120, 150, 50), "Back"))
                    {
                        VideoCaptureCtrl.instance.ChangeStatus();
                        gameObject.SetActive(false);
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                    if (GUI.Button(new Rect(/*465*/Screen.width - 175, Screen.height - 60, 150, 50), "Add Video"))
                    {
                        PanelFondo.gameObject.SetActive(true);
                        txtInfo.text = "Se está enviando el video al servidor";
                        myDocumentsPath += VideoPlayer.instance.VideoName;
                        print(myDocumentsPath);
                        s3Conection.Post(myDocumentsPath, VideoPlayer.instance.VideoName);
                        txtInfo.text = "Se envió el video al servidor";
                        btnOk.gameObject.SetActive(true);
                        VideoCaptureCtrl.instance.ChangeStatus();
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}