using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Route : MonoBehaviour {
	public Text route;
	public Text distance;
	public GameObject back;
	public GameObject lineRender;

	Color initColor;
	Color endColor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void saveInfo(){
		initColor = lineRender.GetComponent<LineRenderer> ().startColor;
		endColor = lineRender.GetComponent<LineRenderer> ().endColor;
	}
	void OnMouseExit(){
		exit ();
	}
	void OnMouseOver(){
		over ();
	}
	void OnMouseDown(){
		over ();
	}
	void over(){
		back.SetActive (true);
		//lineRender.GetComponent<LineRenderer> ()
		lineRender.GetComponent<LineRenderer> ().endColor = Color.red;
		lineRender.GetComponent<LineRenderer> ().startColor = Color.red;
	}
	void exit(){
		back.SetActive (false);
		//Debug.Log (Color.blue.);
		lineRender.GetComponent<LineRenderer> ().endColor = endColor;
		lineRender.GetComponent<LineRenderer> ().startColor = initColor;
	}
}
