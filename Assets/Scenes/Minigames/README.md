# Minigames

Scenes in this directory should be set up to be loaded as a minigame.
In general, that means:

- Include the Prefabs/SceneInitialization script
- Include exactly one `Minigame` component (or a script that inherits from it).
- Your game should call `Minigame.EndGame` to end the game
- Your game should update the `Minigame.result` struct's `text` field with its output string, or pass a `MinigameResult` struct to the `EndGame` method.
