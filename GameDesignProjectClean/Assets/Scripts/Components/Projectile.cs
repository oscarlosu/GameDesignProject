using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    public GameObject SourceCore;
    public GameObject SourceStructure;
    public int Damage;
	public int MaxLifespan;
	public bool InGrace {get; protected set;}
	protected ObjectPool pool;	

	protected void Awake()
	{
		pool = GameObject.FindGameObjectWithTag(GlobalValues.ObjectPoolTag).GetComponent<ObjectPool>();
	}
}
