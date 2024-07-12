using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChangePointer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BlockerPrefab;
    public GameObject BlockerPreview;
    public GameObject DiffractionPrefab;
    public GameObject DiffractionPreview;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeToBlocker()
    {
        Debug.Log("Change to Blocker");
        LineManager.ChangeCurrentPointer(BlockerPrefab, BlockerPreview);
    }

    public void ChangeToDiffraction()
    {
        Debug.Log("Change to Diffraction");
        LineManager.ChangeCurrentPointer(DiffractionPrefab, DiffractionPreview);
    }
}
