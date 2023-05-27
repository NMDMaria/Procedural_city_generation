using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class DayNight : MonoBehaviour
{
    public Light DirectionalLight;

    public LightingPreset Preset;

    [Range(0, 24)] public float TimeOfDay;

    public GameObject parc;

    private void Start()
    {
        parc = GameObject.FindGameObjectWithTag("Parc");
    }

    private void Update()
    {
        if (Preset == null)
            return;

        bool isCycle = parc.GetComponent<Parcuri>().isCycle;

        if (Application.isPlaying && isCycle==true)
        {
            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    // Update is called once per frame
    /*
    void Update()
    {
        float timeOfDay = Time.time / dayDuration;

        // Update the rotation of the sun
        sunLight.transform.rotation = Quaternion.Euler(new Vector3(timeOfDay * 360f, 0f, 0f));

        // Adjust the intensity/color of the sunlight based on the time of day
        float intensity = Mathf.Clamp01(1f - Mathf.Abs(timeOfDay - 0.5f) * 2f); // Dim at night
        sunLight.intensity = intensity;
        sunLight.color = new Color(intensity, intensity, intensity);
    }
    */
}
