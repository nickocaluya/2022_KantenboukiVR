using UnityEngine;
using Valve.VR;

using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;

public class CloudLabelSelect : MonoBehaviour
{
    public Transform thumb;
    bool dragging;

    public SteamVR_TrackedObject rightController;


    public SteamVR_Action_Boolean cloudLabelSel;

    public Hand hand;

    void FixedUpdate()
    {
        
    }

    private void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();

        if (cloudLabelSel == null)
        {
            Debug.LogError("<b>[SteamVR Interaction]</b> No plant action assigned", this);
            return;
        }

        cloudLabelSel.AddOnChangeListener(OnSelectActionChange, hand.handType);
    }

    private void OnDisable()
    {
        if (cloudLabelSel != null)
            cloudLabelSel.RemoveOnChangeListener(OnSelectActionChange, hand.handType);
    }

    private void OnSelectActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
    {
        if (newValue)
        {
            SelectCloudLabel();
        }
    }

    public void SelectCloudLabel()
    {
        StartCoroutine(SelectLabel());
    }


    private IEnumerator SelectLabel()
    {
        //SteamVR_Controller.Device device = SteamVR_Controller.Input((int)rightController.index);
        //right hand

        //device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)
        if (cloudLabelSel.stateDown)
        {
            dragging = false;
            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (GetComponent<Collider>().Raycast(ray, out hit, 100))
            {
                dragging = true;
                ColorManager.Instance.cloudLabel = GetComponent<Collider>().gameObject.name;
                thumb.position = GetComponent<Collider>().gameObject.transform.position;
            }
        }
        //device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)
        if (cloudLabelSel.stateUp) dragging = false;
        if (dragging && cloudLabelSel.state)
        {
            Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
            //var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            if (GetComponent<Collider>().Raycast(ray, out hit, 100))
            {
                ColorManager.Instance.cloudLabel = GetComponent<Collider>().gameObject.name;
                thumb.position = GetComponent<Collider>().gameObject.transform.position;
            }

        }

        float startTime = Time.time;
        float overTime = 0.5f;
        float endTime = startTime + overTime;

        while (Time.time < endTime)
        {
            yield return null;
        }
    }
}
