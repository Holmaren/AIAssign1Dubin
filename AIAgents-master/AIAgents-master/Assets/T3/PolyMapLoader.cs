using UnityEngine;
using System.Collections;
using System;

public class PolyMapLoader {

	private string prefix = Application.dataPath+"/Data/Polygonal/";
	private string postfix = ".txt";

	public PolyData polyData;

	public PolyMapLoader(string x, string y, string goal, string start, string buttons) {
		polyData = new PolyData();
		string xFile = prefix + x + postfix;
		string yFile = prefix + y + postfix;
		string goalFile = prefix + goal + postfix;
		string startFile = prefix + start + postfix;
		string buttonsFile = prefix + buttons + postfix;

		System.IO.StreamReader xReader = new System.IO.StreamReader (xFile);
		System.IO.StreamReader yReader = new System.IO.StreamReader (yFile);

		string xpos, ypos;
		while ((xpos = xReader.ReadLine ()) != null) {
			ypos = yReader.ReadLine (); // xFile and yFile matches each other

			xpos = xpos.Replace (",", ".");
			ypos = ypos.Replace (",", ".");

			polyData.nodes.Add (new Vector3(float.Parse(xpos), 1f, float.Parse (ypos)));
		}

		xReader.Close ();
		yReader.Close ();

		System.IO.StreamReader startReader = new System.IO.StreamReader (startFile);
		string line;
		line = startReader.ReadLine ();
		string[] startPos = line.Split (' ');
		polyData.start = new Vector3 (Mathf.Round(float.Parse (startPos [0])), 1f, float.Parse(startPos [1]));
		startReader.Close ();

		System.IO.StreamReader endReader = new System.IO.StreamReader (goalFile);
		line = endReader.ReadLine ();
		string[] endPos = line.Split (' ');
		polyData.end = new Vector3 (Mathf.Round(float.Parse(endPos [0])), 1f, float.Parse(endPos [1]));
		endReader.Close ();

		System.IO.StreamReader buttonReader = new System.IO.StreamReader (buttonsFile);
		string button;
		while ((button = buttonReader.ReadLine ()) != null) {
			polyData.buttons.Add (Convert.ToInt32(button));
		}
		buttonReader.Close ();
	}


}
