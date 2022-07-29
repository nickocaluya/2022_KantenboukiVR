using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem.Sample
{
    public class DrawAnnotation : MonoBehaviour
    {
        public SteamVR_Action_Boolean drawAnnotation;

        public Hand hand;

        public GameObject prefabToPlant;

        public GameObject drawingPoint;
        public ColorManager colMgr;
        private MeshLineRenderer currLine;

        private int numClicks = 0;
        private int numClouds = 0;

        public Material lMat;

        private float timer = 0;

        List<GameObject> annotations = new List<GameObject>();


        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (drawAnnotation == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> No plant action assigned", this);
                return;
            }

            drawAnnotation.AddOnChangeListener(OnDrawActionChange, hand.handType);
        }

        private void OnDisable()
        {
            if (drawAnnotation != null)
                drawAnnotation.RemoveOnChangeListener(OnDrawActionChange, hand.handType);
        }

        private void OnDrawActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
        {
            if (newValue)
            {
                Draw();
            }
        }

        public void Draw()
        {
            StartCoroutine(DoDraw());
        }

        private IEnumerator DoDraw()
        {
        
            //if (trigger.x > 0.1f && timer > 1.0f)
            //{
            //    timer = 0;
                GameObject go = new GameObject(ColorManager.Instance.cloudLabel);
                go.AddComponent<MeshFilter>();
                go.AddComponent<MeshRenderer>();


                currLine = go.AddComponent<MeshLineRenderer>();
                currLine.setColorManager(colMgr);
                currLine.setWidth(0.1f);

                currLine.lmat = this.lMat;

                GetComponent<MeshRenderer>().material.color = colMgr.color;

                Vector3 dir = hand.transform.position - hand.transform.forward * 2.0f;
                Vector3 left = Vector3.Cross(dir, Vector3.up).normalized;

                if (ColorManager.Instance.cloudLabel != "_None" && ColorManager.Instance.cloudLabel != null)
                {
                    //trackedObj.transform.position + trackedObj.transform.forward * 2.0f
                    GameObject label = Instantiate(Resources.Load(ColorManager.Instance.cloudLabel), drawingPoint.transform.position, Camera.main.transform.rotation) as GameObject;
                    if (label.GetComponent<CloudLabelSelect>() != null)
                    {
                        Destroy(label.GetComponent<CloudLabelSelect>());
                    }


                    label.transform.eulerAngles = new Vector3(label.transform.eulerAngles.x, label.transform.eulerAngles.y, 180); //label.transform.RotateAround(transform.position, transform.up, 180f);
                    label.transform.localScale = new Vector3(0.5F, 0.5f, 0);

                    label.transform.parent = go.transform;

                    if (label.GetComponent<Collider>() != null)
                    {
                        Destroy(label.GetComponent<Collider>());
                    }

                }

                annotations.Add(go);


                numClicks = 0;
            //}

            float startTime = Time.time;
            float overTime = 0.5f;
            float endTime = startTime + overTime;

            while (Time.time < endTime)
            {
                yield return null;
            }

        }

        private void AddPoint()
        {
            if (currLine != null)
            {
                currLine.AddPoint(drawingPoint.transform.position);
            }
        }
    }
}