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

    public GameObject curHitted;
    public GameObject preHitted;
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
    public float bias = 0.2f;
    float minDistanceThreshold = 3f; // Adjust as needed
    public float refractiveIndex1 = 1.0f; // 空气的折射率
    public float refractiveIndex2 = 1.5f; // 目标介质的折射率
    public int maxbounce = 0; // 最大反射/折射次数

    // Start is called before the first frame update
    public GameObject ParentObject;
    void Start()
    {
        if(transform.parent) ParentObject = transform.parent.gameObject;
        layerMask |= LayerMask.GetMask("IgnoreLaser");
        layerMask |= LayerMask.GetMask("Player");
        layerMask = ~layerMask;
        linerenderer = GetComponentInChildren<LineRenderer>();
        linerenderer.enabled = true;
        //linerenderer.material.color = color * colorIntensity;
        //linerenderer.material.SetFloat("_Scale", noiseScale);
        //linerenderer.material.SetFloat("_Thickness", thickness);
        //Relinerenderer = GetComponentInChildren<LineRenderer>();
        ////Relinerenderer.enabled = true;
        //Relinerenderer.material.color = color * colorIntensity;
        //Relinerenderer.material.SetFloat("_Scale", noiseScale);
        //Relinerenderer.material.SetFloat("_Thickness", thickness);
        //ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        //foreach (ParticleSystem particle in particles)
        //{
        //    Renderer rend = particle.GetComponent<Renderer>();
        //    // rend.material.SetColor("_BaseColor", color * colorIntensity);
        //    rend.material.SetColor("_EmissionColor", color * (colorIntensity + enhance));
        //}
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
        //if (Rotation) RotateLauncher();

    }

    private void UpdateStartPosition(Vector2 direction)
    {
        EmissionPoint.transform.position = (Vector2)transform.position + direction * bias;
        linerenderer.SetPosition(0, EmissionPoint.transform.position);
        // Relinerenderer.SetPosition(0, EmissionPoint.transform.position);

    }
    private void UpdateEndPosition(Vector2 direction)
    {
        
        if(transform.GetComponent<PointController>() && transform.GetComponent<PointController>().isHit == false) return;
        if(ParentObject != null)
            if(ParentObject.GetComponent<ReflectionController>() && ParentObject.GetComponent<ReflectionController>().isHit == false) 
                return;

        
        float length = 100f;
        // float laserEndRotation = 180;
        Vector2 endPosition;
        linerenderer.positionCount = 1;
        //int i = 0;
        //int j = 0;
        Vector2 curPosition = EmissionPoint.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(curPosition, direction.normalized, length, layerMask);
        Debug.DrawLine(curPosition, curPosition + direction * 100f, Color.red);




        // TraceLight(curPosition, direction, i, ref hit);
        LightCheck(direction, length, maxbounce, hit, new HashSet<Vector2>());

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

        void TraceLight(Vector2 origin, Vector2 direction, int currentBounces, ref RaycastHit2D hit)
        {
            //if (currentBounces > maxBounces) return;

            // RaycastHit2D hit = Physics2D.Raycast(origin, direction, layerMask);
                
            Vector2 hitPoint = hit.point;
            linerenderer.positionCount += 2;
            linerenderer.SetPosition(linerenderer.positionCount - 2, hitPoint);
            Vector2 normal = hit.normal;
            if (hit.collider != null){
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Point") )
                    {
                        // Debug.Log("打到了点上");
                        hit.transform.gameObject.GetComponent<PointController>().hitByLaser(linerenderer);
                    }

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Reflectable") )
                    {
                        Vector2 reflectedDirection = Vector2.Reflect(direction, normal);
                        linerenderer.SetPosition(linerenderer.positionCount - 1, hitPoint + reflectedDirection * length);
                        Debug.DrawLine(origin, origin + reflectedDirection * 100f, Color.red);

                        TraceLight(hitPoint, reflectedDirection, currentBounces + 1, ref hit);
                    }

                else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Refractable"))
                    {
                        // 计算反射
                        Vector2 reflectedDirection = Vector2.Reflect(direction, normal);
                        linerenderer.SetPosition(linerenderer.positionCount - 1, hitPoint + reflectedDirection * length);
                        Debug.DrawLine(origin, origin + reflectedDirection * 100f, Color.red);

                        // 计算折射
                        float n1 = refractiveIndex1;
                        float n2 = refractiveIndex2;
                        float cosI = -Vector3.Dot(normal, direction);
                        float sinT2 = (n1 / n2) * (n1 / n2) * (1.0f - cosI * cosI);
                    if (sinT2 <= 1.0f)
                        {
                            float cosT = Mathf.Sqrt(1.0f - sinT2);
                            Vector2 refractedDirection = (n1 / n2) * direction + (n1 / n2 * cosI - cosT) * normal;
                            linerenderer.positionCount += 1;
                            linerenderer.SetPosition(linerenderer.positionCount - 1, hitPoint + refractedDirection * length);
                            Debug.DrawLine(origin, origin + refractedDirection * 100f, Color.blue);


                            // 递归调用处理折射光线
                            TraceLight(hitPoint, refractedDirection, currentBounces + 1, ref hit);
                        }

                        // 递归调用处理反射光线
                        TraceLight(hitPoint, reflectedDirection, currentBounces + 1, ref hit);

                    }
            }
            }
        

        void LightCheck(Vector2 direction,  float length, int i, RaycastHit2D hit, HashSet<Vector2> visitedPoints)
        {

            //// 超过最大反弹次数
            //if (i >= 1)
            //{ 
            //    Debug.Log("Out of Bound !");
            //    return;
            //} 

            if (hit.collider != null && i < 1 && GetComponentInChildren<LineRenderer>().enabled != false)
            {
                // Hit the player
                // if(hit.transform.gameObject.name.Equals("Player") && deadly){
                //         GameManager.DeathNum[SceneManager.GetActiveScene().name] += 1;
                //         PlayerController.justDied = true;
                //         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //         break;
                //     }
                // Hit the point
                if(curHitted && curHitted != hit.transform.gameObject)
                {
                    
                    if (curHitted.GetComponent<ReflectionController>())
                    {
                            curHitted.GetComponent<ReflectionController>().unHitted();
                            Debug.Log("Close the light");
                    }
                    else if (curHitted.GetComponent<PointController>())
                    {
                            curHitted.GetComponent<PointController>().unHitted();
                    }
                    else if (curHitted.GetComponent<ButtonController>())
                    {
                        curHitted.GetComponent<ButtonController>().unHitted();
                    }
                }
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Point"))
                {
                    curHitted = hit.transform.gameObject;
                    length = (hit.point - curPosition).magnitude;
                    endPosition = curPosition + direction * length;
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, endPosition);
                    // Debug.Log("打到了点上");
                    hit.transform.gameObject.GetComponent<PointController>().hitByLaser(linerenderer);
                }
                else if (hit.transform.gameObject.tag == "Reciever") {
                    curHitted = hit.transform.gameObject;
                    // 阻断当前射线
                    length = (hit.point - curPosition).magnitude;
                    endPosition = curPosition + direction * length;
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, endPosition);
                    hit.transform.gameObject.GetComponent<RecieverController>().hitByLaser();
                }
                else if (hit.transform.gameObject.tag == "Plane") {
                    curHitted = hit.transform.gameObject;
                    // 阻断当前射线
                    length = (hit.point - curPosition).magnitude;
                    endPosition = curPosition + direction * length;
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, endPosition);

                    Vector2 refractedDirection = new Vector2();
                    // 计算反射
                    Vector2 normal = hit.normal;
                    Vector2 reflectDirection = Vector2.Reflect(direction, normal);
                    // 计算折射
                    float n1 = refractiveIndex1;
                    float n2 = refractiveIndex2;

                    float cosI = -Vector3.Dot(normal, direction.normalized);
                    float sinT2 = (n1 / n2) * (n1 / n2) * (1.0f - cosI * cosI);
                    if (sinT2 <= 1.0f)
                    {
                        float cosT = Mathf.Sqrt(1.0f - sinT2);
                        refractedDirection = (n1 / n2) * direction + (n1 / n2 * cosI - cosT) * normal;
                    }
                    // Logic for the reciever(win the game, show hidden object, etc.)

                    // 传入折射和反射角度并创建新的光线
                    hit.transform.gameObject.GetComponent<ReflectionController>().hitByLaser(refractedDirection, reflectDirection, hit.point, normal);
                    //Debug.Log("Direction:" + direction + "normal:" + normal + "Reflection:" + reflectDirection + "Dot: " + Vector2.Dot(normal, direction));
                }
                else if (hit.transform.gameObject.tag == "Trap")
                {
                    curHitted = hit.transform.gameObject;
                    // 阻断当前射线
                    length = (hit.point - curPosition).magnitude;
                    endPosition = curPosition + direction * length;
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, endPosition);
                    hit.transform.gameObject.GetComponent<TrapController>().hitByLaser();
                }
                // 被机关触发所开启的激光，不能二次触发机关
                else if (hit.transform.gameObject.tag == "Button" && hit.transform.GetComponent<ButtonController>().triggerAble == true)
                {
                    curHitted = hit.transform.gameObject;
                    // 阻断当前射线
                    length = (hit.point - curPosition).magnitude;
                    endPosition = curPosition + direction * length;
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, endPosition);
                    hit.transform.gameObject.GetComponent<ButtonController>().hitByLaser();
                    if (hit.transform.gameObject.GetComponent<RecieverController>()) hit.transform.gameObject.GetComponent<RecieverController>().hitByLaser();
                }
                // Hit the object that is not reflectable

                // Hit the object that is reflectable
                // else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Reflectable"))
                // {
                //     // reflectLaser(StartPosition, direction, hit);
                //     // endVFX.SetActive(false);
                //     // if (hit.transform.gameObject.tag == "Mirror") {
                //     //     // Logic for the mirror(win the game, show hidden object, etc.)
                //     //     hit.transform.gameObject.GetComponentInParent<HittedAndLight>().Hitted();
                //     //     // Debug.Log("Hit the mirror");
                //     // }
                //     Vector2 curPosition = hit.point;
                //     linerenderer.positionCount++;
                //     linerenderer.SetPosition(++i, curPosition);
                //     Debug.Log("加点位置1");
                //     Vector2 normal = hit.normal;
                //     direction = Vector2.Reflect(direction.normalized, normal);

                //     hit = Physics2D.Raycast(curPosition + 0.01f * direction, direction, 1000f, layerMask);
                //     Debug.DrawLine(curPosition, curPosition + direction * 100f, Color.red);
                //     // infrared(direction, length, laserEndRotation, ref i, ref curPosition, ref hit);

                // }
                // else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Refractable"))
                // {

                //     // 计算反射
                //     Vector2 curPosition = hit.point;
                //     linerenderer.positionCount++;
                //     linerenderer.SetPosition(++i, curPosition);
                //     Debug.Log("加点位置2");

                //     Vector2 normal = hit.normal;
                //     Vector2 reflectDirection = Vector2.Reflect(direction.normalized, normal);

                //     hit = Physics2D.Raycast(curPosition + 0.01f * reflectDirection, reflectDirection, 1000f, layerMask);
                //     Debug.DrawLine(curPosition, curPosition + reflectDirection * 100f, Color.red);
                //     LightCheck(reflectDirection, length,  i, hit, new HashSet<Vector2>());

                //     // 计算折射
                //     linerenderer.positionCount++;
                //     linerenderer.SetPosition(++i, curPosition); //新建折射起点
                //     Debug.Log("加点位置3");


                //     float n1 = refractiveIndex1;
                //     float n2 = refractiveIndex2;

                //     float cosI = -Vector3.Dot(normal, direction);
                //     float sinT2 = (n1 / n2) * (n1 / n2) * (1.0f - cosI * cosI);
                //     if (sinT2 <= 1.0f)
                //     {
                //         float cosT = Mathf.Sqrt(1.0f - sinT2);
                //         Vector2 refractedDirection = (n1 / n2) * direction + (n1 / n2 * cosI - cosT) * normal;
                //         hit = Physics2D.Raycast(curPosition + 0.01f * refractedDirection, refractedDirection, 1000f, layerMask);
                //         Debug.DrawLine(curPosition, curPosition + refractedDirection * 100f, Color.blue);
                //         LightCheck(refractedDirection, length,  i, hit, new HashSet<Vector2>());
                //         iterEnd = true;
                //     }

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
                // }





                else
                {
                    
                    // endVFX.SetActive(true);
                    curHitted = hit.transform.gameObject;

                    length = (hit.point - curPosition).magnitude;
                   
                    endPosition = curPosition + direction * length;
                    // startVFX.transform.position = curPosition;
                    // endVFX.transform.position = endPosition;
                    // endVFX.transform.rotation = Quaternion.Euler(0, 0, laserEndRotation);
                    linerenderer.positionCount++;
                    linerenderer.SetPosition(++i, endPosition);
                    // Debug.Log("加点位置4");

                    return;
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
                    // if (Vector2.Distance(curPosition, hit.point) > minDistanceThreshold)
                        // infrared(direction, length, laserEndRotation, ref i, ref curPosition, ref hit);
                }
            }
            
                // 未击中任何物体或当前光线处于关闭状态的处理逻辑
            else
            {
                // if(curHitted)
                //     {
                //         if(curHitted.GetComponent<ReflectionController>())
                //         {
                //             curHitted.GetComponent<ReflectionController>().unHitted();
                //         }
                //         else if(curHitted.GetComponent<PointController>())
                //         {
                //             curHitted.GetComponent<PointController>().unHitted();
                //         }
                //         curHitted = null;
                //     }
                maxbounce = 0;
                endPosition = curPosition + direction * length;
                linerenderer.positionCount++;
                linerenderer.SetPosition(++i, endPosition);
                // Debug.Log("加点位置5");
                //maxbounce = 0;
                //Debug.Log(this.gameObject.name + maxbounce);
                return;
            }
        }
    }

    private void OnDisable()
    {
        if (curHitted)
        {

            if (curHitted.GetComponent<ReflectionController>())
            {
                curHitted.GetComponent<ReflectionController>().unHitted();
                Debug.Log("Close the light");
            }
            else if (curHitted.GetComponent<PointController>())
            {
                curHitted.GetComponent<PointController>().unHitted();
            }
            else if (curHitted.GetComponent<ButtonController>())
            {
                curHitted.GetComponent<ButtonController>().unHitted();
            }
        }
        curHitted = null;
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