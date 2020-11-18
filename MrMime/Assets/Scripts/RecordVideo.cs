using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.Video;

public class RecordVideo : MonoBehaviour
{
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
    void Start()
    {
        panelVideo.SetActive(false);
        camaraVideo.gameObject.SetActive(false);
        VideoCaptureCtlr.SetActive(false);
        //btnRecordVideo.onClick.AddListener(() => OpenCamera());
        camara = false;
        targetPath = System.IO.Directory.GetCurrentDirectory();
        targetPath = targetPath.Replace(@"\", "/");
        reproduce = true;
        videoPlayer.gameObject.SetActive(false);
        video = videoPlayer.GetComponent<VideoPlayer>();
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

    public void OpenExplorer()
    {
        path = EditorUtility.OpenFilePanel("Overwrite with mp4", "", "mp4");
        Debug.Log(path);
        GetVideoName();
        //System.IO.File.Copy()
        //GetVideo();
    }
    public void GetVideoName()
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
    }
    public void AddVideo()
    {
        Debug.Log("enviar a Amazon AWS");
        videoPlayer.gameObject.SetActive(false);
        panelVideo.SetActive(false);
    }

    private void OnGUI()
    {
        if (!camara)
        {
            if (!panelVideo.activeSelf)
            {
                if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "Record Video"))
                {
                    OpenCamera();
                }
                if (GUI.Button(new Rect(465, Screen.height - 60, 150, 50), "Upload Video"))
                {
                    OpenExplorer();
                }
            }
            else
            {
                if (GUI.Button(new Rect(300, Screen.height - 60, 150, 50), "Add Video"))
                {
                    AddVideo();
                }
                if (GUI.Button(new Rect(465, Screen.height - 60, 150, 50), "Upload Video"))
                {
                    OpenExplorer();
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (reproduce && System.IO.File.Exists(desFile))
        {
            video.clip = Resources.Load<VideoClip>(search);
            if (videoPlayer.clip != null)
            {
                videoPlayer.gameObject.SetActive(true);
                panelVideo.gameObject.SetActive(true);
                reproduce = false;
            }
        }
    }
}
