using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class NYImageTrackerManager : MonoBehaviour
{
    public XRReferenceImageLibrary targetLib;
    public Transform[] trackerObjs;
    TrackingState[] trackerStates;

    ARTrackedImageManager _foundationTrackerManager;
    Dictionary<string, int> trackerImageNameDict;

    public UnityEngine.UI.Text debugText;

    int _imgIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // setup reference
        trackerStates = new TrackingState[targetLib.count];
        trackerImageNameDict = new Dictionary<string, int>();
        for(int i=0; i< trackerObjs.Length; i++)
        {
            trackerImageNameDict.Add(trackerObjs[i].name, i);
            trackerStates[i] = TrackingState.None;
        }


        // init foundation ar image manager
        _foundationTrackerManager = GameObject.FindObjectOfType<ARTrackedImageManager>();

        Debug.Log("Get Manager: " + _foundationTrackerManager.gameObject.name);
        //_foundationTrackerManager = gameObject.AddComponent<ARTrackedImageManager>();
        _foundationTrackerManager.enabled = false;
        _foundationTrackerManager.referenceLibrary = targetLib;
        _foundationTrackerManager.enabled = true;

        Debug.Log("Start AR Manager");
    }


    // Update is called once per frame
    void Update()
    {
        UpdateTrackerStatus();

        if (debugText != null)
        {
            LogTrackerStatus();
        }
    }

    void UpdateTrackerStatus ()
    {
        foreach (ARTrackedImage _arImg in _foundationTrackerManager.trackables)
        {
            _imgIndex = trackerImageNameDict[_arImg.referenceImage.name];

            // state has change!
            if (_arImg.trackingState != trackerStates[_imgIndex])
            {
                trackerStates[_imgIndex] = _arImg.trackingState;

                if (_arImg.trackingState == TrackingState.Tracking)
                {
                    trackerObjs[_imgIndex].GetComponent<NYImageTracker>().OnTrackingFound();
                }
                else if (_arImg.trackingState == TrackingState.Limited)
                {
                    trackerObjs[_imgIndex].GetComponent<NYImageTracker>().OnTrackingLost();
                    /*trackerObjs[_imgIndex].GetComponent<NYImageTracker>().SetActive(false);*/
                }
                else if (_arImg.trackingState == TrackingState.None)
                {
                    // error?
                    Debug.LogError("Object [" + _arImg.referenceImage.name + "] become NONE");
                }
            }

            // update data if tracked
            if (_arImg.trackingState == TrackingState.Tracking)
            {
                UpdateTargetObjStatus(_arImg, _imgIndex);
            }
        }
    }

    void UpdateTargetObjStatus(ARTrackedImage targetImg, int targetIndex)
    {
        Transform targetObj = trackerObjs[targetIndex];

        //targetObj.position = targetImg.transform.position;
        //targetObj.rotation = targetImg.transform.rotation;
        //targetObj.transform.localScale = new Vector3(targetImg.extents.x, 1.0f, targetImg.extents.y);

        targetObj.GetComponent<NYImageTracker>().UpdateTransform(targetImg.transform.position, targetImg.transform.rotation);
    }

    void LogTrackerStatus ()
    {
        string debugMessage = "";
        debugMessage += "trackers: " + _foundationTrackerManager.trackables.count + "\n";

        foreach (ARTrackedImage _arImg in _foundationTrackerManager.trackables)
        {
            debugMessage += _arImg.name + "\n";
            debugMessage += "[" + _arImg.referenceImage.name + "]: " + _arImg.trackingState.ToString() + "\n";
            debugMessage += "extends: " + _arImg.extents.x + "  " + _arImg.extents.y + "\n";
        }

        debugText.text = debugMessage;
    }

}
