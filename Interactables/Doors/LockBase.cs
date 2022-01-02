using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBase : MonoBehaviour
{
    public Door door;

    public virtual void TryUnlock()
    {
        // Lock specific code here...
    }
}
