using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using SmartDLL;

public class RecordVideo : MonoBehaviour
{
    [SerializeField] private Button btnGoHome;
    [SerializeField] private Button btnExit;
    [SerializeField] private GameObject pnlExit;
    [SerializeField] private Button btnLeave;
    [SerializeField] private Button btnStay;
    [SerializeField] private GameObject VideoCaptureCtlr;
    [SerializeField] private RawImage camaraVideo;
    [SerializeField] private GameObject panelVideo;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private S3Conection s3Conection;
    [SerializeField] private GameObject PanelFondo;
    [SerializeField] private Text txtInfo;
    [SerializeField] private Button btnOk;
    static WebCamTexture backCam;
    private bool camara;
    private string path;
    private string videoName;
    private string targetPath;
    private string desFile;
    private bool reproduce;
    private VideoPlayer video;
    private string search;
    public SmartFileExplorer fileExplorer = new SmartFileExplorer();
    void Start()
    {
        btnGoHome.onClick.AddListener(() => GoHome());
        btnExit.onClick.AddListener(() => PanelExit());
        btnLeave.onClick.AddListener(() => Exit());
        btnStay.onClick.AddListener(() => PanelExit());
        btnOk.gameObject.SetActive(false);
        PanelFondo.SetActive(false);
        txtInfo.text = "";
        panelVideo.SetActive(false);
        camaraVideo.gameObject.SetActive(false);
        VideoCaptureCtlr.SetActive(false);
        //btnRecordVideo.onClick.AddListener(() => OpenCamera());
        camara = false;
        targetPath = System.IO.Directory.GetCurrentDirectory();
        targetPath = targetPath.Replace(@"\", "/");
        reproduce = false;
        videoPlayer.gameObject.SetActive(false);
        video = videoPlayer.GetComponent<VideoPlayer>();
        btnOk.onClick.AddListener(() => ClosePanel());
    }
    private void ClosePanel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void GoHome()
    {
        StopVideo();
        SceneManager.LoadScene("Main Menu");
    }

    private void PanelExit()
    {
        if (!VideoCaptureCtlr.activeSelf)
        {
            Globals.vExit = !Globals.vExit;
            PauseVideo();
            pnlExit.SetActive(Globals.vExit);
        }
    }

    private void Exit()
    {
        StopVideo();
        Application.Quit();
    }

    public void OpenCamera()
    {
        VideoCaptureCtlr.SetActive(true);
        camara = true;
        if (backCam == null)
            backCam = new WebCamTexture();
        camaraVideo.texture = backCam;
        backCam.Play();
        camaraVideo.gameObject.SetActive(true);
    }

    public void ShowExplorer()
    {
        string initialDir = @"C:\";
        bool restoreDir = true;
        string title = "Overwrite with mp4";
        string defExt = "mp4";
        string filter = "mp4 files (*.mp4)|*.mp4";

        fileExplorer.OpenExplorer(initialDir, restoreDir, title, defExt, filter);
        path = fileExplorer.fileName;
        path = path.Replace(@"\", "/");
        reproduce = true;
        Debug.Log(path);
    }

    public void OpenExplorer()
    {
        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
#if UNITY_EDITOR
        path = EditorUtility.OpenFilePanel("Overwrite with mp4", "", "mp4");
        if (path != "")
        {
            path = path.Replace(@"\", "/");
            reproduce = true;
            Debug.Log(path);
        }
        //GetVideoName();
#else
        ShowExplorer();
#endif
        if (videoPlayer.isPaused)
            videoPlayer.Play();
    }
    /*public void GetVideoName()
    {
        videoName = System.IO.Path.GetFileName(path);
        desFile = System.IO.Path.Combine(targetPath, "Assets/Resources/Video/", videoName);
        desFile = desFile.Replace(@"\", "/");
        Debug.Log(videoName);
        Debug.Log(targetPath);
        Debug.Log(desFile);
        if (!System.IO.File.Exists(desFile))
            System.IO.File.Copy(path, desFile);
        reproduce = true;
        search = "Video/" + videoName;
        search = search.Split('.')[0];
    }*/
    public void AddVideo()
    {
        videoPlayer.gameObject.SetActive(false);
        panelVideo.SetActive(false);
        //PanelFondo.gameObject.SetActive(true);
        //txtInfo.text = "Se está enviando el video al servidor";
        string[] fileNames = path.Split('/');
        string nameFile = fileNames[fileNames.Length - 1];
        Debug.Log(nameFile);
        s3Conection.Post(path, nameFile);
        //txtInfo.text = "Se envió el video al servidor";
        //btnOk.gameObject.SetActive(true);
    }
    public void StopVideo()
    {
        if (backCam != null)
            backCam.Stop();
    }
    public void PauseVideo()
    {
        if (video.url != null)
        {
            if (Globals.vExit)
                video.Pause();
            else if (!Globals.vExit)
                video.Play();
        }
    }

    private void OnGUI()
    {
        if (!Globals.vExit)
        {
            if (!camara)
            {
                StopVideo();
                if (!panelVideo.activeSelf)
                {
                    if (GUI.Button(new Rect(25, Screen.height - 60, 150, 50), "Record Video"))
                    {
                        OpenCamera();
                    }
                    if (GUI.Button(new Rect(Screen.width - 175, Screen.height - 60, 150, 50), "Upload Video"))
                    {
                        OpenExplorer();
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(Screen.width - 525, Screen.height - 60, 150, 50), "Back"))
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                    if (GUI.Button(new Rect(Screen.width - 350, Screen.height - 60, 150, 50), "Add Video"))
                    {
                        AddVideo();
                    }
                    if (GUI.Button(new Rect(Screen.width - 175, Screen.height - 60, 150, 50), "Upload Video"))
                    {
                        OpenExplorer();
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if (reproduce && System.IO.File.Exists(desFile))
        if(reproduce)
        {
            //video.clip = Resources.Load<VideoClip>(search);
            video.url = path;
            if (videoPlayer.url != null)//antes era video.clip
            {
                videoPlayer.gameObject.SetActive(true);
                panelVideo.gameObject.SetActive(true);
                reproduce = false;
            }
        }
    }
}
