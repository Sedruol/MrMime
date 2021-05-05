using UnityEngine;
using System.Collections;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using UnityEngine.Networking;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;

public class S3Conection : MonoBehaviour
{
    [SerializeField] public GameObject botones;
    [SerializeField] public GameObject loadingScreen; 
    [SerializeField]public List<string> fileNames;
    [SerializeField] public List<string> options;
    public bool canStart = false;

    private string IdentityPoolId = "us-east-2:b5d7144c-323b-4c5a-abd3-e143731b5f73";
    private string CognitoIdentityRegion = RegionEndpoint.USEast2.SystemName;
    private string S3Region = RegionEndpoint.USEast2.SystemName;
    private string S3BucketName = "tesis-resources"; 
    private IAmazonS3 _s3Client; 
    private AWSCredentials _credentials;
    private RegionEndpoint _CognitoIdentityRegion
    {
        get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
    }
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }
    private AWSCredentials Credentials
    {
        get
        {
            if (_credentials == null)
                _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
            return _credentials;
        }
    }
    private IAmazonS3 Client
    {
        get
        {
            if (_s3Client == null)
            {
                _s3Client = new AmazonS3Client(Credentials, _S3Region);
            }
            //test comment

            return _s3Client;
        }
    }
    //validar conexión
    public void GetBucketList()
    {
        print("Fetching all the Buckets");
        Client.ListBucketsAsync(new ListBucketsRequest(), (responseObject) =>
        {

            if (responseObject.Exception == null)
            {
                print("Got Response \nPrinting now \n");
                responseObject.Response.Buckets.ForEach((s3b) =>
                {
                    print(string.Format("bucket = {0}, created date = {1} \n", s3b.BucketName, s3b.CreationDate));
                });
            }
            else
            {
                print("Got Exception");
            }
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        GetList();
        //GetObject("R_saludar.txt");
        //Post("C:/Users/m/Pictures/Camera Roll", "agenda.mp4");
        //GetBucketList();
    }
    public void Post(string path, string fileName)
    {
        loadingScreen.SetActive(true);
        botones.SetActive(false);
        //path = path + Path.DirectorySeparatorChar + fileName;
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        var request = new PostObjectRequest()
        {
            Bucket = S3BucketName,
            Key = fileName,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };
        Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                print(string.Format("object {0} posted to bucket {1}", 
                    responseObj.Request.Key, responseObj.Request.Bucket));
                StartCoroutine(GetRequest("http://localhost:5000/" + fileName));

            }
            else
            {
                print("Exception while posting the result object");
                print(string.Format("receieved error {0}", responseObj.Response.HttpStatusCode.ToString()));
            }
        });
    }
    public void GetList()
    {
        // ResultText is a label used for displaying status information
        string message = "Fetching all the Objects from " + S3BucketName;
        string word;
        var request = new ListObjectsRequest()
        {
            BucketName = S3BucketName
        };

        Client.ListObjectsAsync(request, (responseObject) =>
        {
            message += "\n";
            if (responseObject.Exception == null)
            {
                message += "Got Response \nPrinting now \n";


                responseObject.Response.S3Objects.ForEach((o) =>
                {

                    word = string.Format("{0}\n", o.Key);
                    //print(word);
                    if (word[0] == 'R')
                    {
                        fileNames.Add(word);
                        GetObject(word.Substring(0,word.Length-1));
                    }
                });
            }
            else
            {
                message += "Got Exception \n";
            }
            print("cargo completamente");
            canStart = true;
            print(message);
        });
    }

    private void GetObject(string SampleFileName)
    {
        print(SampleFileName);
        Client.GetObjectAsync(S3BucketName, SampleFileName, (responseObj) =>
        {
            string data = null;
            var response = responseObj.Response;
            if (response.ResponseStream != null)
            {

                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    data = reader.ReadToEnd();
                    data += "#" + SampleFileName;
                    options.Add(data);

                }


            }
            else
            {
                print("no se encontro");
            }
            
        });
    }
    public void ObtainNotepads()
    {

        print("finish");
        
    }

    
 

    IEnumerator GetRequest(string uri)
    {
        
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            loadingScreen.SetActive(false);
            botones.SetActive(true);
        }
    }
    
}
