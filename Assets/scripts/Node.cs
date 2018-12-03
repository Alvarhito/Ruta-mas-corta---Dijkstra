using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
	public int Id=0;

	public struct forNodes{
		public GameObject node;
		public GameObject line;
		public float distance;
		
	}
	public List<Node.forNodes> exitNodes = new List<Node.forNodes> ();
	public List<GameObject> ob=new List<GameObject>();
	GameObject driver;


	//2.261056 -> 299.12m
	public float relativaA = 2.261056f;
	public float relativaB = 299.12f;

	//public GameObject myText;

	// Use this for initialization
	void Start () {
		
		driver = GameObject.Find ("Driver");
		//Vector2 pos = new Vector2 (transform.position.x, transform.position.y+0.12f);
		//myText=Instantiate (driver.GetComponent<Driver> ().text, pos, Quaternion.identity);
		GetComponent<SpriteRenderer> ().color = Color.cyan;
		//myText.GetComponent<TextMesh>().text = name;
		//myText.GetComponent<SpriteRenderer> ().sortingOrder = 3;
		//myText.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseEnter(){
		if (GetComponent<SpriteRenderer> ().color != Color.green && GetComponent<SpriteRenderer> ().color != Color.yellow) {
			over ();
		}
	}
	void OnMouseDown(){
		//if (GetComponent<SpriteRenderer> ().color != Color.green)
			pressed ();
		//else
		//	normal ();
	}
	void OnMouseExit(){
		if (GetComponent<SpriteRenderer> ().color != Color.yellow && GetComponent<SpriteRenderer> ().color != Color.green){
			normal ();
		}
	}
	public void pressed(){
		GetComponent<SpriteRenderer> ().color = Color.green;
		driver.GetComponent<Driver> ().distance (transform.position,gameObject);
	}
	public void repeatPressed(){
		GetComponent<SpriteRenderer> ().color = Color.green;
		GetComponent<SpriteRenderer> ().sortingOrder = -1;
	}
	public void afterPressed(){
		GetComponent<SpriteRenderer> ().color = Color.yellow;
		GetComponent<SpriteRenderer> ().sortingOrder = -1;
		//driver.GetComponent<Driver> ().distance (transform.position,gameObject);
	}
	public void over(){
		GetComponent<SpriteRenderer> ().color = Color.red;
		//myText.SetActive (true);
		GetComponent<SpriteRenderer> ().sortingOrder = -1;
	}
	public void normal(){
		GetComponent<SpriteRenderer> ().color = Color.cyan;
		//myText.SetActive (false);
		GetComponent<SpriteRenderer> ().sortingOrder = -3;
	}
	float getDistance(Vector2 a, Vector2 b){
		float aux = (a - b).magnitude;
		float approximateReal = (aux * relativaB) / relativaA;
		return approximateReal;
	}

	public GameObject addExit(GameObject other){
		float distance = getDistance (other.transform.position, transform.position);
		var o = Instantiate (driver.GetComponent<Driver> ().line, transform.position, Quaternion.identity);
		o.transform.SetParent (transform);
		Vector3 pos1 = new Vector3 (other.transform.position.x,other.transform.position.y,0);
		Vector3 pos2 = new Vector3 (transform.position.x,transform.position.y,0);
		Vector3[] vec = {pos1,pos2};
		o.GetComponent<LineRenderer> ().SetPositions (vec);
		forNodes aux=new forNodes();
		ob.Add (other);
		aux.node = other;
		aux.line = o;
		aux.distance = distance;
		exitNodes.Add (aux);
		return o;
	}
}
