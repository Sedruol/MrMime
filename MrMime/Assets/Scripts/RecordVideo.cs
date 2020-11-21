using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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
    [SerializeField] private GameObject VideoCaptureCtlr;
    [SerializeField] private RawImage camaraVideo;
    [SerializeField] private GameObject panelVideo;
    [SerializeField] private VideoPlayer videoPlayer;
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
    private const string awsBucketName = "tesis-resources"; //pedirle al diogoz
    private const string awsAccessKey = "AKIAVKND3FIWXOADB5GX"; //pedirle al diogoz
    private const string awsSecretKey = "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"; //pedirle al diogoz
    private string awsURLBaseVirtual = "";
    void Start()
    {
        btnGoHome.onClick.AddListener(() => GoHome());
        btnExit.onClick.AddListener(() => Exit());
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
        awsURLBaseVirtual = "https://" + awsBucketName + ".s3.amazonaws.com/";
    }
    private void GoHome()
    {
        StopVideo();
        SceneManager.LoadScene("Main Menu");
    }
    private void Exit()
    {
        StopVideo();
        Application.Quit();
    }
    public void UploadFileToAWS3(string FileName, string FilePath) {
        string currentAWS3Date = System.DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss ") + "GMT";
        string canonicalString = "PUT\n\n\n\nx-amz-date:" + currentAWS3Date + "\n/" + awsBucketName + "/" + FileName;
        
        UTF8Encoding encode = new UTF8Encoding();
        HMACSHA1 signature = new HMACSHA1();
        signature.Key = encode.GetBytes(awsSecretKey);
        byte[] bytes = encode.GetBytes(canonicalString);
        byte[] moreBytes = signature.ComputeHash(bytes);
        string encodedCanonical = Convert.ToBase64String(moreBytes);

        string aws3Header = "AWS " + awsAccessKey + ":" + encodedCanonical;

        string URL3 = awsURLBaseVirtual + FileName;

        WebRequest requestS3 = (HttpWebRequest)WebRequest.Create(URL3); 
        requestS3.Headers.Add("Authorization", aws3Header);
        requestS3.Headers.Add("x-amz-date", currentAWS3Date);

        byte[] fileRawBytes = File.ReadAllBytes(FilePath);
        requestS3.ContentLength = fileRawBytes.Length;

        requestS3.Method = "PUT";

        Stream S3Stream = requestS3.GetRequestStream();
        S3Stream.Write(fileRawBytes, 0, fileRawBytes.Length);
        Debug.Log("Sent bytes: " + requestS3.ContentLength + ", for file: " + FileName);

        S3Stream.Close();
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
    }

    public void OpenExplorer()
    {
#if UNITY_EDITOR
        path = EditorUtility.OpenFilePanel("Overwrite with mp4", "", "mp4");
        //GetVideoName();
#else
        ShowExplorer();
#endif
        reproduce = true;
        Debug.Log(path);
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
        string[] fileNames = path.Split('/');
        string nameFile = fileNames[fileNames.Length - 1];
        nameFile = nameFile.Split('.')[0];
        Debug.Log(nameFile);
        UploadFileToAWS3(nameFile, path);
        videoPlayer.gameObject.SetActive(false);
        panelVideo.SetActive(false);
    }
    public void StopVideo()
    {
        if (backCam != null)
            backCam.Stop();
    }

    private void OnGUI()
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
                if(GUI.Button(new Rect(Screen.width - 525, Screen.height - 60, 150, 50), "Back"))
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
