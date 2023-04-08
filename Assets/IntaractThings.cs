using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntaractThings : MonoBehaviour
{
    public bool isStickable;
    private Rigidbody _rigidbody;
    private FixedJoint fixedjoint;
    public List<GameObject> connectedObjects;
    private bool isBreak;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        fixedjoint = GetComponent<FixedJoint>();
        ReAdd();
    }
    public void ReAdd()
    {
        connectedObjects.Add(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
       if(!GameManager.instance.stickLock)
        {
            if (other.gameObject.CompareTag("Bone") && isStickable)
            {
                var othergameobject = other.gameObject;
                othergameobject.GetComponent<FixedJoint>().connectedBody = _rigidbody;
                fixedjoint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                // othergameobject.GetComponent<BoxCollider>().enabled = false;
              
                foreach (var connectedbone in connectedObjects)
                {
                    if (othergameobject.transform.name == connectedbone.transform.name)
                        return;
                }
                connectedObjects.Add(othergameobject);
                var otherinteractor = othergameobject.GetComponent<IntaractThings>();


                foreach (var connectedobj in connectedObjects)
                {
                    isBreak = false;
                    for (var i = 0; i < otherinteractor.connectedObjects.Count; i++)
                    {
                        if (otherinteractor.connectedObjects[i] == connectedobj)
                            isBreak = true;
                    }
                    if (isBreak)
                    {
                        continue;
                    }
                    otherinteractor.connectedObjects.Add(connectedobj);
                }
                UpdateConnectedObjects(otherinteractor);

                foreach (var otherconnectedobj in otherinteractor.connectedObjects)
                {
                    var otherconnectedinteractor = otherconnectedobj.GetComponent<IntaractThings>();

                    foreach (var otherobj in otherconnectedinteractor.connectedObjects)
                    {
                        if (otherobj == otherconnectedobj)
                            isBreak = true;
                    }
                    if (isBreak)
                    {
                        continue;
                    }
                    otherconnectedinteractor.connectedObjects.Add(otherconnectedobj);
                }
                Check(otherinteractor);
            }
        }
    }
 
    private void UpdateConnectedObjects(IntaractThings otherinteractor)
    {
        foreach (var otherconnectedobj in otherinteractor.connectedObjects)
        {
            var i = 0;
            isBreak = false;
              for(;i < connectedObjects.Count;i++)
            {
                if (connectedObjects[i] == otherconnectedobj)
                    isBreak = true;
            }
            if (isBreak)
            {
                continue;
            }
            connectedObjects.Add(otherconnectedobj);
        }
    }

    private void Check(IntaractThings otherinteractor)
    {
        foreach(var otherobj in otherinteractor.connectedObjects)
         {
            otherobj.GetComponent<IntaractThings>().connectedObjects = otherinteractor.connectedObjects;   
            
        }
    }
    private void CheckChildrenCollisions()
    {

    }
    public void Grab()
    {
      /*  foreach (var bone in GameManager.instance.bones)
        {
            foreach (var connected in connectedObjects)
            {
                if (connected.transform.name == bone.transform.name)
                {
                    Debug.Log("returned");
                    isBreak = true;
                }
            }
            if (!isBreak)
            {
                Debug.Log("Grabbed");
                var collider = bone.GetComponent<BoxCollider>();
                collider.isTrigger = true;
                isStickable = true;
            }

        }*/
        foreach (var bone in GameManager.instance.bones)
        {
            Debug.Log("Grabbed");
            var collider = bone.GetComponent<BoxCollider>();
            collider.isTrigger = true;
            foreach(var connected in connectedObjects)
            {
                    connected.GetComponent<IntaractThings>().isStickable = true;
                //  connected.GetComponent<BoxCollider>().isTrigger = false;
                if(connected.gameObject != this.gameObject)
                connected.GetComponent<Rigidbody>().isKinematic = false;
            }
            _rigidbody.isKinematic = true;

        }
    }
    public void Release()
    {
        foreach (var bone in GameManager.instance.bones)
        {
            var collider = bone.GetComponent<BoxCollider>();
            collider.isTrigger = false;
            isStickable = false; 
           bone.GetComponent<Rigidbody>().isKinematic = true;

        }
    }
}