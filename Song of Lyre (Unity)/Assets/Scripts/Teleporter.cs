using UnityEngine;
using UnityEngine.VFX;

public class Teleporter : MonoBehaviour
{
    public GameObject player;
    public GameObject teleporter;
    public GameObject vfx;
    public string VFXEventName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        player.transform.position = teleporter.transform.position;
        if (vfx != null) { vfx.GetComponent<VisualEffect>().SendEvent(VFXEventName); }
    }
}
