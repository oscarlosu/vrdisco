using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Source: http://russellsoftworks.com/blog/steamvr_01//// 
/// </summary>
public class WandController : SteamVR_TrackedController {
    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)controllerIndex); } }
    public Vector3 velocity { get { return controller.velocity; } }
    public Vector3 angularVelocity { get { return controller.angularVelocity; } }

    protected override void Start () {
        base.Start();
	}

    protected override void Update () {
        base.Update();
	}

    public float GetTriggerAxis() {
        // If the controller isn't valid, return 0
        if (controller == null)
            return 0;

        // Use SteamVR_Controller.Device's GetAxis() method (mentioned earlier) to get the trigger's axis value
        return controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis1).x;
    }

    public Vector2 GetTouchpadAxis() {
        // If the controller isn't valid, return (basically) 0
        if (controller == null)
            return new Vector2();

        // Use SteamVR_Controller.Device's GetAxis() method (mentioned earlier) to get the touchpad's axis value
        return controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
    }

    #region Events
    public override void OnTriggerClicked(ClickedEventArgs e) {
        base.OnTriggerClicked(e);
    }

    public override void OnTriggerUnclicked(ClickedEventArgs e) {
        base.OnTriggerUnclicked(e);
    }

    public override void OnMenuClicked(ClickedEventArgs e) {
        base.OnMenuClicked(e);
    }

    public override void OnMenuUnclicked(ClickedEventArgs e) {
        base.OnMenuUnclicked(e);
    }

    public override void OnSteamClicked(ClickedEventArgs e) {
        base.OnSteamClicked(e);
    }

    public override void OnPadClicked(ClickedEventArgs e) {
        base.OnPadClicked(e);
    }

    public override void OnPadUnclicked(ClickedEventArgs e) {
        base.OnPadUnclicked(e);
    }

    public override void OnPadTouched(ClickedEventArgs e) {
        base.OnPadTouched(e);
    }

    public override void OnPadUntouched(ClickedEventArgs e) {
        base.OnPadUntouched(e);
    }

    public override void OnGripped(ClickedEventArgs e) {
        base.OnGripped(e);
    }

    public override void OnUngripped(ClickedEventArgs e) {
        base.OnUngripped(e);
    }

    #endregion
}
