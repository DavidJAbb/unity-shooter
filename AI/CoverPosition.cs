using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverPosition : MonoBehaviour
{
    public enum CoverType
    {
        LOW_COVER, // for low walls, crates etc.
        STANDING_LEFT, // for wall corners on the left
        STANDING_RIGHT, // for wall corners on the right
        STANDING_BOTH // for columns
    }
    public CoverType coverType;
    public bool isReserved; // An agent has reserved it
}
