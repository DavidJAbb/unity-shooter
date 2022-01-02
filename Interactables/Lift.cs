using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : InteractableReciever
{
    public LiftPlatform liftPlatform;


    // Start is called before the first frame update
    void Start()
    {
        canRecieve = true;
        // IsOn = false;
    }

    // TODO this doen't work - need to consult CCP docs...
    public override void RecieveSignal(bool value)
    {
        /*
        if(liftPlatform.IsMoving)
        {
            return;
        }
        else
        {
            liftPlatform.IsMoving = true;
        }
        */
    }


    public void CallLift(int i)
    {
        liftPlatform.MoveToPosition(i);
    }
}
