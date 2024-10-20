using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbPoint : MonoBehaviour
{
    [SerializeField] List<Neighbour> neighbours;
    
    private void Awake()
    {
        var twoWayNeighbours = neighbours.Where(n => n.isTwoWay);
        foreach (var neigbour in twoWayNeighbours)
        {
            neigbour.point?.CreateConnection(this, -neigbour.direction, neigbour.connectionType, neigbour.isTwoWay);
        }    
    }

    public void CreateConnection(ClimbPoint point, Vector2 direction, ConnectionType connectionType,
        bool isTwoWay = true)
    {
        var neigbour = new Neighbour()
        {
            point = point,
            direction = direction,
            connectionType = connectionType,
            isTwoWay = isTwoWay
        };
        neighbours.Add(neigbour);
    }

    //For testing drawing line between 2 ledge objects
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        foreach (var neigbour in neighbours)
        {
            if (neigbour.point != null)
                Debug.DrawLine(transform.position, neigbour.point.transform.position, neigbour.isTwoWay? Color.green : Color.gray);
        }
    }
}

[System.Serializable]
public class Neighbour
{
    public ClimbPoint point;
    public Vector2 direction;
    public ConnectionType connectionType;
    public bool isTwoWay = true;
}

public enum ConnectionType { Jump, Move }
