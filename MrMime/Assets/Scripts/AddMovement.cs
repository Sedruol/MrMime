using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddMovement : MonoBehaviour
{
    [SerializeField] private Button btnRecordVideo;
    [SerializeField] private Button btnGrabaVideo;
    static WebCamTexture backCam;
    private MeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        mesh.enabled = false;
        btnRecordVideo.onClick.AddListener(() => RecordVideo());
        btnGrabaVideo.onClick.AddListener(() => StartRecord());
    }

    public void RecordVideo()
    {
        if (backCam == null)
            backCam = new WebCamTexture();
        GetComponent<Renderer>().material.mainTexture = backCam;

        if (!backCam.isPlaying)
        {
            backCam.Play();
            mesh.enabled = true;
        }
        else
        {
            mesh.enabled = false;
            backCam.Pause();
        }
    }

    public void StartRecord()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
