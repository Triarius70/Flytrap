using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScaleParticles : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponentInChildren<ParticleSystem>().startSize = transform.lossyScale.magnitude;
    }
}
