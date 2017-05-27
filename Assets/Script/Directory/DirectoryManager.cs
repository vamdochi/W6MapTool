using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// 네이밍도 바꿔 파일매니저가 뭐야 ㅡㅡ;
// 지금 형태에 절대 안맞음. 리소스에 특화되어서 만들어져있어
// 형태를 바꾸던가 명명을 맞추던가 해야할듯
// 파일 불러오기를 정규화시키는것도 방법~
// 일단 여기 코드 다바꿔야함!
public class DirectoryManager : MonoBehaviour {

    private const string _rawImagePath = "Prefab/UI/FileView";
    private const string _topDirectoryPath = "C:\\Users\\tpghk\\OneDrive\\Documents\\W6MapTool\\Resource";
    private const int _iconfileSize = 25;
    private const int _iconExpandPer = 2;
    private const string _imageExtensionName = ".png";

    public GameObject ImagesParent;
    private string _currDirectoryPath;
    private List<GameObject> _directoryBtnList;
    private TileManager _tileManager;
    
	// Use this for initialization
	void Start () {
        _directoryBtnList = new List<GameObject>();
        _tileManager = GetComponent<TileManager>();
        LoadDirectory(_topDirectoryPath);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnMoveParentDirectory()
    {
        if( _currDirectoryPath != _topDirectoryPath)
        {
            LoadDirectory(_currDirectoryPath.Substring(0, _currDirectoryPath.LastIndexOf('\\')) );
        }
    }
    public void LoadDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            StartCoroutine("LoadAllFile", directoryPath);
        }
    }
    // 이 함수 한번 다시 바꿔야할것 같소이다.
    // 지금 파일 형태 불러올려고 대충 짜놈 
    // 현재 파일 폴더 뷰도 안되어있고, 스크롤도 안되어있고, 안되어있는게 너무많다
    // 한번 정리해야할것 같음
    private IEnumerator LoadAllFile( string directoryPath)
    {
        // Load시 기존의 FileEntity 전부 제거해야함~
        foreach ( var go in _directoryBtnList)
        {
            Destroy(go);
        }
        _directoryBtnList.Clear();
        var topDirectoryInfo = new DirectoryInfo(directoryPath);
        // 현재 png의 탑디렉토리만 불러오는데
        // 이것이 아마 들어있는 폴더들도 불러와야할듯?
        // 아니면 폴더를 따로 구하는 방법을 좀 찾아야함
        DirectoryInfo[] topDirectories  = topDirectoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
        FileInfo[] fileInfos            = topDirectoryInfo.GetFiles("*" + _imageExtensionName, SearchOption.TopDirectoryOnly);

        // 블럭 사이즈의 하드코딩 없애고 최대한 TileManager의 값들을 가져오는 형태로 사용했으면함

        Vector2 fileEntityPosition = new Vector2(0, 0);

        if(_topDirectoryPath != directoryPath)
        {
            fileEntityPosition.y -= _iconfileSize * ( _iconExpandPer + 1 );
        }

        foreach ( var directoryInfo in topDirectories)
        {
            WWW www = new WWW("file://" + directoryInfo.FullName);
            yield return www;

            FileEntity fileEntity = CreateFileEntity(directoryInfo);
            fileEntity.ClickFileEventHanlder += () => { StartCoroutine("LoadAllFile",directoryInfo.FullName); };
            fileEntity.Initialize(directoryInfo.Name, ImagesParent.transform, fileEntityPosition, null);
            fileEntityPosition.y -= _iconfileSize * ( _iconExpandPer + 1);
            _directoryBtnList.Add(fileEntity.gameObject);
        }
        foreach( var fileInfo in fileInfos)
        { 
            WWW www = new WWW("file://" + fileInfo.FullName);
            yield return www;

            FileEntity fileEntity = CreateFileEntity( fileInfo);
            fileEntity.ClickFileEventHanlder += () => { _tileManager.ChangeSellectTile(Sprite.Create(www.texture, Rect.MinMaxRect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f))); };
            fileEntity.Initialize(fileInfo.Name, ImagesParent.transform, fileEntityPosition, www.texture);
            fileEntityPosition.y -= www.texture.height * ( _iconExpandPer + 1);
            _directoryBtnList.Add(fileEntity.gameObject);
        }
        _currDirectoryPath = directoryPath;
    }

    private FileEntity CreateFileEntity(FileSystemInfo fileInfo) { return (Instantiate(Resources.Load(_rawImagePath, typeof(GameObject))) as GameObject).GetComponent<FileEntity>(); }

}
