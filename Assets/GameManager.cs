using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager :  MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Slider rSlider;
    [SerializeField] private Slider gSlider;
    [SerializeField] private Slider bSlider;
    public List<GameObject> bones;
    public List<IntaractThings> intaractors;
    [SerializeField] private List<Material> rock_Materials;
    [SerializeField] private List<Material> matsonTable;
    [SerializeField] private List<XRSocketInteractor> SocketInteractors;
    public List<GameObject> ontableObjects;
    public bool stickLock;
    void Start()
    {

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        UpdateSocketInteractors();
        foreach(var bone in bones)
        {
            intaractors.Add(bone.GetComponent<IntaractThings>());
        }
       
    }
    private void UpdateSocketInteractors()
    {
        foreach(var socket in SocketInteractors)
        {
            socket.onSelectEntered.AddListener(OnTable);
            socket.onSelectExited.AddListener(OutTable);
        }
    }
    private void GetObjectMaterials()
    {
        var i = 0;
        foreach (var rock in bones)
        {
            rock_Materials[i] = rock.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
            i++;
        }
    }
    public void ChangeColor()
    {
        foreach(var mat in matsonTable)
        {
            var color = new Color(rSlider.value, gSlider.value, bSlider.value, 1f);
            mat.color = color;
        }

    }
    void FixedUpdate()
    {
    }

    public void OnTable(XRBaseInteractable interactable)
    {
      var ontableobject = interactable.gameObject;
        var mat = ontableobject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        matsonTable.Add(mat);
        ontableObjects.Add(ontableobject);
    }  
    public void OutTable(XRBaseInteractable interactable)
    {
      var outtableobject = interactable.gameObject;
        var mat = outtableobject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        matsonTable.Remove(mat);
        ontableObjects.Remove(outtableobject);
    }
    public void Reset()
    {
        foreach(var ontableobject in ontableObjects)
        {
            ontableobject.GetComponent<IntaractThings>().Release();
        }
    }
    public void SetStick()
    {
        if(!stickLock)
        {
            foreach (var bone in bones)
            {
                bone.GetComponent<FixedJoint>().connectedBody = null;
               for(var i=1;i < intaractors[i].connectedObjects.Count; i++)
                {
                    intaractors[i].connectedObjects.RemoveAt(i);
                }

            }
            stickLock = true;
            return;
        }
        else 
        {
            stickLock = false;

       
        }
      

    }
}
