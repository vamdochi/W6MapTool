using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryManager : MonoBehaviour {

    private const int _historyMaxCount = 10;

    private List<History> _historyList;
    private List<History> _frontHistoryList;

	// Use this for initialization
	void Start () {
        _historyList = new List<History>();
        _frontHistoryList = new List<History>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateHistory();
    }

    public void PushHistory(History history)
    {
        if (_historyList.Count > _historyMaxCount)
        {
            _historyList.RemoveAt(0);
        }
        _historyList.Add(history);
        _frontHistoryList.Clear();
    }

    private void UpdateHistory()
    {
        if (InputSystem.Instance.GetKeyCode(CustomKeyCode.RestoreHistory) &&
            !_historyList.Empty())
        {
            History history = _historyList[_historyList.Count - 1];
            if (history != null)
            {
                HandleHistory(history);

                _frontHistoryList.Add(history);
                _historyList.Remove(history);
            }
        }
        else if (InputSystem.Instance.GetKeyCode(CustomKeyCode.FrontRestoreHistory) &&
            !_frontHistoryList.Empty())
        {
            History history = _frontHistoryList[_frontHistoryList.Count - 1];
            if (history != null)
            {
                HandleHistory(history);
                _frontHistoryList.Remove(history);
                _historyList.Add(history);
            }
        }
    }
    private void HandleHistory( History history)
    {
        switch(history.Type )
        {
            case HistoryType.AllocateTile:
                var changedResourceHistory = history as ChangedResourceHistory;
                changedResourceHistory.Restore();
                break;
        }
    }


}
