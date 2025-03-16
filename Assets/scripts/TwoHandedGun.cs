using UnityEngine;

public class TwoHandedGun : MonoBehaviour
{
    public Transform rightHand;  // Main hand (RightControllerAnchor)
    public Transform leftHand;   // Support hand (LeftControllerAnchor)
    public Transform grabPoint;  // Assign "FrontGripPoint" where left hand should grab
    public float grabDistance = 0.15f; // How close the left hand must be to grab

    //private bool isGrabbing = false;

    private void Update()
    {


        if (rightHand == null || leftHand == null)
            return;

        RotateGunWithBothHands();

        //// **1. Gun follows right hand position (Main Hand)**
        //transform.position = rightHand.position;

        //// **2. Check if Left Hand is near the grab point**
        //float distance = Vector3.Distance(leftHand.position, grabPoint.position);
        //bool isNearGrabPoint = distance <= grabDistance;

        //// **3. Detect Grab Button Press on Left Hand**
        //bool isLeftGripping = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);

        //if (isNearGrabPoint && isLeftGripping)
        //{
        //    if (!isGrabbing)
        //    {
        //        isGrabbing = true;
        //    }

        //    // **Rotate gun based on both hands' positions**
        //    RotateGunWithBothHands();
        //}
        //else
        //{
        //    isGrabbing = false;
        //}
    }

    private void RotateGunWithBothHands()
    {
        // **Gun rotation is now based on the vector between both hands**
        Vector3 direction = (leftHand.position - rightHand.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, rightHand.up);

        // **Smoothly rotate the gun**
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
}
