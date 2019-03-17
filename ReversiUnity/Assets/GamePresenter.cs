using System.Collections;
using System.Collections.Generic;
using Core;

using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GamePresenter : MonoBehaviour
{
    public int GridSize = 8;
    public GridLayoutGroup Board;
    public Button CellTemplate;
    public GameObject BlackPieceTemplate;
    public GameObject WhitePieceTemplate;
    public GameObject PossibleMoveTemplate;
    public Transform PieceRoot;

    public Button BlackPossibleMove;
    public Button WhitePossibleMove;
    public Text CurrentPossibleMove;

    ReactiveProperty<char>[,] GridModel;

    Dictionary<int, GameObject> pieceDic = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GridModel = new ReactiveProperty<char>[GridSize, GridSize];

        for (int i = 0; i < GridSize * GridSize; ++i)
        {
            var index = i;
            var row = i / GridSize;
            var col = i % GridSize;

            GridModel[row, col] = new ReactiveProperty<char>(Game.EmptyCellChar);

            //cell input control
            var cell = Instantiate(CellTemplate, Board.transform);
            cell.OnClickAsObservable().Subscribe(_ =>
            {
                GridModel[row, col].Value = NextPiece(GridModel[row, col].Value);
            });

            //grid view
            GridModel[row, col].Subscribe(newPiece =>
            {
                if (pieceDic.TryGetValue(index, out var piece))
                {
                    pieceDic.Remove(index);
                    Destroy(piece);
                }

                var pieceTemplate = GetPieceTemplate(newPiece);
                if (pieceTemplate != null)
                {
                    var pieceGameObject = Instantiate(pieceTemplate, PieceRoot);
                    pieceGameObject.transform.position = cell.transform.position;
                    pieceDic[index] = pieceGameObject;
                }
            });
        }
        CellTemplate.gameObject.SetActive(false);

        //generate possible move control
        BlackPossibleMove.OnClickAsObservable().Subscribe(_ =>
        {
            ClearAll();
            var game = new Game(ToGrid());
            var result = game.GeneratePossibleMove(Game.BlackPieceChar);
            FromGrid(result);
            CurrentPossibleMove.text = "Black";
        });

        WhitePossibleMove.OnClickAsObservable().Subscribe(_ =>
        {
            ClearAll();
            var game = new Game(ToGrid());
            var result = game.GeneratePossibleMove(Game.WhitePieceChar);
            FromGrid(result);
            CurrentPossibleMove.text = "White";
        });
    }

    GameObject GetPieceTemplate(char pieceChar)
    {
        switch (pieceChar)
        {
            case Game.BlackPieceChar:
                return BlackPieceTemplate;
            case Game.WhitePieceChar:
                return WhitePieceTemplate;
            case Game.PossibleMoveChar:
                return PossibleMoveTemplate;
            default:
                return null;
        }
    }

    void ClearAll()
    {
        foreach(var each in pieceDic)
        {
            Destroy(each.Value);
        }
        pieceDic.Clear();
    }

    char NextPiece(char piece)
    {
        var list = new List<char>()
        {
            Game.BlackPieceChar,
            Game.WhitePieceChar,
            Game.EmptyCellChar,
        };

        var index = list.IndexOf(piece) + 1;
        index %= list.Count;

        return list[index];
    }

    void FromGrid(char[,] grid)
    {
        var rowLength = grid.GetLength(0);
        var columnLength = grid.GetLength(1);

        for (int row = 0; row < rowLength; ++row)
        {
            for (int column = 0; column < columnLength; ++column)
            {
                GridModel[row, column].SetValueAndForceNotify(grid[row, column]);
            }
        }
    }

    char[,] ToGrid()
    {
        var rowLength = GridModel.GetLength(0);
        var columnLength = GridModel.GetLength(1);

        char[,] resultGrid = new char[rowLength, columnLength];
        for (int row = 0; row < rowLength; ++row)
        {
            for (int column = 0; column < columnLength; ++column)
            {
                var c = GridModel[row, column].Value;
                if(c == Game.PossibleMoveChar)
                {
                    c = Game.EmptyCellChar;
                }
                resultGrid[row, column] = c;
            }
        }

        return resultGrid;
    }
}
