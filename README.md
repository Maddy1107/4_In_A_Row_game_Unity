# 🎮 Connect 4 in a Row

A polished, feature-rich Connect 4 game built in Unity with animations, audio, and smart AI using Minimax. Battle a human or outwit AI at various difficulty levels, including Impossible mode powered by decision trees.

---

## 🧩 Features

- 🎯 **4-in-a-Row Gameplay** — Classic, intuitive gameplay with grid snapping and smooth flow.
- 🧠 **AI with Difficulty Scaling**
  - Easy – Random moves
  - Medium – Blocks winning moves
  - Hard – Win-first, block-later logic
  - Impossible – Minimax with heuristics (2s, 3s, center control)
- 🕹️ **Local Multiplayer** — 2 players on the same device
- ⚙️ **Settings Menu**
  - Mute music/SFX
  - Toggle difficulty
- 🎨 **Responsive UI** — Resizes with resolution
- 🎉 **Juicy Effects**
  - Piece drop with bounce
  - Glow on win streak
  - Confetti win animation
  - SFX on click, hover, win/lose

---

## 🎮 Controls

| Action            | Input           |
|------------------|-----------------|
| Drop a piece     | Left-click on column |
| Navigate UI      | Mouse / Pointer |
| Toggle Music/SFX | In Settings Menu |

---

## 📁 Project Structure (Unity)

```
📁 AI
├── AIController.cs
├── AIMoveSelector.cs
├── AIMoveUtility.cs
├── MiniMaxAI.cs

📁 Audio
├── AudioManager.cs
├── AudioSettings.cs

📁 GameCore
├── GameEnums.cs
├── GameManager.cs
├── GameResult.cs

📁 GameCore
└── 📁 BoardScripts
    ├── Board.cs
    ├── Cell.cs

📁 Player
├── IHumanInputReceiver.cs
├── IPlayerController.cs
├── PlayerController.cs
├── PlayerFactory.cs

📁 UI
├── UIManager.cs

└── 📁 Menu
    ├── MainMenu.cs
    ├── SettingsMenu.cs

└── 📁 Popup
    ├── AreYouSurePopup.cs
    ├── WinPopup.cs

└── 📁 Utility
    ├── PopupAnimator.cs
    ├── RadioOption.cs
    ├── UIElementSFX.cs
```

## 🔊 Audio

- Button SFX
- Win/lose music
- Piece drop

---

## 🚀 How to Run

1. Open project in Unity
2. Press Play and enjoy!

---

## 🛠️ Future Improvements (Optional)

- Online multiplayer via Photon or Netcode
- Alpha-beta pruning for Minimax
- Visual tree explorer for AI debug
- Adaptive AI

---

## 👤 Credits

- **Developer:** Nilankar Deb  
- **Tools:** Unity, C#

---
