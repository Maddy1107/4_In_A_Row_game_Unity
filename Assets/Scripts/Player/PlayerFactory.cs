using UnityEngine;

public static class PlayerFactory
{
    public static IPlayerController CreatePlayer(PlayerType type, int playerId)
    {
        IPlayerController player = type switch
        {
            PlayerType.Human => new PlayerController(),
            PlayerType.AI => new AIController(
                (Difficulty)System.Enum.Parse(typeof(Difficulty),
                PlayerPrefs.GetString("GameDifficulty", Difficulty.Easy.ToString()))),

            _ => throw new System.NotImplementedException($"PlayerType {type} not handled")
        };

        player.PlayerId = playerId;
        return player;
    }
}