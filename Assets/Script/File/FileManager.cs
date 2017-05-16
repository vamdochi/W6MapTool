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
public class FileManager : MonoBehaviour {

    private TileManager _tileManager;
    public GameObject ImagesParent;
    
	// Use this for initialization
	void Start () {
        _tileManager = GetComponent<TileManager>();
        string url = "C://Users/tpghk/OneDrive/Documents/W6MapTool/Resource";
        StartCoroutine("LoadAllFile", url);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // 이 함수 한번 다시 바꿔야할것 같소이다.
    // 지금 파일 형태 불러올려고 대충 짜놈 
    // 현재 파일 폴더 뷰도 안되어있고, 스크롤도 안되어있고, 안되어있는게 너무많다
    // 한번 정리해야할것 같음
    public IEnumerator LoadAllFile( string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            // 현재 png의 탑디렉토리만 불러오는데
            // 이것이 아마 들어있는 폴더들도 불러와야할듯?
            // 아니면 폴더를 따로 구하는 방법을 좀 찾아야함
            FileInfo[] fileInfos = directoryInfo.GetFiles("*.png", SearchOption.TopDirectoryOnly);

            // 블럭 사이즈의 하드코딩 없애고 최대한 TileManager의 값들을 가져오는 형태로 사용했으면함
            int blockSize = _tileManager.BlockSize * 4;
            Vector2 position = new Vector2(0, 0);

            foreach( var fileInfo in fileInfos)
            {
                WWW www = new WWW("file://" + fileInfo.FullName);
                yield return www;

                // 리소스 부르는 하드코딩부터 제거해야함~
                GameObject go = Instantiate(Resources.Load("Prefab/UI/RawImage", typeof(GameObject))) as GameObject;
                // 차일드 0번에 있는 Text 불러오는것도 제거해야함~
                // 아마도 따로 클래스파서 그것에 세팅해주면 알아서 바뀌는 형태가 좋을듯?
                go.transform.GetChild(0).GetComponent<Text>().text = fileInfo.Name;

                go.GetComponent<RawImage>().texture = www.texture;
                go.transform.SetParent(ImagesParent.transform );
                position.y -= blockSize;
                go.transform.localPosition = position;
            }
        }
    }

}
