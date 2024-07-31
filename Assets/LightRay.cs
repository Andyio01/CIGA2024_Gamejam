using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class LightRay : MonoBehaviour
{
    private Transform lightSource;
    private GameObject lightRayPrefab;
    public LayerMask layerMask;

    private LineRenderer linerenderer;
    private List<LineRenderer> lightRays = new List<LineRenderer>();
    [SerializeField] public ParticleSystem EmissionPoint;

    [SerializeField] private Color color;
    [SerializeField] private float colorIntensity = 4.3f;
    [SerializeField] private float thickness = 9;
    [SerializeField] private float noiseScale = 1f;
    [SerializeField] private float enhance = 3f;

    private void Start()
    {
        layerMask |= LayerMask.GetMask("IgnoreLaser");
        layerMask |= LayerMask.GetMask("Player");
        layerMask = ~layerMask;
        linerenderer = GetComponentInChildren<LineRenderer>();
        linerenderer.enabled = true;
        linerenderer.material.color = color * colorIntensity;
        linerenderer.material.SetFloat("_Scale", noiseScale);
        linerenderer.material.SetFloat("_Thickness", thickness);
        lightSource = EmissionPoint.GetComponent<Transform>();
        lightRayPrefab = gameObject;
    }
     private void UpdateStartPosition(Vector2 direction)
    {
        EmissionPoint.transform.position = (Vector2)transform.position + direction * 0.2f;
        linerenderer.SetPosition(0, EmissionPoint.transform.position);
        // Relinerenderer.SetPosition(0, EmissionPoint.transform.position);

    }
    private void Update()
    {
        float rotationZ = transform.rotation.eulerAngles.z; //degree
        rotationZ *= Mathf.Deg2Rad; //radian

        Vector2 direction = new Vector2(math.cos(rotationZ), math.sin(rotationZ));

        // UpdateStartPosition(direction);
        DestroyAllLightRays();

        RaycastHit2D hit = Physics2D.Raycast(EmissionPoint.transform.position, direction, Mathf.Infinity, layerMask);
        if (hit.collider != null)
        {
            Debug.DrawLine(lightSource.position, hit.point, Color.red);
            CreateLightRay(lightSource.position, hit.point, hit.normal, lightSource.forward, 1);
        }
    }

    private void CreateLightRay(Vector3 startPoint, Vector3 hitPoint, Vector3 normal, Vector3 incomingDirection, int depth)
    {
        if (depth > 20) return; // Limit recursion depth
        Debug.Log("hit something");
        // Create LineRenderer for initial ray
        LineRenderer lr = Instantiate(lightRayPrefab).GetComponentInChildren<LineRenderer>();
        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, hitPoint);
        lightRays.Add(lr);

        // Calculate reflection direction
        Vector3 reflectDir = Vector3.Reflect(incomingDirection, normal);
        // Calculate refraction direction using Snell's Law
        float n1 = 1.0f; // Air
        float n2 = 1.33f; // Water
        Vector3 refractDir = Refract(incomingDirection, normal, n1 / n2);

        // Raycast for reflected ray
        RaycastHit2D reflectHit = Physics2D.Raycast(hitPoint, reflectDir, Mathf.Infinity, layerMask);
        if (reflectHit.collider != null)
        {
            CreateLightRay(hitPoint, reflectHit.point, reflectHit.normal, reflectDir, depth + 1);
        }

        // Raycast for refracted ray
        RaycastHit2D refractHit = Physics2D.Raycast(hitPoint, reflectDir, Mathf.Infinity, layerMask);
        if (refractHit.collider != null)
        {
            CreateLightRay(hitPoint, refractHit.point, refractHit.normal, refractDir, depth + 1);
        }
    }

    private Vector3 Refract(Vector3 incident, Vector3 normal, float n)
    {
        float cosI = -Vector3.Dot(normal, incident);
        float sinT2 = n * n * (1.0f - cosI * cosI);
        if (sinT2 > 1.0f) return Vector3.zero; // Total internal reflection
        float cosT = Mathf.Sqrt(1.0f - sinT2);
        return n * incident + (n * cosI - cosT) * normal;
    }

    private void DestroyAllLightRays()
    {
        foreach (var lr in lightRays)
        {
            Destroy(lr.gameObject);
        }
        lightRays.Clear();
    }
}