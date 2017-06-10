using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using W6;

public enum FILE_MODE
{
    SAVE, LOAD
}
public class FileManager : MonoBehaviour {
    private W6MFile     _w6mFile = null;
    private TileManager _tileManager;
    void Start()
    {
        _tileManager = GetComponent<TileManager>();
    }
    private void Update()
    {
        if( InputSystem.Instance.GetKeyCode(CustomKeyCode.Save))
            Save();
        else if (InputSystem.Instance.GetKeyCode(CustomKeyCode.SaveAs))
            SaveAs();
    }
    public void Save()
    {
        if (_w6mFile == null)
            FileOpen(FILE_MODE.SAVE);
        else
            _w6mFile.Save(_tileManager.GetTilesBinary());
    }
    public void SaveAs()
    {
        FileOpen(FILE_MODE.SAVE);
    }
    public void Load()
    {
        FileOpen(FILE_MODE.LOAD);
    }
    private bool FileOpen(FILE_MODE fileMode )
    {
        switch(fileMode)
        {
            case FILE_MODE.SAVE:
                FileBrower.OpenSaveDialog(( s ) => { CreateW6MFile(s, fileMode); });
                break;
            case FILE_MODE.LOAD:
                FileBrower.OpenLoadDialog(( s ) => { CreateW6MFile(s, fileMode); });
                break;
        }
        return true;
    }
    private void CreateW6MFile( string path, FILE_MODE fileMode)
    {
        if (path != null)
        {
            _w6mFile = new W6MFile(path);

            switch (fileMode)
            {
                case FILE_MODE.SAVE:
                    _w6mFile.Save(_tileManager.GetTilesBinary());
                    break;
                case FILE_MODE.LOAD:
                    _tileManager.LoadTiles(_w6mFile.Load());
                    break;
            }
        }
    }
}
[Serializable]
public class IndexKey
{
    public string FileName;
    public int Index;
}
public class W6MFile
{
    private string _fileName;

    public W6MFile() { }
    public W6MFile( string fileName )
    {
        _fileName = fileName;
    }
    public byte[] Load()
    {
        return File.ReadAllBytes(_fileName);
    }
    public void Save( byte[] binaryData)
    {
        using (Stream stream = File.Open(_fileName, FileMode.Create, FileAccess.ReadWrite))
        {
            stream.Write(binaryData, 0, binaryData.Length);
        }
    }
}

public class FileBrower
{
    public static void OpenSaveDialog(SimpleFileBrowser.FileBrowser.OnSuccess success)
    {
        SimpleFileBrowser.FileBrowser.SetFilters(true,
            new SimpleFileBrowser.FileBrowser.Filter("w6m", ".w6m"));
        SimpleFileBrowser.FileBrowser.SetDefaultFilter(".w6m");
        SimpleFileBrowser.FileBrowser.ShowSaveDialog(success, null);
        
    }
    public static void OpenLoadDialog(SimpleFileBrowser.FileBrowser.OnSuccess success)
    {
        SimpleFileBrowser.FileBrowser.SetFilters(true,
            new SimpleFileBrowser.FileBrowser.Filter("w6m", ".w6m"));
        SimpleFileBrowser.FileBrowser.SetDefaultFilter(".w6m");
        SimpleFileBrowser.FileBrowser.ShowLoadDialog(success, null);
    }
}
