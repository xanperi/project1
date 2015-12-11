/*******************
This code belongs to the NYU's Mobile Augmented Reality Lab
@author Mark Skwarek
@author Alexandre Pellitero-Rivero
********************/

using UnityEngine;
using Vuforia;
using System.Net;
using System.IO;
using System;

public class ModelSwapper : MonoBehaviour {

    public TrackableBehaviour theTrackable;
    private bool mSwapModel = false;
    private string baseServerUrl = "http://172.16.9.203:33333/";

    static string myLog;

    // Use this for initialization
    void Start()
    {
        if (theTrackable == null)
        {
            Debug.Log("Warning: Trackable not set !!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (mSwapModel && theTrackable != null)
        {
            SwapModel();
            mSwapModel = false;
        }
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 240, 80), "Swap Model"))
        {
            mSwapModel = true;
        }

        myLog = GUI.TextArea(new Rect(350, 60, Screen.width - 400, Screen.height - 400), myLog);
    }

    /**
     * Function to make a GET request to a URL
     *
     * @param url
     *  String containing the URL of the service
     *
     * @return string
     *  String returned by the get petition
     */
    private string HttpGet(string url)
    {
        HttpWebRequest req = WebRequest.Create(url)
                             as HttpWebRequest;
        string result = null;
        using (HttpWebResponse resp = req.GetResponse()
                                      as HttpWebResponse)
        {
            StreamReader reader =
                new StreamReader(resp.GetResponseStream());
            result = reader.ReadToEnd();
        }
        return result;
    }

    /**
     * Change the current overlaid model for a new one
     */
    private void SwapModel()
    {
        GameObject trackableGameObject = theTrackable.gameObject;
        //disable any pre-existing augmentation
        for (int i = 0; i < trackableGameObject.transform.childCount; i++)
        {
            Transform child = trackableGameObject.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }

        string nextModel = HttpGet(baseServerUrl);
        string fullNextModelUrl = baseServerUrl + nextModel;
        myLog = myLog + "\r\n" + fullNextModelUrl ;
        //BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
        // Create a simple cube object
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Re-parent the cube as child of the trackable gameObject
        cube.transform.parent = theTrackable.transform;
        // Adjust the position and scale
        // so that it fits nicely on the target
        cube.transform.localPosition = new Vector3(0, 0.2f, 0);
        cube.transform.localRotation = Quaternion.identity;
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        // Make sure it is active
        cube.SetActive(true);
    }

}