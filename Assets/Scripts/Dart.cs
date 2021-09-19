using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Dart : MonoBehaviour
{
    private Rigidbody rigid;
    private GameObject dirObj;
    public bool isForceOK = false;
    bool isDartRotating = false;
    bool isDartreadyToShoot = true;
    bool isDartHitOnBoard = false;

    ARSessionOrigin aRSession;
    GameObject aRCamera;

    public Collider dartFrontCollider;

    private void Start()
    {
        aRSession = GameObject.FindGameObjectWithTag("ARSessionOrigin").GetComponent<ARSessionOrigin>();
        aRCamera = aRSession.transform.Find("AR Camera").gameObject;

        if(TryGetComponent(out Rigidbody rg))
        rigid = rg;
        dirObj = GameObject.FindGameObjectWithTag("dartThrowPoint");
    }
    private void FixedUpdate()
    {
        if (isForceOK)
        {
            dartFrontCollider.enabled = true;
            StartCoroutine(InitDartDestroyVFX());
            GetComponent<Rigidbody>().isKinematic = false;
            isForceOK = true;
            isDartRotating = true;
        }

        //Add force
        rigid.AddForce(dirObj.transform.forward * (12f + 6f) * Time.deltaTime, ForceMode.VelocityChange);

        //Dart Ready

        if (isDartreadyToShoot) 
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * 20f);
        }
        if (isDartRotating)
        {
            isDartreadyToShoot = false;
            transform.Rotate(Vector3.forward * Time.deltaTime * 400f);
        }
    }

    private IEnumerator InitDartDestroyVFX()
    {
        yield return new WaitForSeconds(5f);
        if (!isDartHitOnBoard)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dart_board")) 
        {
            //Trigger phone vibration
            Handheld.Vibrate();

            GetComponent<Rigidbody>().isKinematic = true;
            isDartRotating = false;

            //Dart hit the board
            isDartHitOnBoard = true; 
        }
    }
}
