using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    bool started = true;
    public GameObject prefab;
    public GameObject grid;
    public S3Conection s3Conection;
    public GameObject loadingScreen;
    
    void Update()
    {
        
        if (s3Conection.canStart && started)
        {

            if(s3Conection.options.Count == s3Conection.fileNames.Count)
            {
                s3Conection.ObtainNotepads();
                int iterat = 0;
                foreach (string s in s3Conection.options)
                {
                    string[] separat = s.Split(char.Parse("#"));
                    var pref = Instantiate(prefab, transform.position, Quaternion.identity);
                    pref.transform.SetParent(grid.transform);
                    pref.transform.localScale = new Vector3(1, 1, 1);
                    pref.GetComponent<Option>().content = separat[0]; 
                    string newMes = separat[1];
                    newMes = newMes.Substring(2,newMes.Length-6);
                    pref.transform.GetChild(0).GetComponent<Text>().text = newMes;
                    iterat++;
                }
                started = false;
                loadingScreen.SetActive(false);
            }
            
            
        }
    }
}
