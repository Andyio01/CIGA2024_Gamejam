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
    
    public float length;
    private LineRenderer linerenderer;
    private float time;
    private float InitAngle;
    private Vector2 StartPosition;
    public LayerMask layerMask;
    public ParticleSystem EmissionPoint;
    [SerializeField] private GameObject startVFX;
    [SerializeField] private GameObject endVFX;
    // Control the rotation of the Launcher
    [SerializeField] private bool Rotation;
    [SerializeField] private float RotationPeriod;
    [SerializeField] private float RotationAngle;
    [SerializeField] private Color color;
    [SerializeField] private float colorIntensity = 4.3f;
    [SerializeField] private float thickness = 9;
    [SerializeField] private float noiseScale = 1f;
    [SerializeField] private float enhance = 3f;


    // Start is called before the first frame update
    
    void Start()
    {
        layerMask  |= LayerMask.GetMask("IgnoreLaser");
        layerMask  |= LayerMask.GetMask("Player");
        layerMask = ~layerMask;
        linerenderer = GetComponentInChildren<LineRenderer>();
        linerenderer.enabled = true;
        linerenderer.material.color = color * colorIntensity;
        linerenderer.material.SetFloat("_Scale", noiseScale);
        linerenderer.material.SetFloat("_Thickness", thickness);
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles) {
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
        UpdateStartPosition();
        UpdateEndPosition();
        if (Rotation) RotateLauncher();
        
    }

    private void UpdateStartPosition()
    {
        linerenderer.SetPosition(0, EmissionPoint.transform.position);
        
    }
    private void UpdateEndPosition()
    {
        float rotationZ =  transform.rotation.eulerAngles.z; //degree
        rotationZ *= Mathf.Deg2Rad; //radian

        Vector2 direction = new Vector2(math.cos(rotationZ), math.sin(rotationZ));


        length = 100f;
        float laserEndRotation = 180;
        Vector2 endPosition;
        linerenderer.positionCount = 1;
        int i = 0;
        Vector2 curPosition = StartPosition;
        RaycastHit2D hit = Physics2D.Raycast(curPosition, direction.normalized, length, layerMask);

        while (hit.collider != null && i < 100) {
            // Hit the player
            // if(hit.transform.gameObject.name.Equals("Player") && deadly){
            //         GameManager.DeathNum[SceneManager.GetActiveScene().name] += 1;
            //         PlayerController.justDied = true;
            //         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //         break;
            //     }
            // Hit the point
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Point") ) {
                hit.transform.gameObject.GetComponent<PointController>().hitByLaser(linerenderer);
            }
            // Hit the object that is not reflectable
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Reflectable")) {
                
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
                if (hit.transform.gameObject.tag == "Reciever") {
                    // Logic for the reciever(win the game, show hidden object, etc.)
                    Destroy(hit.transform.gameObject);
                }
                // if (hit.transform.gameObject.tag == "Mirror") {
                //     // Logic for the mirror(win the game, show hidden object, etc.)
                //     hit.transform.gameObject.GetComponentInParent<HittedAndLight>().Hitted();
                //     // Debug.Log("Hit the mirror");
                // }
            }
            // Hit the object that is reflectable
            else {
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
                
                hit = Physics2D.Raycast(curPosition + 0.01f*direction, direction, 1000f, layerMask);
                Debug.DrawLine(curPosition, curPosition + direction * 100f, Color.red);
            }
        }
        
        // linerenderer.SetPosition(1, new Vector3(length, 0, 0));
        //  endVFX.SetActive(true); 
        curPosition = curPosition + direction * length;
        startVFX.transform.position = StartPosition;
        endVFX.transform.position = curPosition;
        endVFX.transform.rotation = Quaternion.Euler(0, 0, laserEndRotation);
        linerenderer.positionCount++;
        linerenderer.SetPosition(++i, curPosition);

        
    }

    private void RotateLauncher()
    {
        if(RotationPeriod == 0) return;
        time += Time.deltaTime; 
        float angle = RotationAngle * Mathf.Sin(2 * Mathf.PI * time / RotationPeriod); 
        transform.rotation = Quaternion.Euler(0, 0, InitAngle + angle);
    }

    private void reflectLaser(Vector2 StartPosition, Vector2 direction, RaycastHit2D Firsthit)
    {
        // linerenderer.SetPosition(1, new Vector3(1, 0, 0));
        endVFX.SetActive(false);
        Vector2 curPosition = Firsthit.transform.position;
        Vector2 normal = Firsthit.normal;
        direction = Vector2.Reflect(direction.normalized, normal);
        // Define the number of reflections
        int i = 1;
        // linerenderer.positionCount = 2;
        // linerenderer.SetPosition(i, curPosition);
        RaycastHit2D hit = Physics2D.Raycast(curPosition + direction * 0.01f, direction, 1000f, layerMask);
        while (hit.collider != null && i < 100) {
            
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Reflectable")) {
                break;
            }
            
            linerenderer.positionCount++;
            curPosition = hit.point;
            linerenderer.SetPosition(++i, curPosition);
            // i++;
            
            // Reflect the laser
            
            // curPosition += direction.normalized * 0.01f;
            hit = Physics2D.Raycast(curPosition + direction * 0.01f, direction.normalized, 100f, layerMask); 
            
        }
        // linerenderer.positionCount++;
        // linerenderer.SetPosition(++i, curPosition);
       
        

    }

    public void SetLength(float length)
    {
        this.length = length;
    }
}
