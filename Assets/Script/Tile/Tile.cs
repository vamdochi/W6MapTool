using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private SpriteRenderer _spriteRenderer;
	// Use this for initialization
	void Start () {
        _spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeSprite( Sprite sprite) { _spriteRenderer.sprite = sprite; }
}
