public class PlayerController : IPlayerController, IHumanInputReceiver
{
    public int PlayerId { get; set; }
    public bool IsHuman => true;

    private int selectedCol = -1;

    public void SetColumn(int col)
    {
        selectedCol = col;
        PlayTurn();
    }

    public void PlayTurn()
    {
        if (selectedCol >= 0)
        {
            int moveCol = selectedCol;
            selectedCol = -1;
            GameManager.Instance.TryMakeMove(moveCol);
        }
    }
}
