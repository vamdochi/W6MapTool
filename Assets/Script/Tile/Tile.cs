using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public SpriteRenderer SpriteRenderer { get; private set; }
	// Use this for initialization
	void Start () {
        SpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Sprite GetSprite() { return SpriteRenderer.sprite; }
    public void ChangeSprite( Sprite sprite) { SpriteRenderer.sprite = sprite; }
}
