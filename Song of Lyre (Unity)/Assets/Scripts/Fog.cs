using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fog : MonoBehaviour
{
    public Material High;
    public Material Low;
    public ScriptableRendererFeature Lines;
    public void toggleFogOn()
    {
        RenderSettings.fog = true;
        RenderSettings.skybox = High;
        Lines.SetActive(true);
    }

    public void toggleFogOff()
    {
        RenderSettings.fog = false;
        RenderSettings.skybox = Low;
        Lines.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
