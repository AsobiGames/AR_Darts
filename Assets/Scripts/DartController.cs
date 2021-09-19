using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DartController : MonoBehaviour
{
    public GameObject dartPrefab;
    public Transform dartThrowPoint;
    ARSessionOrigin aRSession;
    GameObject aRCamera;
    Transform dartboardObj;
    private GameObject dartTemp;
    private Rigidbody rigid;
    private bool isDartboardSearched = false;
    private float distanceFromDartboard = 0f;
    public TMP_Text textDistance;

    private void Start()
    {
        aRSession = GameObject.FindGameObjectWithTag("ARSessionOrigin").GetComponent<ARSessionOrigin>();
        aRCamera = aRSession.transform.Find("AR Camera").gameObject;
    }
    private void OnEnable()
    {
        PlaceObjectOnPlane.onObjectPlaced += DartsInit;
    }
    private void OnDisable()
    {
        PlaceObjectOnPlane.onObjectPlaced -= DartsInit;
    }
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(raycast, out hit)) 
            {
                if (hit.collider.CompareTag("dart"))
                {
                    //Disable back touch collider from dart
                    hit.collider.enabled = false;
                    dartTemp.transform.parent = aRSession.transform;

                    Dart currentDartScript = dartTemp.GetComponent<Dart>();
                    currentDartScript.isForceOK = true;

                    //Load next dart
                    DartsInit();
                }
            }
        }
        if (isDartboardSearched)
        {
            distanceFromDartboard = Vector3.Distance(dartboardObj.position, aRCamera.transform.position);
            textDistance.text = distanceFromDartboard.ToString().Substring(0, 3);
        }
    }

    private void DartsInit()
    {
        dartboardObj = GameObject.FindGameObjectWithTag("dart_board").transform;
        if (dartboardObj)
        {
            isDartboardSearched = true;
        }
        StartCoroutine(WaitAndSpawnDart());
    }
    public IEnumerator WaitAndSpawnDart() 
    {
        yield return new WaitForSeconds(0.1f);
        dartTemp = Instantiate(dartPrefab, dartThrowPoint.position, aRCamera.transform.localRotation);
        dartTemp.transform.parent = aRCamera.transform;
        rigid = dartTemp.GetComponent<Rigidbody>();
        rigid.isKinematic = true;
          
    }
}
