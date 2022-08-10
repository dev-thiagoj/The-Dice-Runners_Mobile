using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class PiecesManager : Singleton<PiecesManager>
{
    public List<LevelPieceBase> pieces;
    public int _index = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    [NaughtyAttributes.Button]
    public void AddPiecesToGame()
    {
        for (int i = 0; i <= _index; i++)
        {
            LevelManager.Instance.levelPieces.Add(pieces[i]);
        }

        _index += 1;
        PlayerPrefs.SetInt("pieceIndex", _index);
    }
}
