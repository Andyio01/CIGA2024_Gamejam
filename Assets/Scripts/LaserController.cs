using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;









public class LaserController : MonoBehaviour
{

    public bool deadly;
    private LineRenderer linerenderer;
    private LineRenderer Relinerenderer;
    private float time;
    private float InitAngle;
    private Vector2 StartPosition;
    public LayerMask layerMask;
    private float tolerance = 0.01f; // Adjust the tolerance as needed

    [SerializeField] public ParticleSystem EmissionPoint;
    [SerializeField] private GameObject startVFX;
    [SerializeField] private GameObject endVFX;
    [SerializeField] public GameObject RestartVFX;

    [SerializeField] private GameObject ReendVFX;

    // Control the rotation of the Launcher
    [SerializeField] private bool Rotation;
    [SerializeField] private float RotationPeriod;
    [SerializeField] private float RotationAngle;
    [SerializeField] private Color color;
    [SerializeField] private float colorIntensity = 4.3f;
    [SerializeField] private float thickness = 9;
    [SerializeField] private float noiseScale = 1f;
    [SerializeField] private float enhance = 3f;
    float minDistanceThreshold = 3f; // Adjust as needed
    public float refractiveIndex1 = 1.0f; // 空气的折射率
    public float refractiveIndex2 = 1.5f; // 目标介质的折射率

    // Start is called before the first frame update

    void Start()
    {
        layerMask |= LayerMask.GetMask("IgnoreLaser");
        layerMask |= LayerMask.GetMask("Player");
        layerMask = ~layerMask;
        linerenderer = GetComponentInChildren<LineRenderer>();
        linerenderer.enabled = true;
        linerenderer.material.color = color * colorIntensity;
        linerenderer.material.SetFloat("_Scale", noiseScale);
        linerenderer.material.SetFloat("_Thickness", thickness);
        Relinerenderer = GetComponentInChildren<LineRenderer>();
        Relinerenderer.enabled = true;
        Relinerenderer.material.color = color * colorIntensity;
        Relinerenderer.material.SetFloat("_Scale", noiseScale);
        Relinerenderer.material.SetFloat("_Thickness", thickness);
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            Renderer rend = particle.GetComponent<Renderer>();
            // rend.material.SetColor("_BaseColor", color * colorIntensity);
            rend.material.SetColor("_EmissionColor", color * (colorIntensity + enhance));
        }
        // linerenderer.SetPosition(1, new Vector3(1, 0, 0));
        InitAngle = transform.rotation.eulerAngles.z;
        StartPosition = EmissionPoint.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        float rotationZ = transform.rotation.eulerAngles.z; //degree
        rotationZ *= Mathf.Deg2Rad; //radian

        Vector2 direction = new Vector2(math.cos(rotationZ), math.sin(rotationZ));

        UpdateStartPosition(direction);
        UpdateEndPosition(direction);
        if (Rotation) RotateLauncher();

    }

    private void UpdateStartPosition(Vector2 direction)
    {
        EmissionPoint.transform.position = (Vector2)transform.position + direction * 0.2f;
        linerenderer.SetPosition(0, EmissionPoint.transform.position);
        Relinerenderer.SetPosition(0, EmissionPoint.transform.position);

    }
    private void UpdateEndPosition(Vector2 direction)
    {
        
        if(transform.GetComponent<PointController>() && transform.GetComponent<PointController>().isHit == false) return;
        
        float length = 100f;
        float laserEndRotation = 180;
        Vector2 endPosition;
        linerenderer.positionCount = 1;
        int i = 0;
        int j = 0;
        Vector2 curPosition = EmissionPoint.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(curPosition, direction.normalized, length, layerMask);






        LightCheck(ref direction, ref length, ref laserEndRotation, ref i, ref curPosition, ref hit, new HashSet<Vector2>());

        // linerenderer.SetPosition(1, new Vector3(length, 0, 0));
        //  endVFX.SetActive(true); 
        //infrared(direction, length, laserEndRotation, ref i, ref curPosition);
        // infrared(direction, length, laserEndRotation, ref i, ref curPosition, ref hit);

        void infrared(Vector2 direction, float length, float laserEndRotation, ref int i, ref Vector2 curPosition, ref RaycastHit2D hit)
        {
            curPosition = curPosition + direction * length;
            startVFX.transform.position = StartPosition;
            endVFX.transform.position = curPosition;
            endVFX.transform.rotation = Quaternion.Euler(0, 0, laserEndRotation);
            linerenderer.positionCount++;
            linerenderer.SetPosition(++i, curPosition);
        }

        void Reinfrared(Vector2 direction, float length, float laserEndRotation, ref int i, ref Vector2 curPosition, ref RaycastHit2D hit)
        {
            //Debug.Log(hit.transform.position);
            GameObject Res = Instantiate(RestartVFX, hit.point, Quaternion.identity);
            
            Res.transform.SetParent(transform);
            Res.transform.position = hit.point;
            curPosition = curPosition + direction * length;
            
            ReendVFX.transform.position = curPosition;
            ReendVFX.transform.rotation = Quaternion.Euler(0, 0, laserEndRotation);
            Debug.Log(laserEndRotation);
            Relinerenderer.positionCount++;
            Relinerenderer.SetPosition(++i, curPosition);
        }


        void LightCheck(ref Vector2 direction, ref float length, ref float laserEndRotation, ref int i, ref Vector2 curPosition, ref RaycastHit2D hit, HashSet<Vector2> visitedPoints)
        {
            
            while (hit.collider != null && i < 100)
            {
                // Hit the player
                // if(hit.transform.gameObject.name.Equals("Player") && deadly){
                //         GameManager.DeathNum[SceneManager.GetActiveScene().name] += 1;
                //         PlayerController.justDied = true;
                //         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //         break;
                //     }
                // Hit the point

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Point"))
                {
                    // Debug.Log("打到了点上");
                    hit.transform.gameObject.GetComponent<PointController>().hitByLaser(linerenderer);
                }
                if (hit.transform.gameObject.tag == "Reciever") {
                    // Logic for the reciever(win the game, show hidden object, etc.)
                    hit.transform.gameObject.GetComponent<Reciever>().CameraMove();
                }
                // Hit the object that is not reflectable

                // Hit the object that is reflectable
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Reflectable"))
                {
                    // reflectLaser(StartPosition, direction, hit);
                    // endVFX.SetActive(false);
                    // if (hit.transform.gameObject.tag == "Mirror") {
                    //     // Logic for the mirror(win the game, show hidden object, etc.)
                    //     hit.transform.gameObject.GetComponentInParent<HittedAndLight>().Hitted();
                    //     // Debug.Log("Hit the mirror");
                    // }
                    curPosition = hit.point;
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, curPosition);
                    Vector2 normal = hit.normal;
                    direction = Vector2.Reflect(direction.normalized, normal);

                    hit = Physics2D.Raycast(curPosition + 0.01f * direction, direction, 1000f, layerMask);
                    Debug.DrawLine(curPosition, curPosition + direction * 100f, Color.red);
                    // infrared(direction, length, laserEndRotation, ref i, ref curPosition, ref hit);
                }
                else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Refractable"))
                {
                    // 计算反射
                    curPosition = hit.point;
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, curPosition);
                    Vector2 normal = hit.normal;
                    direction = Vector2.Reflect(direction.normalized, normal);

                    hit = Physics2D.Raycast(curPosition + 0.01f * direction, direction, 1000f, layerMask);
                    Debug.DrawLine(curPosition, curPosition + direction * 100f, Color.red);

                    // 计算折射
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, curPosition); //新建折射起点

                    float n1 = refractiveIndex1;
                    float n2 = refractiveIndex2;

                    float cosI = -Vector3.Dot(normal, direction);
                    float sinT2 = (n1 / n2) * (n1 / n2) * (1.0f - cosI * cosI);
                    if (sinT2 <= 1.0f)
                    {
                        float cosT = Mathf.Sqrt(1.0f - sinT2);
                        Vector2 refractedDirection = (n1 / n2) * direction + (n1 / n2 * cosI - cosT) * normal;
                        RaycastHit2D ExtraHit = Physics2D.Raycast(curPosition + 0.01f * refractedDirection, refractedDirection, length, layerMask);
                        Debug.DrawLine(curPosition, curPosition + refractedDirection * 100f, Color.blue);
                        LightCheck(ref refractedDirection, ref length, ref laserEndRotation, ref i, ref curPosition, ref ExtraHit, new HashSet<Vector2>());
                        
                    }
                    LightCheck(ref direction, ref length, ref laserEndRotation, ref i, ref curPosition, ref hit, new HashSet<Vector2>());

                    // infrared(direction, length, laserEndRotation, ref i, ref curPosition, ref hit);
                    // bool pointVisited = false;
                    // if (pointVisited)
                    // {
                    //     break;
                    // }
                    //     // If the point has already been visited, break to avoid infinite loop
                     
                    //     visitedPoints.Add(hit.point);

                    //     Debug.Log(hit.point);
                    //     Debug.Log(visitedPoints);
                    //     RaycastHit2D ihit = hit;
                    //     Vector2 idirection = direction;



                    //     curPosition = hit.point;
                    //     linerenderer.positionCount++;
                    //     linerenderer.SetPosition(++i, curPosition);
                    //     Vector2 normal = hit.normal;
                    //     direction = Vector2.Reflect(direction.normalized, normal);

                    //     hit = Physics2D.Raycast(curPosition + 0.01f * direction, direction, 1000f, layerMask);
                    //     Debug.DrawLine(curPosition, curPosition + direction * 100f, Color.red);
                    //     if (Vector2.Distance(curPosition, hit.point) > minDistanceThreshold)
                    //     {
                    //         infrared(direction, length, laserEndRotation, ref i, ref curPosition, ref hit);
                    //         LightCheck(ref direction, ref length, ref laserEndRotation, ref i, ref curPosition, ref hit, visitedPoints);
                    //     }


                    //     // Calculate reflection direction
                    //     curPosition = ihit.point;
                    //     linerenderer.positionCount++;
                    //     linerenderer.SetPosition(++i, curPosition);


                    //     // Calculate refraction direction using Snell's Law
                    //     float angle = Vector2.Angle(idirection, -ihit.normal);
                    //     float refractedAngle = Mathf.Asin(Mathf.Sin(angle * Mathf.Deg2Rad) * 1.0f / 1.8f) * Mathf.Rad2Deg;
                    //     Vector2 refractedDirection = Quaternion.Euler(0, 0, refractedAngle) * idirection.normalized;
                    //     hit = Physics2D.Raycast(curPosition + 0.01f * refractedDirection, refractedDirection, 1000f, layerMask);
                    //     Debug.DrawLine(curPosition, curPosition + refractedDirection * 100f, Color.red);
                    //     direction = refractedDirection;
                    //     if (Vector2.Distance(curPosition, hit.point) > minDistanceThreshold)
                    //     {
                    //         Debug.Log("R");
                    //         Reinfrared(direction, length, laserEndRotation, ref j, ref curPosition, ref hit);
                    //         //LightCheck(ref direction, ref length, ref laserEndRotation, ref i, ref curPosition, ref hit);
                    //     }
                }
                    
    




                else
                {

                    // endVFX.SetActive(true);
                    length = (hit.point - curPosition).magnitude;
                    laserEndRotation = hit.transform.rotation.eulerAngles.z;
                    endPosition = curPosition + direction * length;
                    startVFX.transform.position = curPosition;
                    endVFX.transform.position = endPosition;
                    endVFX.transform.rotation = Quaternion.Euler(0, 0, laserEndRotation);
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, endVFX.transform.position);
                    // break;
                    // if (hit.transform.gameObject.tag == "Reciever")
                    // {
                    //     // Logic for the reciever(win the game, show hidden object, etc.)
                    //     Destroy(hit.transform.gameObject);
                    // }
                    // if (hit.transform.gameObject.tag == "Mirror") {
                    //     // Logic for the mirror(win the game, show hidden object, etc.)
                    //     hit.transform.gameObject.GetComponentInParent<HittedAndLight>().Hitted();
                    //     // Debug.Log("Hit the mirror");
                    // }
                    if (Vector2.Distance(curPosition, hit.point) > minDistanceThreshold)
                        infrared(direction, length, laserEndRotation, ref i, ref curPosition, ref hit);
                }
            }
            endPosition = curPosition + direction * length;
            linerenderer.positionCount++;
            linerenderer.SetPosition(++i, endPosition);
            return;
        }
    }

    private void RotateLauncher()
    {
        if (RotationPeriod == 0) return;
        time += Time.deltaTime;
        float angle = RotationAngle * Mathf.Sin(2 * Mathf.PI * time / RotationPeriod);
        transform.rotation = Quaternion.Euler(0, 0, InitAngle + angle);
    }

    //    private void reflectLaser(Vector2 StartPosition, Vector2 direction, RaycastHit2D Firsthit)
    //    {
    //        // linerenderer.SetPosition(1, new Vector3(1, 0, 0));
    //        endVFX.SetActive(false);
    //        Vector2 curPosition = Firsthit.transform.position;
    //        Vector2 normal = Firsthit.normal;
    //        direction = Vector2.Reflect(direction.normalized, normal);
    //        // Define the number of reflections
    //        int i = 1;
    //        // linerenderer.positionCount = 2;
    //        // linerenderer.SetPosition(i, curPosition);
    //        RaycastHit2D hit = Physics2D.Raycast(curPosition + direction * 0.01f, direction, 1000f, layerMask);
    //        while (hit.collider != null && i < 100) {

    //            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Reflectable")) {
    //                break;
    //            }

    //            linerenderer.positionCount++;
    //            curPosition = hit.point;
    //            linerenderer.SetPosition(++i, curPosition);
    //            // i++;

    //            // Reflect the laser

    //            // curPosition += direction.normalized * 0.01f;
    //            hit = Physics2D.Raycast(curPosition + direction * 0.01f, direction.normalized, 100f, layerMask); 

    //        }
    //        // linerenderer.positionCount++;
    //        // linerenderer.SetPosition(++i, curPosition);



    //    }
}