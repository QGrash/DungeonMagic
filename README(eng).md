# Dungeon Magic

A console-based RPG in C# with turn-based combat, random elements, and player choices that affect the story. This project was created for educational purposes to master object-oriented programming fundamentals and console application development.

## Design Philosophy

### 1. Why the console?
I chose the console as my canvas to focus purely on mechanics and player choice, without getting lost in graphics or asset creation. The limitation of text and symbols forces clarity: every element must be justified by gameplay. It's a tribute to classic roguelikes and the minimalist arcade games I've loved (like the "Space Villain" terminal in Space Station 14).

### 2. Magic Schools & Resistance System
I wanted combat to be tactical, not just "click to win." Four schools of magic (Fire, Water, Air, Light) each have their own strengths and weaknesses against different enemy types. This forces the player to adapt their strategy to the opponent rather than spamming the same spell. Different colors in the console provide instant feedback on which magic is effective.

### 3. Player Choice & Consequences
Every spell cast increases an "essence" counter for the corresponding school. By the final boss, the game tracks which schools you've favored, and the dialogue changes accordingly – you can even avoid the battle entirely if your choices align with the boss's nature. This is my way of making the player's journey meaningful, not just linear.

### 4. Resource Management
Health and mana regenerate slowly, and potion quantities are limited (maximum two of each type). This creates tension: save potions for the boss or use them now to survive? The "Strange Potion" adds unpredictability – sometimes it helps, sometimes it harms. It's a small risk/reward element that makes each playthrough unique.

### 5. Multiple Endings
I believe games should react to how you play. The ending isn't just a cutscene – it's a reflection of your playstyle. Whether you fight the boss, convince him to stand down, or summon a holy golem depends on the essence you've accumulated. This encourages replayability and gives weight to every decision.

### 6. Code as Design
The code is intentionally straightforward (some might say "crutchy") because my goal was rapid iteration and idea validation. As I grow, I'll refactor and optimize, but the core principle remains: design first, elegance second.

## Features

- **Four Magic Schools**: Fire, Water, Air, Light – each with its own console color and damage multipliers against different enemies
- **Advanced Magic**: Barrier and healing spells that ignore resistances but consume mana
- **Resistance System**: Each enemy (golems and the boss) has unique vulnerabilities and resistances to magic schools
- **Essence Accumulation**: Every spell cast increases a counter for its school, affecting the final dialogue with the boss
- **Inventory & Potions**: Defeated golems drop potions (healing, mana, fortitude, strange potion). You can carry no more than two potions of each type
- **Random Effects**: The strange potion may heal, restore mana, grant fortitude, or poison you
- **Multiple Endings**: Based on accumulated essence and the boss's eye color, you can avoid combat, summon a holy golem, or engage in battle
- **Mana Regeneration**: 10 mana restored after each turn (player + enemy)

## Controls

During combat, choose actions by entering the corresponding number:

- `1` – Fireball (fire, 20 mana)
- `2` – Ice Spike (water, 20 mana)
- `3` – Air Strike (air, 20 mana)
- `4` – Holy Ray (light, 20 mana)
- `5` – Protective Barrier (advanced, 10 mana) – blocks one attack
- `6` – Holy Healing (advanced, 10 mana) – +20 HP
- `7` – Healing Potion (+30 HP) [quantity]
- `8` – Mana Potion (+30 MP) [quantity]
- `9` – Fortitude Potion (3 charges, incompatible with barrier) [quantity]
- `0` – Strange Potion (random effect) [quantity]

When casting a spell, you'll need to select the target number (living golem).

In dialogues, simply enter the option number (1 or 2) and press Enter.

## Installation & Launch

1. Make sure [.NET SDK](https://dotnet.microsoft.com/download) (version 5.0 or higher) is installed
2. Clone the repository: `git clone https://github.com/QGrash/DungeonMagic.git`
3. Navigate to the project folder: `cd DungeonMagic`
4. Run the game: `dotnet run`
   
   Or open the `.sln` / `.csproj` file in Visual Studio and press `F5`

## Project Structure

- `Program.cs` – main game code (`Enemy` and `Player` classes, combat logic, drops, dialogues)
- The project is a simple console application with no external dependencies

## Screenshots

*Coming soon... Maybe...*

## Authors

- **Concept and Game Design**: Leonid Kotov
- **Code and Implementation**: Leonid Kotov, with assistance from an AI assistant for code structure formalization and text processing (DeepSeek)

## License

This project is distributed under the MIT license. See the `LICENSE` file for details.