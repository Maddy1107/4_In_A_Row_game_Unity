// Type of player
public enum PlayerType { Human, AI }

// Current game result
public enum Result { Ongoing, Win, Draw }

// Current UI screen/state
public enum UIState { MainMenu, Game, SettingsMenu }

// AI difficulty level
public enum Difficulty { Easy, Medium, Hard, Impossible }

// Sound effects used in UI
public enum UISFX
{
    None,
    Hover,
    NormalClick,
    BackClick,
    ModeSwitchClick,
    ToggleSwitchClick,
    Piecefall,
    Win,
    Lose
}

// Whether audio is currently enabled or muted
public enum AudioState { On, Off }

// Used for identifying radio button groups
public enum RadioOptionType { Difficulty, SFX }

// Game modes supported
public enum GameMode { Local, AI }
