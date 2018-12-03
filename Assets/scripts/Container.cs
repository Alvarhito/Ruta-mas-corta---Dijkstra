using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Container : MonoBehaviour {

	public GameObject contain;
	//public Transform pivot;
	List<GameObject> routes = new List<GameObject> ();

	public GameObject info;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	float truncate(float value, int digits){
		double mult = Math.Pow(10.0, digits);
		double result = Math.Truncate( mult * value ) / mult;
		return (float) result;
	}
	public void addRoutes(List<string> route,List<Node.forNodes> nodes){
		//route.Reverse ();
		//distance.Reverse ();
		deleteRoutes ();
		float offset = (Camera.main.orthographicSize * 0.63f) / 5;
		float addE = (Camera.main.orthographicSize * 0.2f) / 5;
		contain.GetComponent<RectTransform> ().sizeDelta = new Vector2 (contain.GetComponent<RectTransform> ().sizeDelta.x, 46f * route.Count);

		for(int i=0;i<route.Count;i++){
			Vector3 pos = new Vector3 (transform.position.x - ((Camera.main.orthographicSize * 0.8f) / 5), transform.position.y - addE, -9f);
			var o = Instantiate (info, pos, Quaternion.identity);

			float size = (Camera.main.orthographicSize * 0.02231602f) / 5;
			o.transform.localScale = new Vector3 (size, size, size);
			o.GetComponent<Route> ().route.text= route [i];
			o.GetComponent<Route> ().distance.text = truncate (nodes [i].distance, 2).ToString () + " Metros";
			o.GetComponent<Route> ().lineRender = nodes [i].line;
			o.GetComponent<Route> ().saveInfo ();
			o.transform.SetParent (contain.transform);
			routes.Add (o);
			addE += offset;
		}
	}
	public void deleteRoutes(){
		for (int i = 0; i < routes.Count; i++) {
			Destroy (routes [i]);
		}
		routes = new List<GameObject> ();
		contain.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0,0);
		contain.GetComponent<RectTransform> ().localPosition = Vector2.zero;
	}
}
