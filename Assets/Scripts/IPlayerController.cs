public interface IPlayerController
{
    int PlayerId { get; set; }
    bool IsHuman { get; }

    void PlayTurn();
}
