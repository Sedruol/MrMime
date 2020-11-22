using UnityEngine;
using System.Collections;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;

public class S3Conection : MonoBehaviour
{
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
        //Post("C:/Users/m/Pictures/Camera Roll", "agenda.mp4");
        //GetBucketList();
    }
    public void Post(string path, string fileName)
    {
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
            }
            else
            {
                print("Exception while posting the result object");
                print(string.Format("receieved error {0}", responseObj.Response.HttpStatusCode.ToString()));
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
