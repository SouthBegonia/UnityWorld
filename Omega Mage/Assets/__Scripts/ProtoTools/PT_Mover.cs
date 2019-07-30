using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PTM_State {
	idle,
	preMove,
	move,
	postMove
}

public delegate void PTM_Callback();

public class PT_Mover : PT_MonoBehaviour {
	public List<PT_Loc>		ptm_locs = null;
	public float			ptm_start;
	public float			ptm_duration;
	public PTM_State		ptm_state = PTM_State.idle;
	public string			ptm_easing = Easing.Linear;
	protected Material[]	ptm_mats;
	public float			ptm_u;
	public float			ptm_u2;
	public PTM_Callback		callback;

	// Update is called once per frame
	protected virtual void Update () {
		if (ptm_state == PTM_State.idle || ptm_state == PTM_State.postMove) return;

		// We know that we're set up to move
		float u = (Time.time - ptm_start)/ptm_duration;
		float u2 = u;
		if (u<0) {
			ptm_state = PTM_State.preMove;
			u=0;
			PT_SetLoc(ptm_locs[0]);
		} else if (u>1) {
			ptm_state = PTM_State.postMove;
			u=1;
			PT_SetLoc(ptm_locs[ptm_locs.Count-1]);
			if (callback != null) callback();
		} else {
			ptm_state = PTM_State.move;

			// Perform easing on u
			u2 = Easing.Ease(u, ptm_easing);

			PT_Loc l = PT_Loc.Bezier(u2, ptm_locs);
			PT_SetLoc(l);
		}
		// Place these variables where others can see them
		ptm_u = u;
		ptm_u2 = u2;
	}

	public void PT_SetLoc(PT_Loc l) {
		transform.position = l.pos;
		transform.rotation = l.rot;
		transform.localScale = l.scale;

		if (ptm_mats == null) {
			Renderer[] rends = GetComponentsInChildren<Renderer>();
			ptm_mats = new Material[rends.Length];
			for (int i=0; i<rends.Length; i++) {
				ptm_mats[i] = rends[i].material;
			}
		}
		foreach (Material m in ptm_mats) {
			m.color = l.color;
		}
	}

	public void PT_StartMove( IEnumerable<PT_Loc> locs, float timeDuration=1, float timeStart=float.NaN ) {
		if (float.IsNaN(timeStart)) timeStart = Time.time;
		ptm_start = timeStart;
		ptm_duration = timeDuration;
		ptm_locs = new List<PT_Loc>(locs);
		ptm_state = PTM_State.preMove;
	}
}

// PT_Loc holds all of the Transform + Color information for interpolation
[System.Serializable]
public class PT_Loc {
	public Vector3		position = Vector3.zero;
	public Quaternion	rotation = Quaternion.identity;
	public Vector3		scale = Vector3.one;
	public Color		color = Color.white;

	public Vector3		pos {
		get { return( position ); }
		set { position = value; }
	}

	public Quaternion	rot {
		get { return( rotation ); }
		set { rotation = value; }
	}

	public Vector3		localScale {
		get { return( scale ); }
		set { scale = value; }
	}

	public PT_Loc( Transform t = null ) {
		if (t == null) return;
		pos = t.position;
		rot = t.rotation;
		scale = t.localScale;

		// Note: Possibly pull Color in?
	}

	public static PT_Loc Bezier(float u, List<PT_Loc> locs) {
		// Pass to the Array version, which should be a tad faster
		return( Bezier (u, locs.ToArray()) );
	}

    // This is the new, fast and efficient version of PT_Loc.Bezier()
    public static PT_Loc Bezier(float u, PT_Loc[] locs, int iL=0, int iR=-1) {
        // iL and iR are indices into the array locs.
        // In this version, instead of creating lots of new arrays and wasting
        //   memory and time we pass the same array into each recursion and
        //   adjust these indices.
        if (iR == -1) {
            iR = locs.Length-1;
        }
        // This is the terminal case when both iL and iR point to the same element
        if (iL == iR) {
            return locs[iL];
        }
        // Since they still point to two different elements of locs,
        //   recur to divide the problem
        PT_Loc res = PT_Loc.Lerp( Bezier(u, locs, iL, iR-1), 
            Bezier(u, locs, iL+1, iR), u );
        return res;
    }

    // This is the old, less efficient version of the Bezier method
/*
	public static PT_Loc Bezier(float u, PT_Loc[] locs) {
		// Recursively solve this

		// If there is only 1 PT_Loc in locs, return it
		if (locs.Length == 1) return(locs[0]);

		int len = locs.Length-1;
		// Create locsL, which is all but the last element of locs
		// e.g. if locs = [0,1,2,3,4] then locsL = [0,1,2,3]
		PT_Loc[] locsL = new PT_Loc[len];
        System.Array.Copy(locs, 0, locsL, 0, len);
        //      PT_Loc[].Copy(locs, 0, locsL, 0, len);
		// Create locsR, which is all but the 0th element of locs
		// e.g. if locs = [0,1,2,3,4] then locsR = [1,2,3,4]
		PT_Loc[] locsR = new PT_Loc[len];
        System.Array.Copy(locs, 1, locsR, 0, len);
//		PT_Loc[].Copy(locs, 1, locsR, 0, len);

		// The result is the Lerp of these two shorter Lists
		PT_Loc res = Lerp( Bezier(u, locsL), Bezier(u, locsR), u );
		return( res );
	}
*/   

	public static PT_Loc Lerp( PT_Loc l0, PT_Loc l1, float u ) {
		PT_Loc l = new PT_Loc();
		l.pos = (1-u)*l0.pos + u*l1.pos;
		l.rot = Quaternion.Slerp( l0.rot, l1.rot, u );
		l.scale = (1-u)*l0.scale + u*l1.scale;
		l.color = (1-u)*l0.color + u*l1.color;
		return( l );
	}

	public PT_Loc Clone() {
		PT_Loc l = new PT_Loc();
		l.pos = pos;
		l.rot = rot;
		l.scale = scale;
		l.color = color;
		return(l);
	}




	/*
	 * 	static public Vector3 Bezier( float u, List<Vector3> vList ) {
		// If there is only one element in vList, return it
		if (vList.Count == 1) {
			return( vList[0] );
		}
		// Otherwise, create vListR, which is all but the 0th element of vList
		// e.g. if vList = [0,1,2,3,4] then vListR = [1,2,3,4]
		List<Vector3> vListR =  vList.GetRange(1, vList.Count-1);
		// And create vListL, which is all but the last element of vList
		// e.g. if vList = [0,1,2,3,4] then vListL = [0,1,2,3]
		List<Vector3> vListL = vList.GetRange(0, vList.Count-1);
		// The result is the Lerp of these two shorter Lists
		Vector3 res = Lerp( Bezier(u, vListL), Bezier(u, vListR), u );
		return( res );
	}



	/*
	public static PT_Loc operator - (PT_Loc l0, PT_Loc l1) {
		PT_Loc l = new PT_Loc();
		l.pos = l0.pos-l1.pos;
		l.rot = l0.rot-l1.rot;
		l.scale = l0.scale-l1.scale;
		l.color = l0.color-l1.color;
		return( l );
	}
	
	public static PT_Loc operator + (PT_Loc l0, PT_Loc l1) {
		PT_Loc l = new PT_Loc();
		l.pos = l0.pos+l1.pos;
		l.rot = l0.rot+l1.rot;
		l.scale = l0.scale+l1.scale;
		l.color = l0.color+l1.color;
		return( l );
	}
	
	public static PT_Loc operator * (PT_Loc l0, PT_Loc l1) {
		PT_Loc l = new PT_Loc();
		l.pos = l0.pos*l1.pos;
		l.rot = l0.rot*l1.rot;
		l.scale = l0.scale*l1.scale;
		l.color = l0.color*l1.color;
		return( l );
	}
	
	public static PT_Loc operator / (PT_Loc l0, PT_Loc l1) {
		PT_Loc l = new PT_Loc();
		l.pos = l0.pos/l1.pos;
		l.rot = l0.rot/l1.rot;
		l.scale = l0.scale/l1.scale;
		l.color = l0.color/l1.color;
		return( l );
	}
	
	public static PT_Loc operator * (PT_Loc l0, float f) {
		PT_Loc l = new PT_Loc();
		l.pos = l0.pos*f;
		l.rot = l0.rot*f;
		l.scale = l0.scale*f;
		l.color = l0.color*f;
		return( l );
	}
	
	public static PT_Loc operator / (PT_Loc l0, float f) {
		PT_Loc l = new PT_Loc();
		l.pos = l0.pos/f;
		l.rot = l0.rot/f;
		l.scale = l0.scale/f;
		l.color = l0.color/f;
		return( l );
	}
	*/
}