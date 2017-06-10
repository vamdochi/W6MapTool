using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SpriteManager : SingleTon<SpriteManager> {

    Dictionary<string ,Sprite>  _spriteStorage = new Dictionary<string, Sprite>();

    public bool IsExistSprite( string fileName) { return _spriteStorage.ContainsKey(fileName); }
    public void AddSprite( string fileName, Sprite sprite)
    {
        // 절대경로 제거
        sprite.name = fileName.Replace(@"\", "/").Replace(Path.GetDirectoryName(Application.dataPath) + "/Resource/", "");
        _spriteStorage.Add(sprite.name, sprite);
    }

    public Sprite GetSprite(string fileName)
    {
        Sprite sprite;
        if( !_spriteStorage.TryGetValue(fileName, out sprite) )
            return null; // Throw가 더나을수도
        return sprite;
    }
}
