/* Static utilities class - can access from any script */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    // Example - Pass in player's forward vector and the vector from you to the enemy
    // to determine if the enemy is in front or behind of the player
    // dotProduct >= 0 - in front, else - behind
    public static float DotProduct(Vector3 vec1, Vector3 vec2)
    {
        float dotProduct = vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z;
        return dotProduct;
    }
}
