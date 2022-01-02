using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CoverObject : MonoBehaviour
{
    public CoverPosition[] coverPositions;

    // Get free cover position...
    public CoverPosition GetCoverPosition()
    {
        for(int i = 0; i < coverPositions.Length; i++)
        {
            if(!coverPositions[i].isReserved)
            {
                return coverPositions[0]; // TODO
            }
        }
        return null;
    }
}
