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
    [SerializeField] private List<GameObject> rocks;
    [SerializeField] private List<Material> rock_Materials;
    [SerializeField] private List<Material> matsonTable;
    [SerializeField] private List<XRSocketInteractor> SocketInteractors;
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
        foreach (var rock in rocks)
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
    }  
    public void OutTable(XRBaseInteractable interactable)
    {
      var outtableobject = interactable.gameObject;
        var mat = outtableobject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material;
        matsonTable.Remove(mat);
    }
}
