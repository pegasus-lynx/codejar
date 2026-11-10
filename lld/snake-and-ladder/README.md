## Snake and Ladder 

### Requirements
1. **Is the board fixed size?**
	- No, it can be variable sized.
2. **How many players?**
	- Atleast 2, but it can be more.
3. **How is dice rolled?**
	- Simulated or taken as input
4. **What are the other rules of the game ?**
	- If a 6 is rolled, roll again.
	- Three 6s, revert the player back to original square.
	- If a roll, moves the person ahead of 100, then the turn is skipped.
	- Multiple players can occupy same position
5. **Is number of ladders and snakes configurable?**
	- Yes, they can be configured.

### Non-Functional Requirements
- Maintainability
- Enhancability
- Modularity
- User-feedback: After each turn people get enough information.

### Design

#### Entities
- [x] **Player** 
	- Members : private name, position
	- Methods : public SetPosition()
- [x] **Board**
	- Members : private snakes, ladders
	- Methods : public GetNewPosition, IsEndPosition
- [x] **Game** 
	- Members : private board, state, nextPlayer, dice
	- Methods : MakeMove, GetGameState
- [x] **abstract Jumper**
	- Properties : Start, End (getter public)
- [x] **Dice**
	- Methods: public roll
- [x] **Snake : Jumper**
- [x] **Ladder : Jumper**
- [x] **GameStatus : enum**


#### Problems with v1 design.
- Game can be constructed using Builder pattern as it contains a lot of parameters.
- Encapsulation issue - Position is a game concern but owned by Player, due to which it is mutable, allowing potential outside interaction for state that needs to be handled internally by the game.
- GetGameState - mixes game logic with UI logic
- DRY issue - why maintain two separate list of tunnels - ladders and snakes.
- Mutability of game entities - players, snakes and ladders at game time.

#### v2 Design Improvements
- Merged board tunnels - fixes DRY
- Internal GamePlayer prevents external entities from mutating players and positions.

#### v3 Design Improvements
- Added GameBuilder to allow building the game
- Fixed correctness issues.