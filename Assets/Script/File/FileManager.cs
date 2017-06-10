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
    public void Save()
    {
        if (_w6mFile == null)
        {
            if (!CreateW6MFile(FILE_MODE.SAVE)) return;
        }
        _w6mFile.Save(_tileManager.GetTilesBinary());
    }
    public void SaveAs()
    {
        if (!CreateW6MFile(FILE_MODE.SAVE)) return;
        Save();
    }
    public void Load()
    {
        if (!CreateW6MFile(FILE_MODE.LOAD)) return;
        _tileManager.LoadTiles(_w6mFile.Load());
    }
    private bool CreateW6MFile(FILE_MODE fileMode )
    {
        string path = null;
        switch(fileMode)
        {
            case FILE_MODE.SAVE:
                path = FileBrower.OpenSaveDialog();
                break;
            case FILE_MODE.LOAD:
                path = FileBrower.OpenLoadDialog();
                break;
        }
        if (path == null) return false;

        _w6mFile = new W6MFile( path );
        return true;
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
    public static string OpenSaveDialog()
    {
        SimpleFileBrowser.FileBrowser.SetFilters(true,
            new SimpleFileBrowser.FileBrowser.Filter("Images", ".jpg", ".png"),
            new SimpleFileBrowser.FileBrowser.Filter("Text Files", ".txt", ".pdf"));


        SimpleFileBrowser.FileBrowser.SetDefaultFilter(".jpg");


        SimpleFileBrowser.FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        SimpleFileBrowser.FileBrowser.AddQuickLink(null, "Users", "C:\\Users");
        
        SimpleFileBrowser.FileBrowser.ShowSaveDialog( null, null, false, "C:\\", "Save", "Save" );
        return null; ;
        // Show a select folder dialog 
        // onSuccess event: print the selected folder's path
        // onCancel event: print "Canceled"
        // Load file/folder: folder, Initial path: default (Documents), Title: "Select Folder", submit button text: "Select"
        // FileBrowser.ShowLoadDialog( (path) => { Debug.Log( "Selected: " + path ); }, 
        //                                () => { Debug.Log( "Canceled" ); }, 
        //                                true, null, "Select Folder", "Select" );
    }
    public static string OpenLoadDialog()
    {
        return null;
    }
}
