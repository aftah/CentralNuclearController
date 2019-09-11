using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideBar : MonoBehaviour
{
    public Slider pressionBar;
    // Start is called before the first frame update

    private void Awake()
    {
        FindObjectOfType<InputPortSerie>().onSlideEvent += SlideBar_onSlideEvent; 
    }

    public void SlideBar_onSlideEvent(object sender, OnSlideEventArgs e)
    {
        pressionBar.value = (float)e.ValueSlide;
        //Debug.Log("Potentiometre = " + e.ValueSlide);
    }

    void Start()
    {
        pressionBar = GetComponent<Slider>();
    }

   
}
