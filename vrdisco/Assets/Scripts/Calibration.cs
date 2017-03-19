using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour {
    public SteamVR_ControllerManager cameraRig;
    public float referenceHandDist = 1.7f;

    public SteamVR_TrackedController leftController;
    public SteamVR_TrackedController rightController;

    public float playerHandDist = 1.75f;
    public float avatarPlayerRatio = 1.0f;


    private void OnEnable() {
        leftController = cameraRig.left.GetComponent<SteamVR_TrackedController>();
        rightController = cameraRig.right.GetComponent<SteamVR_TrackedController>();

        leftController.MenuButtonClicked += CalibrateTPose;
        rightController.MenuButtonClicked += CalibrateTPose;
    }

    void OnDisable() {
        leftController.MenuButtonClicked -= CalibrateTPose;
        rightController.MenuButtonClicked -= CalibrateTPose;
    }

    [ContextMenu("Calibrate")]
    public void CalibrateTPose(object sender, ClickedEventArgs e) {
        // Option 2: T pose + measure distance between controllers. cf. Da Vinci's Vitruvian man.
        // As it turns out, very unrealiable to estimate height. For better or worse, real people don't have perfect proportions.
        playerHandDist = Vector3.Distance(leftController.transform.localPosition, rightController.transform.localPosition);
        avatarPlayerRatio = referenceHandDist / playerHandDist;
        cameraRig.transform.localScale = Vector3.one * avatarPlayerRatio;
    }

    [ContextMenu("Save as referenceHandDist")]
    public void SaveReferenceHandDist() {
        referenceHandDist = Vector3.Distance(leftController.transform.localPosition, rightController.transform.localPosition);
        Debug.Log("New reference hand distance is " + referenceHandDist);
    }
}