using UnityEngine;

public class LightManager : MonoBehaviour
{
    public LightFlicker[] lights;

    public void LightReset(int index)
    {
        foreach (var light in lights)
        {
            if (light.checkpointIndex == index)
            {
                light.ResetLight();
            }
        }
    }
}
