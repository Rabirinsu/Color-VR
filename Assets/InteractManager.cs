using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractManager : XRSocketInteractor
{
   
 
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        var ontableobect = args.interactor.gameObject;
        Debug.Log("WAWAEWAE " + ontableobect.transform.name);
        //GameManager.instance.OnTable(ontableobect);
    }
}
