# PokemonTCG
Basic Pokemon TCG using Data structures in C#

This is the second version of the Pokémon Trading Card Game (TCG) system built with C#. In this version, we expand the original base functionality by adding sorting capabilities and preventing duplicates in the deck.

🎯 Level Intermediate – You are expected to understand:

Object-oriented programming principles in C#

Array manipulation and iteration

Enum, CompareTo, Equals, GetHashCode

🧠 General Objective
Extend the application from version 1 by adding:

In-place sorting of the card collection using a configurable criteria

Duplicate prevention when adding cards to the deck


🧩 New Features and Requirements
🔢 1. In-Place Sorting of the Collection
The player can now sort their card collection using different criteria.

-Enum Creation
An enum named CardSortCriteria has been added with the following values:

Id, Name, Type, Rarity

-Static Sort Criteria
The Card class includes a static property:

public static CardSortCriteria CurrentSortCriteria { get; set; }

-CompareTo Implementation
The CompareTo(Card other) method has been modified to compare two cards depending on the active sorting criteria:

Id: compares numerically

Name: compares alphabetically, ignoring case

Type: compares the Pokémon type alphabetically, ignoring case

Rarity: uses numeric mapping:

Common < Rare < Especial < Legendary

-Ordering Method
The Player class has a method InPlaceOrder() which:

    Displays available sorting criteria

    Takes user input

    Updates Card.CurrentSortCriteria

    Uses Array.Sort() to sort the collection in place



🚫 2. Duplicate Prevention in the Deck
Cards cannot be added more than once to the deck.

Equals and GetHashCode Overridden
Cards are considered equal if they share the same Id.
Used to check for duplicates before adding.

Updated BuildDeck() Logic

Verifies if the selected card is already present in the deck

If it is, displays a message and doesn't add it

If not, creates a new instance and adds it

⚙️ Project Structure
🔥 Base Classes
Pokémon

Constructor: Receives all data except isPlayable, which is auto-determined
ToString(): Displays full card info
Implements CompareTo() based on the selected sort criteria

⚙️ Configuration

Contains arrays of PokemonDefinition and CardDefinition

Uses [JsonPropertyName("fieldName")] when needed

Loaded using:

JsonSerializer.Deserialize<Configuration>(jsonContent);

🧑‍🎮 Player Class

Manages:

catalog: all cards from config

collection: player inventory (up to 100 cards)

deck: current deck (up to 10 cards)

Counters: collectionCount, deckCount

Random draws

Key Methods:

    ViewCatalog() → shows available cards

    ViewCollection() → shows player’s inventory

    GachaponDraw() → adds a random card from catalog to collection

    BuildDeck() → adds a card from collection to deck, avoids duplicates

    InPlaceOrder() → sorts collection by selected criteria

    ViewDeck() → displays current deck

    StartBattle() → simulates a battle using deck

🖥️ Main Program
The program:

Loads the configuration file (config.json)

Creates a Player instance

Shows a menu with these options:

1. View Catalog
2. View Collection
3. Perform Gachapon Draw
4. Build Deck
5. View Deck
6. Sort Collection
7. Start Battle
8. Exit
9. 
📁 Included Files
program.cs


config.json

README.md (this file)

💬 Final Notes

Ensure that config.json is placed correctly in the project directory

Arrays are still used for all internal data — no generic collections

Next version will be adding iterator and Iterable
