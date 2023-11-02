using UnityEngine;

public class LaserLine : MonoBehaviour
{
    private LineRenderer[] lines;

    public Vector3 Target => lines[0].transform.position;
    
    void Awake()
    {
        lines = GetComponentsInChildren<LineRenderer>();
    }

    public void SetLaserLines(Vector3 target)
    {
        foreach (var line in lines)
        {
            line.SetPosition(0, line.transform.position);
            line.SetPosition(1, new Vector3(target.x, line.transform.position.y, target.z));
        }
    }
}
