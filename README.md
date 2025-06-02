# OpponentStatistics
## 
# STRAFTAT Opponent Stats Mod

## Overview
This mod for *STRAFTAT* displays opponent statistics (rank and score) in the in-game chat. It retrieves data from the game's leaderboard system, likely integrated with Steam's API.

## Functionality
The mod uses the `LeaderboardManager` to fetch opponent data from the game's leaderboard system and displays it in the chat. No malicious activity (e.g., unauthorized data transmission, file modification, or system access) is present in the code.

### How It Works
1. **Leaderboard Access**:
   - Uses reflection to access `LeaderboardManager` from `Settings.Instance`.
   - `LeaderboardManager` handles interactions with the game's leaderboard system.

2. **Data Retrieval**:
   - Calls `LeaderboardManager.leaderboard.GetEntries` with the opponent's Steam ID.
   - Fetches leaderboard entries for the specified user.

3. **Data Display**:
   - Processes the response in a delegate function.
   - Extracts `Rank` and `Score` from the `LeaderboardEntry`.
   - Formats and displays the data in the chat as: `Rank: {entry.Rank}\nScore: {entry.Score}`.

## Data Source
The mod retrieves rank and score from the game's leaderboard system, likely via Steamworks API, ensuring official and accurate data.

## Safety
- No evidence of harmful behavior.
- Uses BepInEx for logging, with no unauthorized file or system access.

## Usage
1. Install the mod using a compatible mod loader (e.g., BepInEx).
2. Ensure *STRAFTAT* and Steam are running.
3. Opponent stats will appear in the in-game chat during matches.

## Notes
- Requires a working Steam integration for leaderboard data.
- Tested with *STRAFTAT*'s official leaderboard system.

## Disclaimer
**Use this mod at your own risk**. This mod is from the creator of cheats, but the function is separate from unfair things, so it does not perform any functionality other than what is stated. I am not responsible for your data and computer. I checked it using the paid version of Grok 3. I myself do not understand the code 🥴
### Example
![123](https://github.com/MLGbalu/trash/blob/main/image.png?raw=true)
