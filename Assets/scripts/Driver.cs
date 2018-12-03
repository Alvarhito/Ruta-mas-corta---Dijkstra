using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Driver : MonoBehaviour{
	string path="/Documents/AdjacencyMatrix.csv";
	//string path2="/Documents/DistanceMatrix.csv";
	public float speed=1f;
	public float speedTurn=1;
	public float max=5f;
	public float min = 2.22f;
	public float time=0.6f;

	public float offsetSquare = 1;

	Camera camara;

	//public GameObject turn;
	public GameObject node;
	public GameObject content;
	public GameObject text;
	public GameObject line;

	public GameObject routeText;
	public GameObject square;
	public GameObject square2;
	public GameObject onlyRoutes;
	public GameObject showRoutes;
	public GameObject noRoutes;

	public GameObject forContainer;
	       bool  showingMap=true;
	public GameObject map;

	//public TextAsset csv;

	public int idCounter = 0;

	public bool AddNodeMode = false;
	public bool save = false;
	public bool load = true;
	public bool route = true;
	       bool load2 = true;

	Color color;
	GameObject o;

	public List<GameObject> nodes;
	public GameObject[] generalNodes;

	List<GameObject> linesShowed = new List<GameObject> ();

	List<Node.forNodes> lines = new List<Node.forNodes> ();

	//Vector2    firstPosition;
	GameObject firstObjet;
	GameObject lastObjet;
	int selected=0;

	Dijkstras.Graph graph = new Dijkstras.Graph ();

	float posInit;

	void Start () {
		onlyRoutes.GetComponent<Button> ().onClick.AddListener (showAllRoutes);
		showRoutes.GetComponent<Button> ().onClick.AddListener (showDetails);
		posInit = showRoutes.transform.localPosition.x;
		noRoutes.GetComponent<Button> ().onClick.AddListener (hideNoRoutes);
		
		/*string texto = "python \"C:\\Users\\ALVARO ORTEGA\\Documents\\Unity\\Cartagena\\Assets\\scripts\\dijkstra.py\"";
		Debug.Log (texto);
		var hola = System.Diagnostics.Process.Start (texto);
		Debug.Log (hola.ExitCode);
		hola.Close ();*/

		camara = Camera.main;
		//Debug.Log (csv.text);
		generalNodes = GameObject.FindGameObjectsWithTag ("Nodes");
		order ();
		idCounter = generalNodes.Length;
		//turn.GetComponent<Button> ().onClick.AddListener (StartCoroutine (turnCamera ()));
	}
	
	// Update is called once per frame
	void Update () {
		wheel ();
		if (save && !load2)
			saver ();
		if (load && load2) {
			loader ();
			minDistance ();
			routeText.GetComponent<Text> ().text = "Rutas cargadas";
			Invoke ("hideText", 2f);

			//graph.shortest_path(0, 7).ForEach( x => Console.Write(x) );
		}
	}
	void hideNoRoutes(){
		noRoutes.SetActive (false);
	}
	void showDetails(){
		float auxi = 130;
		if (forContainer.activeSelf) {
			showRoutes.GetComponentInChildren<Text> ().text = "Mostrar detalles";
			showRoutes.transform.localPosition = new Vector3 (posInit, showRoutes.transform.localPosition.y, showRoutes.transform.localPosition.z);
		} else {
			showRoutes.GetComponentInChildren<Text> ().text = "Oculatr detalles";
			showRoutes.transform.localPosition= new Vector3 (posInit - auxi, showRoutes.transform.localPosition.y, showRoutes.transform.localPosition.z);
		}
		forContainer.SetActive (!forContainer.activeSelf);

	}
	void showAllRoutes(){
		if (showingMap) {
			map.SetActive (false);
			onlyRoutes.GetComponentInChildren<Text> ().text = "Mostrar mapa";
		} else {
			map.SetActive (true);
			onlyRoutes.GetComponentInChildren<Text> ().text = "Mostrar solo rutas";
		}
		showingMap = !showingMap;

	}
	void hideText(){
		routeText.SetActive (false);
	}
	float go(int init,int end){
		List<int> list = graph.shortest_path (init, end);
		if (list != null) {
			return showLines (list);
		} else {
			Debug.Log ("No hay ruta " + init + " " + end);
			noRoutes.GetComponentInChildren<Text> ().text = "No existe una ruta \n" + init + " - " + end;
			noRoutes.SetActive (true);
			return 0;
		}
		
	}

	float showWithID(List<Node.forNodes> nodes,int id){
		for (int i = 0; i < nodes.Count; i++) {
			if (nodes [i].node.GetComponent<Node> ().Id == id) {
				//nodes [i].node.GetComponent<Node> ();
				nodes [i].line.GetComponent<LineRenderer> ().sortingOrder = -1;
				nodes [i].line.GetComponent<LineRenderer> ().endColor = color;
				nodes [i].line.GetComponent<LineRenderer> ().startColor = color;
				linesShowed.Add (nodes [i].line);
				lines.Add (nodes [i]);

				return nodes [i].distance;
			}
		}
		Debug.Log ("No hay coincidencia");
		return 0;
	}
	float showLines(List<int> list){
		float totalDistance = 0;
		float auxDistance;

		List<string> aux = new List<string> ();
		if (list.Count > 0) {
			aux.Add (firstObjet.GetComponent<Node> ().Id + "  " + list [list.Count - 1]);
			Debug.Log (firstObjet.GetComponent<Node> ().Id + "  " + list [list.Count - 1]);
			totalDistance = showWithID (firstObjet.GetComponent<Node> ().exitNodes, list [list.Count - 1]);
			//aux1.Add (totalDistance);
		}

		for (int i = list.Count-1; i>0; i--) {
			Debug.Log (list [i]+"  "+list [i - 1]);
			aux.Add (list [i] + "  " + list [i - 1]);
			auxDistance=showWithID (generalNodes [list [i]].GetComponent<Node> ().exitNodes, list [i - 1]);
			//aux1.Add (auxDistance);

			totalDistance += auxDistance;
		}

		forContainer.GetComponent<Container> ().addRoutes (aux, lines);
		lines.Clear ();
		//lines = new List<Node.forNodes> ();
		return totalDistance;
		//for()
	}
	void minDistance(){
		for (int i = 0; i < generalNodes.Length; i++) {
			Dictionary<int, int> dic = new Dictionary<int, int> ();
			for (int j = 0; j < generalNodes[i].GetComponent<Node>().exitNodes.Count; j++) {
				
				int d=(int)(generalNodes [i].GetComponent<Node> ().exitNodes [j].distance);
				int id=generalNodes [i].GetComponent<Node> ().exitNodes [j].node.GetComponent<Node> ().Id;

				dic.Add(id,d);

				// new Dictionary<char, int> () { { 'B', 7 }, { 'C', 8 } }
			}
			graph.add_vertex (i, dic);
		}
	}
	void loader(){
		StreamReader reader = System.IO.File.OpenText(Application.dataPath + path);

		int cont = 0;
		while (!reader.EndOfStream) {
			string[] aux = reader.ReadLine ().Split (',');
			for (int i = 0; i < aux.Length; i++) {
				if (aux [i] == "1") {
					color = generalNodes [cont].GetComponent<Node> ().addExit (generalNodes [i]).GetComponent<LineRenderer> ().endColor;
					//generalNodes [cont].GetComponent<Node> ().afterPressed ();
					//generalNodes [i].GetComponent<Node> ().afterPressed ();
				}
			}
			cont +=1;
		}

		reader.Close ();
		load = false;
		load2 = false;
		Debug.Log ("Cargado");

	}
	void saver(){
		string separador = ",";
		StringBuilder sb = new StringBuilder ();
		string cont = "";
		for (int i = 0; i < generalNodes.Length; i++) {
			cont = "";
			for (int j = 0; j < generalNodes.Length; j++) {
				string aux="0";
				for (int k = 0; k <generalNodes[i].GetComponent<Node>().exitNodes.Count; k++) {
					if (generalNodes[i].GetComponent<Node>().exitNodes[k].node.GetComponent<Node>().Id==j) {
						aux="1";
						break;
					}
				}

				if (j == generalNodes.Length - 1)
					cont += aux;
				else
					cont+=(aux+separador);
			}
			sb.AppendLine (cont);
		}

		StreamWriter writer = System.IO.File.CreateText (Application.dataPath + path);
		writer.WriteLine (sb);
		writer.Close ();
		save = false;
		Debug.Log ("Guardado");
	}

	void MoverCamara(){
		float x = Input.GetAxis ("Mouse X") * speed;
		float y = Input.GetAxis ("Mouse Y") * speed;

		camara.transform.position += new Vector3 (-x, -y);
	}
	void OnMouseDrag(){
		MoverCamara ();
	}
	void OnMouseUp(){
		if (AddNodeMode) {
			
			newNode ();
		}
	}
	void newNode(){
		Vector2 newposition = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
		o = Instantiate (node, newposition, Quaternion.identity);
		o.transform.SetParent (content.transform);
		o.GetComponent<Node> ().Id = idCounter;
		o.name = "Node" + idCounter;
		idCounter += 1;
		nodes.Add (o);
	}
	void wheel(){
		float x = Input.GetAxis ("Mouse ScrollWheel");
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (nodes.Count > 0) {
				GameObject aux = nodes [nodes.Count - 1];
				nodes.Remove (aux);
				Destroy (aux);
				idCounter -= 1;
			}
		}
		//Debug.Log (x);
		if (camara.orthographicSize >= min && camara.orthographicSize <= max) {
			if (!(camara.orthographicSize == max && x < 0) && !(camara.orthographicSize == min && x > 0)) {
				camara.orthographicSize -= x;
				square.transform.localScale -= new Vector3 (x * offsetSquare, x * offsetSquare);
				square2.transform.localScale -= new Vector3 (x * offsetSquare, x * offsetSquare);
				speed -= (x * 0.1f);
			}
		} else if (camara.orthographicSize < min)
			camara.orthographicSize = min;
		else if (camara.orthographicSize > max)
			camara.orthographicSize = max;
		
	}

	void deleteLines(){
		for (int i = 0; i < linesShowed.Count; i++) {
			linesShowed[i].GetComponent<LineRenderer> ().sortingOrder = -3;
		}
		linesShowed = new List<GameObject> ();
	}

	float truncate(float value, int digits){
	    double mult = Math.Pow(10.0, digits);
	    double result = Math.Truncate( mult * value ) / mult;
	    return (float) result;
	}
	public void distance(Vector2 newPos,GameObject o){
		if (selected==0) {
			forContainer.SetActive (false);
			showRoutes.GetComponentInChildren<Text> ().text = "Mostrar detalles";
			showRoutes.SetActive (false);
			showRoutes.transform.localPosition = new Vector3 (posInit, showRoutes.transform.localPosition.y, showRoutes.transform.localPosition.z);
			forContainer.GetComponent<Container> ().deleteRoutes ();

			//firstPosition = newPos;
			if(firstObjet!=null){
				firstObjet.GetComponent<Node>().normal();
				deleteLines ();
				square.SetActive (false);
				square2.SetActive (false);
			}
			if(lastObjet!=null)lastObjet.GetComponent<Node>().normal();

			o.GetComponent<Node> ().repeatPressed ();

			firstObjet = o;
			selected += 1;

		} else {
			lastObjet = o;


			if (!route) {
				putLine ();
				Invoke ("normal", time);
			} else {
				float totalDistance = go (firstObjet.GetComponent<Node> ().Id, lastObjet.GetComponent<Node> ().Id);
				string textToDistance;
				if (totalDistance > 0) 
					showRoutes.SetActive (true);
				if (totalDistance >= 1000)
					textToDistance = truncate((totalDistance / 1000),2).ToString () + " Km";
				else
					textToDistance = truncate(totalDistance,2).ToString () + " M";
				square.GetComponentInChildren<TextMesh> ().text = textToDistance;
				//square.GetComponentInChildren<SpriteRenderer> ().sortingOrder = 10;

				square.transform.position = lastObjet.transform.position;
				square.SetActive (true);

				square2.transform.position = firstObjet.transform.position;
				square2.SetActive (true);
			}

			selected = 0;
		}
	}
	void normal(){
		firstObjet.GetComponent<Node> ().afterPressed ();
		lastObjet.GetComponent<Node> ().afterPressed();
	}
	void putLine(){
		firstObjet.GetComponent<Node> ().addExit (lastObjet);
	}
	void order(){
		for (int i = 0; i < generalNodes.Length; i++) {
			for (int j = 0; j < generalNodes.Length; j++) {
				if (generalNodes [j].GetComponent<Node> ().Id == i) {
					GameObject aux = generalNodes [j];
					generalNodes [j] = generalNodes [i];
					generalNodes [i] = aux;
					break;
				}
			}

		}

	}
		
}
