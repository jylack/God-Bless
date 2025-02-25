using UnityEngine;

public class PortalEffect : MonoBehaviour
{
    public Material portalMaterial;
    public float speed = 0.5f;

    void Update()
    {
        if (portalMaterial != null)
        {
            float distortion = Mathf.PingPong(Time.time * speed, 0.5f);
            portalMaterial.SetFloat("_Distortion", distortion);
        }
    }
}
