# PokemonTCG
Basic Pokemon TCG using Data structures in C#

The objective in this version is to set up the base of the TCG with all the basic clases and methods.
There's no use of Data Structures outside from arrays in this part 

🎯 Level
Beginner – You are expected to understand:

-Object-oriented programming fundamentals in C#

-Array manipulation

🧠 General Objective
Develop a C# application that manages cards from the Pokémon Trading Card Game (TCG). You'll design and implement several classes to model the following concepts:

Pokémon: Basic data (name, type, and whether it’s a base Pokémon).

Card: Attributes like ID, rarity, attack, life, and the Pokémon associated.

Configuration: Loads data from a JSON file to define the card catalog and Pokémon info.

Player: Logic for managing the catalog, inventory collection, and deck using arrays (including resizing when necessary).

Main Program: Menu interface that calls functions like viewing the catalog, drawing cards (Gachapon), building a deck, starting battles, etc.

🧩 Project Structure
1. 🔥 Base Classes

Pokemon
Attributes:

  name (string)

  type (string)

  isBase (bool)

Constructor: Initializes all properties.

ToString() Override: Returns: {Name} (Type: {Type}) - Base: {IsBase}

Card
  Attributes:

  id (int)

  rarity (string)
  
  attack (int)

  life (int)

  pokemonData (Pokemon)

  isPlayable (bool) → automatically set based on pokemonData.isBase

  Constructor: Takes all attributes (except isPlayable, which is inferred) and assigns them.

  ToString() Override: Returns detailed card info in readable format.

2. ⚙️ Configuration

Classes:
Configuration: Contains:

  pokemons: Array of PokemonDefinition

  cards: Array of CardDefinition

  PokemonDefinition and CardDefinition: Properties match fields from config.json, using [JsonPropertyName("fieldName")] if needed.

Loading Config:

csharp
JsonSerializer.Deserialize<Configuration>(jsonContent)

3. 🧑‍🎮 Player Class
Manages:

  catalog: Cards from JSON

  collection: Player’s inventory (100 cards)

  deck: Battle deck (10 cards)

  Counters: collectionCount, deckCount

  Random instance for draws

Key Methods:
  ViewCatalog() → Shows all cards available

  ViewCollection() → Shows player’s inventory

  GachaponDraw() → Adds a random card from catalog to collection

  BuildDeck() → Adds selected card from collection to deck (new instance, does not remove from collection)

  ViewDeck() → Displays current deck

  StartBattle() → Simulates a battle between last added card in deck and a random enemy card

4. 🖥️ Main Program

Loads configuration from config.json

Checks for successful load

Creates Player object

Displays looped menu with options:

1. View Catalog
2. View Collection
3. Perform Gachapon Draw
4. Build Deck
5. View Deck
6. Start Battle
7. Exit
Each option triggers the related method from the Player class.

📁 Included Files
program.cs
EstructuraDeDatos1.sln
EstructuraDeDatos1.csproj
config.json
README.md (this file)

💬 Final Notes
Ensure that config.json is properly formatted and placed in the same folder as your executable.

The next version will focus in the use on Enum, CompareTo, Equals...
