using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class Board : NetworkBehaviour
{
    public GameObject[,] board = new GameObject[10, 9];
    // GameObject prefabs
    public GameObject blackGeneral; public GameObject blackAdvisor; public GameObject blackElephant; public GameObject blackKnight; public GameObject blackRook; public GameObject blackCannon; public GameObject blackSoldier;
    public GameObject redGeneral; public GameObject redAdvisor; public GameObject redElephant; public GameObject redKnight; public GameObject redRook; public GameObject redCannon; public GameObject redSoldier;
    public bool redPlaying = true;
    public GameObject camR; public GameObject camB;
    public GameObject rollBackUIPanel; public GameObject resignUIPanel; public GameObject resetUIPanel;
    public int turnCount = 0;
    public Dictionary<int, List<PieceData>> boardHistory = new Dictionary<int, List<PieceData>>();
    public TMP_Text turnIndicatorText;

    public class PieceData
    {
        public string pieceType;
        public string colour;
        public int boardX;
        public int boardY;
    }

    void Start()
    {
        InitializeBoard();
    }

    void InitializeBoard()
    {

        PlacePiece(blackGeneral, 0, 4, "B");
        PlacePiece(blackAdvisor, 0, 3, "B");
        PlacePiece(blackAdvisor, 0, 5, "B");
        PlacePiece(blackElephant, 0, 2, "B");
        PlacePiece(blackElephant, 0, 6, "B");
        PlacePiece(blackKnight, 0, 1, "B");
        PlacePiece(blackKnight, 0, 7, "B");
        PlacePiece(blackRook, 0, 0, "B");
        PlacePiece(blackRook, 0, 8, "B");
        PlacePiece(blackCannon, 2, 1, "B");
        PlacePiece(blackCannon, 2, 7, "B");
        PlacePiece(blackSoldier, 3, 0, "B");
        PlacePiece(blackSoldier, 3, 2, "B");
        PlacePiece(blackSoldier, 3, 4, "B");
        PlacePiece(blackSoldier, 3, 6, "B");
        PlacePiece(blackSoldier, 3, 8, "B");
        PlacePiece(redGeneral, 9, 4, "R");
        PlacePiece(redAdvisor, 9, 3, "R");
        PlacePiece(redAdvisor, 9, 5, "R");
        PlacePiece(redElephant, 9, 2, "R");
        PlacePiece(redElephant, 9, 6, "R");
        PlacePiece(redKnight, 9, 1, "R");
        PlacePiece(redKnight, 9, 7, "R");
        PlacePiece(redRook, 9, 0, "R");
        PlacePiece(redRook, 9, 8, "R");
        PlacePiece(redCannon, 7, 1, "R");
        PlacePiece(redCannon, 7, 7, "R");
        PlacePiece(redSoldier, 6, 0, "R");
        PlacePiece(redSoldier, 6, 2, "R");
        PlacePiece(redSoldier, 6, 4, "R");
        PlacePiece(redSoldier, 6, 6, "R");
        PlacePiece(redSoldier, 6, 8, "R");
        SetActivity(redPlaying);
        boardHistory[turnCount] = saveState();
        turnIndicatorText.text = "Red's Turn"; turnIndicatorText.color = new Color(0.7f, 0, 0);
        ChangeCamera();
    }

    void PlacePiece(GameObject prefab, int x, int y, string colour)
    {
        int yRot = (colour == "B") ? 0 : 180;
        GameObject piece = Instantiate(prefab, WorldSpaceCoords(x, y), Quaternion.Euler(new Vector3(-90, yRot, 0)));
        Piece pieceScript = piece.GetComponent<Piece>();
        pieceScript.boardX = x; pieceScript.boardY = y; pieceScript.board = this;
        board[x, y] = piece;
    }

    public Vector3 WorldSpaceCoords(int y, int x)
    {
        return new Vector3(-0.85f + (x * 0.2125f), 0.27f, 1.075f - (y * 0.215f));
    }

    public void MovePiece(int xFrom, int yFrom, int xTo, int yTo)
    {
        if (board[xFrom, yFrom] != null)
        {
            if (board[xTo, yTo] != null)
            {
                GameObject pieceDelete = board[xTo, yTo];
                Destroy(pieceDelete);
            }
            GameObject pieceMove = board[xFrom, yFrom];
            Piece pieceScript = pieceMove.GetComponent<Piece>();
            board[xTo, yTo] = pieceMove;
            pieceMove.transform.position = WorldSpaceCoords(xTo, yTo);
            pieceScript.boardX = xTo; pieceScript.boardY = yTo;
            board[xFrom, yFrom] = null;
        }
        ClearAllPrefabs();
        ChangeTurn();
    }

    public void ClearAllPrefabs()
    {
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] != null)
                {
                    Piece pieceScript = board[row, col].GetComponent<Piece>();
                    pieceScript.ClearPrefabs();
                }
            }
        }
    }

    public void SetActivity(bool redPlaying)
    {
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] != null)
                {
                    Piece pieceScript = board[row, col].GetComponent<Piece>();
                    if (pieceScript.colour == "R")
                    {
                        pieceScript.active = redPlaying;
                    }
                    else
                    {
                        pieceScript.active = !redPlaying;
                    }
                }
            }
        }
    }

    public void ChangeTurn()
    {
        removeResetDialog();
        removeResignDialog();
        removeRollBackDialog();
        redPlaying = !redPlaying;
        turnCount++;
        boardHistory[turnCount] = saveState();
        gameEndChecks();
    }

    public void ChangeCamera()
    {
        if (GameIsOffline())
        {
            if (redPlaying) { camR.SetActive(true); camB.SetActive(false); }
            else { camR.SetActive(false); camB.SetActive(true); }
        }
        else
        {
            if (NetworkManager.Singleton.IsHost) { camR.SetActive(true); camB.SetActive(false); }
            else { camR.SetActive(false); camB.SetActive(true); }
        }
    }

    public bool SpaceSafe(Vector2Int checkSpace, bool redPlaying)
    {
        List<GameObject> checkPieces = new List<GameObject>();
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] != null)
                {
                    Piece pieceScript = board[row, col].GetComponent<Piece>();
                    if ((pieceScript.colour == "R" && !redPlaying) || (pieceScript.colour == "B" && redPlaying))
                    {
                        checkPieces.Add(board[row, col]);
                    }
                }
            }
        }
        bool spaceSafe = true;
        foreach (GameObject checkPiece in checkPieces)
        {
            Piece pieceScript = checkPiece.GetComponent<Piece>();
            List<Vector2Int> pieceSpaces = pieceScript.GetPossibleSpaces();
            if (pieceSpaces.Contains(checkSpace))
            {
                spaceSafe = false;
                break;
            }
        }
        return spaceSafe;
    }

    public bool getCheck(bool redPlaying)
    {
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] != null)
                {
                    Piece pieceScript = board[row, col].GetComponent<Piece>();
                    if (pieceScript.ToString() == "G")
                    {
                        if ((pieceScript.colour == "R" && redPlaying) || (pieceScript.colour == "B" && !redPlaying))
                        {
                            return !SpaceSafe(new Vector2Int(pieceScript.boardX, pieceScript.boardY), redPlaying);
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool getNoLegalMoves(bool redPlaying)
    {
        List<Vector2Int> allMoves = new List<Vector2Int>();
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] != null)
                {
                    Piece pieceScript = board[row, col].GetComponent<Piece>();
                    if ((pieceScript.colour == "R" && redPlaying) || (pieceScript.colour == "B" && !redPlaying))
                    {
                        allMoves.AddRange(pieceScript.FilterSelfChecks(pieceScript.GetPossibleSpaces()));
                    }
                }
            }
        }
        if (allMoves.Count != 0)
        {
            return false;
        }
        return true;
    }

    public List<PieceData> saveState()
    {
        List<PieceData> allPieces = new List<PieceData>();
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] != null)
                {
                    Piece pieceScript = board[row, col].GetComponent<Piece>();
                    PieceData piece = new PieceData
                    {
                        pieceType = pieceScript.ToString(),
                        colour = pieceScript.colour,
                        boardX = pieceScript.boardX,
                        boardY = pieceScript.boardY,
                    };
                    allPieces.Add(piece);
                }
            }
        }
        return allPieces;
    }

    public void loadState(int turnNumber)
    {
        ClearAllPrefabs();
        clearBoard();
        foreach (PieceData piece in boardHistory[turnNumber])
        {
            PlacePiece(GetPrefabFromType(piece.pieceType, piece.colour), piece.boardX, piece.boardY, piece.colour);
        }
        SetActivity(redPlaying);
    }

    public void clearBoard()
    {
        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] != null)
                {
                    GameObject pieceDelete = board[row, col];
                    Destroy(pieceDelete);
                    board[row, col] = null;
                }
            }
        }
    }

    public GameObject GetPrefabFromType(string pieceType, string colour)
    {
        if (colour == "R")
        {
            switch (pieceType)
            {
                case "G": return redGeneral;
                case "A": return redAdvisor;
                case "E": return redElephant;
                case "K": return redKnight;
                case "R": return redRook;
                case "C": return redCannon;
                case "S": return redSoldier;
            }
        }
        else
        {
            switch (pieceType)
            {
                case "G": return blackGeneral;
                case "A": return blackAdvisor;
                case "E": return blackElephant;
                case "K": return blackKnight;
                case "R": return blackRook;
                case "C": return blackCannon;
                case "S": return blackSoldier;
            }
        }
        return null;
    }

    public void RollbackTurn()
    {
        if (turnCount > 0)
        {
            loadState(turnCount - 1);
            redPlaying = !redPlaying;
            turnCount--;
            gameEndChecks();
        }
    }

    public void gameEndChecks()
    {
        bool inCheck = getCheck(redPlaying);
        if (inCheck)
        {
            bool inCheckMate = getNoLegalMoves(redPlaying);
            if (inCheckMate)
            {
                FindObjectOfType<WinUI>().ShowWinDialog(redPlaying ? "Black" : "Red", "checkmate");
            }
        }
        else
        {
            bool inStaleMate = getNoLegalMoves(redPlaying);
            if (inStaleMate)
            {
                FindObjectOfType<WinUI>().ShowWinDialog(redPlaying ? "Black" : "Red", "stalemate");
            }
        }
        SetActivity(redPlaying);
        turnIndicatorText.text = redPlaying ? "Red's Turn" : "Black's Turn";
        turnIndicatorText.color = redPlaying ? new Color(0.7f, 0, 0) : new Color(0, 0, 0);
        ChangeCamera();
    }

    public void Resign()
    {
        removeResignDialog();
        FindObjectOfType<WinUI>().ShowWinDialog(redPlaying ? "Black" : "Red", "resignation");
    }

    public void Reset()
    {
        turnCount = 0;
        redPlaying = true;
        clearBoard();
        InitializeBoard();
    }

    public bool GameIsOffline()
    {
        return !NetworkManager.Singleton.IsListening;
    }

    public bool PlayerCanMove()
    {
        return GameIsOffline() || (NetworkManager.Singleton.IsHost && redPlaying || !NetworkManager.Singleton.IsHost && !redPlaying);
    }

    public void rollbackButton()
    {
        if (GameIsOffline())
        {
            RollbackTurn();
        }
        else
        {
            SendRollbackDialogServerRPC(NetworkManager.Singleton.LocalClientId);
        }
    }

    public void resignButton()
    {
        resignUIPanel.gameObject.SetActive(true);
    }

    public void resignConfirm()
    {
        if (GameIsOffline())
        {
            Resign();
        }
        else
        {
            AttemptResignServerRPC();
        }
    }

    public void resetButton()
    {
        if (GameIsOffline())
        {
            resetUIPanel.gameObject.SetActive(true);
        }
        else
        {
            SendResetDialogServerRPC(NetworkManager.Singleton.LocalClientId);
        }
    }

    public void resetConfirm()
    {
        if (GameIsOffline())
        {
            Reset();
        }
        else
        {
            AttemptResetServerRPC();
        }
    }

    public void removeRollBackDialog()
    {
        rollBackUIPanel.gameObject.SetActive(false);
    }

    public void removeResignDialog()
    {
        resignUIPanel.gameObject.SetActive(false);
    }

    public void removeResetDialog()
    {
        resetUIPanel.gameObject.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AttemptMoveServerRPC(int xFrom, int yFrom, int xTo, int yTo)
    {
        AttemptMoveClientRPC(xFrom, yFrom, xTo, yTo);
    }

    [ClientRpc]
    public void AttemptMoveClientRPC(int xFrom, int yFrom, int xTo, int yTo)
    {
        MovePiece(xFrom, yFrom, xTo, yTo);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendRollbackDialogServerRPC(ulong clientId)
    {
        SendRollbackDialogClientRPC(clientId);
    }

    [ClientRpc]
    public void SendRollbackDialogClientRPC(ulong clientId)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            rollBackUIPanel.gameObject.SetActive(true);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void AttemptRollbackServerRPC()
    {
        AttemptRollbackClientRPC();
    }

    [ClientRpc]
    public void AttemptRollbackClientRPC()
    {
        RollbackTurn();
        removeRollBackDialog();
    }

    [ServerRpc(RequireOwnership = false)]
    public void AttemptResignServerRPC()
    {
        AttemptResignClientRPC();
    }

    [ClientRpc]
    public void AttemptResignClientRPC()
    {
        Resign();
        removeResignDialog();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendResetDialogServerRPC(ulong clientId)
    {
        SendResetDialogClientRPC(clientId);
    }

    [ClientRpc]
    public void SendResetDialogClientRPC(ulong clientId)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            resetUIPanel.gameObject.SetActive(true);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void AttemptResetServerRPC()
    {
        AttemptResetClientRPC();
    }

    [ClientRpc]
    public void AttemptResetClientRPC()
    {
        Reset();
        removeResetDialog();
    }
}

