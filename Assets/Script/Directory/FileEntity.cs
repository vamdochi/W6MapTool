using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FileEntity : MonoBehaviour {


    public Text FileText;
    public delegate void OnClickFile( );
    public event OnClickFile ClickFileEventHanlder;

    private Button _button;
    private RawImage _rawImage;
    
	// Use this for initialization
	void Awake() {
        _rawImage = GetComponent<RawImage>();
        _button = GetComponent<Button>();

	}
    

    public void Initialize( string fileName, Transform parentTransform, Vector3 localPosition, Texture2D texture = null )
    {
        transform.SetParent( parentTransform);
        transform.localPosition = localPosition;
        FileText.text = fileName;

        if( texture != null)
            _rawImage.texture = texture;

        transform.localScale = Vector3.one;
        _button.onClick.AddListener(new UnityAction(()=> {
            ClickFileEventHanlder.Invoke();
            }));
    }
}
