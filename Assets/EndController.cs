using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndController : MonoBehaviour
{
    public GameObject EndText;
    public GameObject EndButton;
    // Start is called before the first frame update
    void Start()
    {
        EndText.GetComponent<Text>().DOFade(1, 2f).onComplete += () => {
            EndButton.GetComponent<Image>().DOFade(1, 1f);
            EndButton.GetComponent<Button>().interactable = true;
        };
        // EndButton.GetComponent<Image>().DOFade(1, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
