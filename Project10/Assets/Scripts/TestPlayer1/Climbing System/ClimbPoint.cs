using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbPoint : MonoBehaviour
{
    
}

public class Neighbor
{
    public ClimbPoint point;
    public Vector2 direction;
    public ConnectionType connectionType;
    public bool isTwoWay = true;
}

public enum ConnectionType { Jump, Move }
