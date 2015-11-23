using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public GameObject SourceCore;
    public GameObject SourceStructure;
    public int Damage;
	public bool InGrace {get; protected set;}
}
