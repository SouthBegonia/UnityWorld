using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
<xml>
	<jeremy age="36">
		<friend name="Harrison">
			"Hello"
		</friend>
	</jeremy>
</xml>


XMLHashtable xml;
xml["jeremy"][0]["friend"][0].text
xml["jeremy"][0].att("age");
*/
		


[System.Serializable]
public class PT_XMLReader {
	static public bool		SHOW_COMMENTS = false;

	//public string input;
	//public TextAsset inputTA;
	public string xmlText;
	public PT_XMLHashtable xml;
	
	/*
	void Awake() {
		inputTA = Resources.Load("WellFormedSample") as TextAsset;	
		input = inputTA.text;
		print(input);
		output = new XMLHashtable();
		Parse(input, output);
		// TODO: Make something which will trace a Hashtable or output it as XML
		print(output["videocollection"][0]["video"][1]["title"][0].text);
	}
	*/
	
	// This function creates a new XMLHashtable and calls the real Parse()
	public void Parse(string eS) {
		xmlText = eS;
		xml = new PT_XMLHashtable();
		Parse(eS, xml);
	}
	
	// This function will parse a possible series of tags
	void Parse(string eS, PT_XMLHashtable eH) {
		eS = eS.Trim();
		while(eS.Length > 0) {
			eS = ParseTag(eS, eH);
			eS = eS.Trim();
		}
	}
	
	// This function parses a single tag and calls Parse() if it encounters subtags
	string ParseTag(string eS, PT_XMLHashtable eH) {
		// search for "<"
		int ndx = eS.IndexOf("<");
		int end, end1, end2, end3;
		if (ndx == -1) {
			// It's possible that this is just a string (e.g. <someTagTheStringIsInside>string</someTagTheStringIsInside>)
			end3 = eS.IndexOf(">"); // This closes a standard tag; look for the closing tag
			if (end3 == -1) {
				// In that case, we just need to add an @ key/value to the hashtable
				eS = eS.Trim(); // I think this is redundant
				//eH["@"] = eS;
				eH.text = eS;
			}
			return(""); // We're done with this tag
		}
		// Ignore this if it is just an XML header (e.g. <?xml version="1.0"?>)
		if (eS[ndx+1] == '?') {
			// search for the closing tag of this header
			int ndx2 = eS.IndexOf("?>");
			string header = eS.Substring(ndx, ndx2-ndx+2);
			//eH["@XML_Header"] = header;
			eH.header = header;
			return(eS.Substring(ndx2+2));
		}
		// Ignore this if it is an XML comment (e.g. <!-- Comment text -->)
		if (eS[ndx+1] == '!') {
			// search for the closing tag of this header
			int ndx2 = eS.IndexOf("-->");
			string comment = eS.Substring(ndx, ndx2-ndx+3);
			if (SHOW_COMMENTS) Debug.Log("XMl Comment: "+comment);
			//eH["@XML_Header"] = header;
			return(eS.Substring(ndx2+3));
		}
		
		// Find the end of the tag name
										// For the next few comments, this is what happens when this character is the first one found after the beginning of the tag
		end1 = eS.IndexOf(" ", ndx);	// This means that we'll have attributes
		end2 = eS.IndexOf("/", ndx);	// Immediately closes the tag, 
		end3 = eS.IndexOf(">", ndx);	// This closes a standard tag; look for the closing tag
		if (end1 == -1) end1 = int.MaxValue;
		if (end2 == -1) end2 = int.MaxValue;
		if (end3 == -1) end3 = int.MaxValue;
		
		
		end = Mathf.Min(end1, end2, end3);
		string tag = eS.Substring(ndx+1, end-ndx-1);
		
		// search for this tag in eH. If it's not there, make it
		if (!eH.ContainsKey(tag)) {
			eH[tag] = new PT_XMLHashList();
		}
		// Create a hashtable to contain this tag's information
		PT_XMLHashList arrL = eH[tag] as PT_XMLHashList;
		//int thisHashIndex = arrL.Count;
		PT_XMLHashtable thisHash = new PT_XMLHashtable();
		arrL.Add(thisHash);
		
		// Pull the attributes string
		string atts = "";
		if (end1 < end3) {
			try {
				atts = eS.Substring(end1, end3-end1);
			}
			catch(System.Exception ex) {
				Debug.LogException(ex);
				Debug.Log("break");
			}
		}
		// Parse the attributes, which are all guaranteed to be strings
		string att, val;
		int eqNdx, spNdx;
		while (atts.Length > 0) {
			atts = atts.Trim();
			eqNdx = atts.IndexOf("=");
			if (eqNdx == -1) break;
			//att = "@"+atts.Substring(0,eqNdx);
			att = atts.Substring(0,eqNdx);
			spNdx = atts.IndexOf(" ",eqNdx);
			if (spNdx == -1) { // This is the last attribute and doesn't have a space after it
				val = atts.Substring(eqNdx+1);
				if (val[val.Length-1] == '/') { // If the trailing / from /> was caught, remove it
					val = val.Substring(0,val.Length-1);
				}
				atts = "";
			} else { // This attribute has a space after it
				val = atts.Substring(eqNdx+1, spNdx - eqNdx - 2);
				atts = atts.Substring(spNdx);
			}
			val = val.Trim('\"');
			//thisHash[att] = val; // All attributes have to be unique, so this should be okay.
			thisHash.attSet(att, val);
		}
		
		
		// Pull the subs, which is everything contained by this tag but exclusing the tags on either side (e.g. <tag att="hi">.....subs.....</tag>)
		string subs = "";
		string leftoverString = "";
		// singleLine means this doesn't have a separate closing tag (e.g. <tag att="hi" />)
		bool singleLine = (end2 == end3-1);// ? true : false;
		if (!singleLine) { // This is a multiline tag (e.g. <tag> ....  </tag>)
			// find the closing tag
			int close = eS.IndexOf("</"+tag+">");
// TODO: Should this do something more if there is no closing tag?
			if (close == -1) {
				Debug.Log("XMLReader ERROR: XML not well formed. Closing tag </"+tag+"> missing.");
				return("");
			}
			subs = eS.Substring(end3+1, close-end3-1);
			leftoverString = eS.Substring( eS.IndexOf(">",close)+1 );
		} else {
			leftoverString = eS.Substring(end3+1);
		}
		
		subs = subs.Trim();
		// Call Parse if this contains subs
		if (subs.Length > 0) {
			Parse(subs, thisHash);
		}
		
		// Trim and return the leftover string
		leftoverString = leftoverString.Trim();
		return(leftoverString);
	
	}
	
}



public class PT_XMLHashList {
	public ArrayList list = new ArrayList();
	
	public PT_XMLHashtable this[int s] {
		get {
			return(list[s] as PT_XMLHashtable);
		}
		set {
			list[s] = value;
		}
	}
	
	public void Add(PT_XMLHashtable eH) {
		list.Add(eH);
	}
	
	public int Count {
		get {
			return(list.Count);
		}
	}
	
	public int length {
		get {
			return(list.Count);
		}
	}
}


public class PT_XMLHashtable {
	
	public List<string>				keys = new List<string>();
	public List<PT_XMLHashList>		nodesList = new List<PT_XMLHashList>();
	public List<string>				attKeys = new List<string>();
	public List<string>				attributesList = new List<string>();
	
	public PT_XMLHashList Get(string key) {
		int ndx = Index(key);
		if (ndx == -1) return(null);
		return( nodesList[ndx] );
	}
	
	public void Set(string key, PT_XMLHashList val) {
		int ndx = Index(key);
		if (ndx != -1) {
			nodesList[ndx] = val;
		} else {
			keys.Add(key);
			nodesList.Add(val);
		}
	}
	
	public int Index(string key) {
		return(keys.IndexOf(key));
	}
	
	public int AttIndex(string attKey) {
		return(attKeys.IndexOf(attKey));
	}
	
	
	public PT_XMLHashList this[string s] {
		get {
			return( Get(s) );
		}
		set {
			Set( s, value );
		}
	}
	
	public string att(string attKey) {
		int ndx = AttIndex(attKey);
		if (ndx == -1) return("");
		return( attributesList[ndx] );
	}
	
	public void attSet(string attKey, string val) {
		int ndx = AttIndex(attKey);
		if (ndx == -1) {
			attKeys.Add(attKey);
			attributesList.Add(val);
		} else {
			attributesList[ndx] = val;
		}
	}
	
	public string text {
		get {
			int ndx = AttIndex("@");
			if (ndx == -1) return( "" );
			return( attributesList[ndx] );
		}
		set {
			int ndx = AttIndex("@");
			if (ndx == -1) {
				attKeys.Add("@");
				attributesList.Add(value);
			} else {
				attributesList[ndx] = value;
			}
		}
	}
	
	
	public string header {
		get {
			int ndx = AttIndex("@XML_Header");
			if (ndx == -1) return( "" );
			return( attributesList[ndx] );
		}
		set {
			int ndx = AttIndex("@XML_Header");
			if (ndx == -1) {
				attKeys.Add("@XML_Header");
				attributesList.Add(value);
			} else {
				attributesList[ndx] = value;
			}
		}
	}
	
	
	public string nodes {
		get {
			string s = "";
			foreach (string key in keys) {
				s += key+"   ";
			}
			return(s);
		}
	}
	
	public string attributes {
		get {
			string s = "";
			foreach (string attKey in attKeys) {
				s += attKey+"   ";
			}
			return(s);
		}
	}
	
	public bool ContainsKey(string key) {
		return( Index(key) != -1 );
	}
	
	public bool ContainsAtt(string attKey) {
		return( AttIndex(attKey) != -1 );
	}
	
	public bool HasKey(string key) {
		return( Index(key) != -1 );
	}
	
	public bool HasAtt(string attKey) {
		return( AttIndex(attKey) != -1 );
	}
	
}

/* Old XMLHashtable Class

public class XMLHashtable {
	
	private Hashtable hash = new Hashtable();
	
	public XMLArrayList this[string s] {
		get {
			return(hash[s] as XMLArrayList);
		}
		set {
			hash[s] = value;
		}
	}
	
	public string att(string s) {
		return(hash["@"+s] as string);
	}
	
	public void attSet(string s, string v) {
		hash["@"+s] = v;
	}
	
	public string text {
		get {
			return(hash["@"] as string);
		}
		set {
			hash["@"] = value;
		}
	}
	
	public string header {
		get {
			return(hash["@XML_Header"] as string);
		}
		set {
			hash["@XML_Header"] = value;
		}
	}
	
	public bool ContainsKey(string tag) {
		return(hash.ContainsKey(tag));
	}
	
}

*/


/*

1. look for <
2. look for next >
3. look for / before the >



*/
						
						