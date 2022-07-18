using Obi;
using UnityEngine;

public class SetRandomForceToWater : MonoBehaviour
{
    ObiActorBlueprintEditor target;

    void Start()
    {
        target = GetComponent<ObiActorBlueprintEditor>();
    }

    void Update()
    {
        ////float x = Mathf.PerlinNoise(Time.time, 0) * 2 - 1;
        ////float y = Mathf.PerlinNoise(Time.time, 1f) * 2 - 1;
        ////float z = Mathf.PerlinNoise(Time.time, 2) * 2 - 1;
        ///

        Debug.Log("Upper");

       //target.
        //float r = Mathf.PerlinNoise(Time.time, 0.25f);
        //float g = Mathf.PerlinNoise(Time.time, 0.75f);
        //float b = Mathf.PerlinNoise(Time.time, 0.8f);

        //target.color = new Color(r, g, b);
    }
}
