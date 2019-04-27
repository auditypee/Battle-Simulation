# Battle-Simulation
Simulates a turn based battle in Unity. Much like the first few installments of the Final Fantasy Series. WIP

## What I'm trying to achieve:
### Functional Requirements
- Player and Ally exist
- Enemies exist
- User has options during battle through player and ally
  - Attack
    - Attack one monster
    - Combo attack
  - Items
    - Drink potion to heal or gain mana
    - Use reviving item
  - Powers
    - Fireball
    - Lightning bolt
    - Water spray
    - Combine powers
  - Run
    - Will end fight if roll success
- Enemies can attack back

### Pseudocode
```
Player turn
• Choose attack option for Hero
	○ If attack
		§ If back
			□ go back to action options
		§ choose enemy to attack
		§ ally turn
	○ If powers
		§ If back
			□ go back to action options
		§ choose power to use
			□ choose enemy to attack
		§ Ally turn
	○ If items
		§ if back
			□ go back to action options
		§ Choose item to use
			□ choose actor to use item on
		§ Ally turn
	○ If run
		§ Ally turn
• Choose action option for Ally
	○ If attack
		§ go back option
		§ choose enemy to attack
			□ If enemy is same as hero target
				® Option to combine attacks
		§ End turn
	○ If powers
		§ Go back option
		§ Choose power to use
			□ If enemy is same hero as target
				® Option to combine attacks
		§ End turn
	○ If items
		§ Go back option
		§ Choose item to use
			□ Choose actor to use item on
		§ End turn
	○ If run
		§ End turn
Monster turn
• Choose random target, or maybe low health
Resolve who goes first
• Speed based
Deal damage
Check deaths
• If all enemies dead
	○ Give loot to players
	○ End battle
• If player and ally dead
	○ Game over
• If player dead
	○ Ally can revive if able
• If ally dead
	○ Player can revive if able
Repeat
```
