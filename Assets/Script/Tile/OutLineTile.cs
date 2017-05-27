using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineTile : MonoBehaviour {

    private SpriteRenderer _spriteRenderer;
	// Use this for initialization
	void Start () {
        _spriteRenderer = GetComponent<SpriteRenderer>();
	}
    
    public void CoverTile( int rowCount, int colCount, Vector3 position)
    {
        _spriteRenderer.size = new Vector2(rowCount * 0.29f, colCount * 0.29f);
        transform.position = position;
    }
}
