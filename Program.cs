using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EstructuraDeDatos2
{
    // -------------------- CLASES POKEMON Y CARTA --------------------
    public enum CardSortCriteria
    {
        Id = 0,
        Name = 1,
        Type = 2,
        Rarity = 3,
    }
    public class Pokemon
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool isBase { get; set; }

        public Pokemon(string n, string t, bool ib)
        {
            this.name = n;
            this.type = t;
            this.isBase = ib = true;
        }

        public override string ToString()
        {
            return $"{this.name}(Type:{this.type}) - Base: {this.isBase}";
        }
    }

    public class Card : IComparable<Card>
    {
        public int id { get; set; }
        public string rarity { get; set; }
        public int attack { get; set; }
        public int life { get; set; }
        public Pokemon pokemonData { get; set; }
        public bool isPlayable { get; set; }
        public CardSortCriteria CurrentSortCriteria { get; set; }

        public int CompareTo(Card other)
        {
            if (other == null) return 1;

            switch(CurrentSortCriteria)
            {
                case CardSortCriteria.Id:
                    return this.id.CompareTo(other.id);
                case CardSortCriteria.Name:
                    return string.Compare(this.pokemonData.name.ToUpper(), other.pokemonData.name.ToUpper());
                case CardSortCriteria.Type:
                    return string.Compare(this.pokemonData.type.ToUpper(), other.pokemonData.type.ToUpper());
                case CardSortCriteria.Rarity:
                    return  this.Rareza().CompareTo(other.Rareza);                 
            }
            return 1;
        }

        private int Rareza()
        {
            int value = 0;
            switch(rarity)
            {
                case "Legendary":
                    value = 100;
                    break; ;
                case "Especial":
                    value = 50;
                    break;
                case "Rare":
                    value = 25;
                    break;
                case "Common":
                    value = 10;
                    break;
            }
            return value;
        }
        public Card(int i, string r, int at, int l, Pokemon pd)
        {
            this.id = i;
            this.rarity = r;
            this.attack = at;
            this.life = l;
            this.pokemonData = pd;
            this.isPlayable = pokemonData.isBase;
            this.CurrentSortCriteria = CardSortCriteria.Id;
        }
        public override string ToString()
        {
            return $"{this.pokemonData.name}(Type:{this.pokemonData.type}) - ID: {this.id} - Rariy: {this.rarity} - Life: {this.life} - Attack: {this.attack} - Playable: {this.isPlayable} ";
        }
    }

    // -------------------- CONFIGURACIÓN --------------------
    // La configuración se carga desde un archivo JSON.
    public class Configuration
    {
        [JsonPropertyName("pokemons")]
        public PokemonDefinition[] pokemons { get; set; }

        [JsonPropertyName("cards")]
        public CardDefinition[] cards { get; set; }
    }

    public class PokemonDefinition
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("isbase")]
        public bool isbase { get; set; }
    }

    public class CardDefinition
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("pokemon")]
        public string pokemon { get; set; }

        [JsonPropertyName("attack")]
        public int attack { get; set; }

        [JsonPropertyName("life")]
        public int life { get; set; }

        [JsonPropertyName("rarity")]
        public string rarity { get; set; }
    }


    // -------------------- CLASE PLAYER (usando arrays) --------------------
    public class Player
    {
        private Card[] collection;
        private Card[] deck;
        private Card[] catalog;
        private Random rng;
        private Configuration config;
        private PokemonDefinition[] pokemonDefinitions;
        private int collectionCount;
        private int deckCount;

        public Player(Configuration config)
        {
            this.config = config;
            this.collection = new Card[100];
            this.deck = new Card[10];

            pokemonDefinitions = config.pokemons;

            catalog = new Card[config.cards.Length];

            for (int i = 0; i < config.cards.Length; i++)
            {
                catalog[i] = CreateCard(config.cards[i]);
            }
        }

        // Crea una carta a partir de una definición.
        private Card CreateCard(CardDefinition cd)
        {
            // Buscar la definición del Pokémon
            PokemonDefinition pd = null;
            for (int i = 0; i < pokemonDefinitions.Length; i++)
            {
                if (pokemonDefinitions[i].name.Equals(cd.pokemon))
                {
                    pd = pokemonDefinitions[i];
                    break;
                }
            }
            // Crear el objeto Pokémon.
            Pokemon poke = new Pokemon(pd.name, pd.type, pd.isbase);
            return new Card(cd.id, cd.rarity, cd.attack, cd.life, poke);
        }

        public void ViewCatalog() //Opción para ver el catálogo completo.
        {
            for (int i = 0; i < catalog.Length; i++)
            {
                if (catalog[i] != null)
                    Console.WriteLine(i + " " + catalog[i].pokemonData.name);
            }
        }
        public void ViewCollection() //Muestra la colección (inventario) del jugador.
        {
            for (int i = 0; i < collection.Length; i++)
            {
                if (collection[i] != null)
                    Console.WriteLine(i + " " + collection[i].pokemonData.name);
            }
        }
        public void GachaponDraw() //selecciona aleatoriamente una carta del catálogo y la añade a la colección.
        {
            int indice;
            Random random = new Random();
            indice = random.Next(0, catalog.Length);
            bool flag = false;
            for (int i = 0; i < collection.Length; i++)
            {
                if (collection[i] == null)
                {
                    collection[i] = catalog[indice];
                    Console.WriteLine($"Your Pokemon is {collection[i].pokemonData.name}");
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                Console.WriteLine("Collection is full");
            }
        }

        public void InPlaceOrder()
        {
            string input;
            Console.WriteLine("Select the ordening method: ");
            Console.WriteLine(" ");
            foreach (CardSortCriteria item in Enum.GetValues(typeof(CardSortCriteria)))
            {
                
                Console.WriteLine($"{(int)item} - {item}");
                
            }
            input = Console.ReadLine();
            Console.Clear();
          
            if(int.TryParse(input, out int indice) && indice >= 0 || indice <= 3)
            {             
                switch (indice)
                    {
                        case 0:
                        foreach (var card in collection)
                        {
                            if (card != null)
                                card.CurrentSortCriteria = CardSortCriteria.Id;
                        }
                        Array.Sort(collection);
                        ViewCollection();
                            break;
                        case 1:
                        foreach (var card in collection)
                        {
                            if (card != null)
                                card.CurrentSortCriteria = CardSortCriteria.Name;
                        }
                        Array.Sort(collection);
                        ViewCollection();
                        break;
                        case 2:
                        foreach (var card in collection)
                        {
                            if (card != null)
                                card.CurrentSortCriteria = CardSortCriteria.Type;
                        }
                        Array.Sort(collection);
                        ViewCollection();
                        break;
                        case 3:
                        foreach (var card in collection)
                        {
                            if (card != null)
                                card.CurrentSortCriteria = CardSortCriteria.Rarity;
                        }
                        Array.Sort(collection);
                        ViewCollection();
                        break;                
                }
         
           }    
        }
        public void BuildDeck() //elige una carta de la colección y la añade al deck (sin quitarla de la colección).
        {
            Console.WriteLine("Choose the Pokemon you want to add to your deck");
            ViewCollection();
            string input = Console.ReadLine();
            if (int.TryParse(input, out int indice) && indice >= 0 && indice < collection.Length && collection[indice] != null)
            {
                bool flag = false;
                bool duplicate = false;
                for (int i = 0; i < deck.Length; i++)
                {
     
                        {
                          if(deck[i] != null && collection[indice].id == deck[i].id)
                          {
                                Console.WriteLine("This card is already on Deck");
                                duplicate = true;
                                flag = true;
                                break;
                           }
                        }
                        
                    if (!duplicate)
                    { 
                        if(deck[i] == null)
                        { 
                         deck[i] = new Card(collection[indice].id, collection[indice].rarity, collection[indice].attack, collection[indice].life, collection[indice].pokemonData);
                         Console.WriteLine($"Added {collection[indice].pokemonData.name} to your deck.");
                         flag = true;
                         break;
                        }
                    }

                }
                if (!flag )
                {
                    Console.WriteLine("Deck is full");
                }
            }
            else
            {
                Console.WriteLine("Invalid value or empty slot selected.");
            }

        }
        public void ViewDeck() //Muestra el deck actual.
        {
            for (int i = 0; i < deck.Length; i++)
            {
                if (deck[i] != null)
                    Console.WriteLine(i + " " + deck[i].pokemonData.name);
            }
        }

        public void StartBattle() //Inicia una batalla simple usando la última carta del deck.
        {
            int i;
            int indice;
            Card jugador = null;
            Card enemigo;
            Random random = new Random();
            indice = random.Next(0, deck.Length);
            for (i = deck.Length - 1; i >= 0; i--)
            {
                if (deck[i] != null)
                {
                    jugador = deck[i];
                    break;
                }
            }
            if (jugador == null)
            {
                Console.WriteLine("No Pokemon in Deck");
                return;
            }
            do
            {
                indice = random.Next(0, deck.Length);
                enemigo = deck[indice];
            }
            while (enemigo == null || enemigo == jugador);

            Console.WriteLine("Your Pokemon is " + jugador.ToString());
            Console.WriteLine("Rival's Pokemon is " + enemigo.ToString());
            Console.WriteLine();
            Console.WriteLine("Press Key to start battle");
            Console.ReadKey();

            while (jugador.life > 0 && enemigo.life > 0)
            {
                jugador.life = jugador.life - enemigo.attack;
                enemigo.life = enemigo.life - jugador.attack;
            }

            if (jugador.life <= 0)
            {
                Console.WriteLine("rival's " + enemigo.pokemonData.name + " won");
            }
            else
            {
                Console.WriteLine("Congratulations! You won");
            }
        }
    }

    // -------------------- PROGRAMA PRINCIPAL --------------------
    internal class Program
    {
        static void Main(string[] args)
        {
            Configuration config = LoadConfig("config.json");
            if (config == null)
            {
                Console.WriteLine("Error loading configuration. Exiting.");
                return;
            }

            Player player = new Player(config);
            void MostrarMenu()
            {
                Console.Clear();
                Console.WriteLine("Seleccione opción del menú");
                Console.WriteLine();
                Console.WriteLine("1-Ver Catálogo");
                Console.WriteLine("2-Ver Colección");
                Console.WriteLine("3-Ordenar colección");
                Console.WriteLine("4-Realizar Gachapón Draw");
                Console.WriteLine("5-Construir Deck");
                Console.WriteLine("6-Ver Deck");
                Console.WriteLine("7-Iniciar Batalla");
                Console.WriteLine("8-Salir");
            }

            ConsoleKey seleccion;

            do
            {
                MostrarMenu();
                seleccion = Console.ReadKey().Key;
                switch (seleccion)
                {
                    case ConsoleKey.D1:

                        Console.Clear();
                        player.ViewCatalog();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;

                    case ConsoleKey.D2:

                        Console.Clear();
                        player.ViewCollection();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;
                    
                    case ConsoleKey.D3:

                        Console.Clear();
                        player.InPlaceOrder();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;

                    case ConsoleKey.D4:

                        Console.Clear();
                        player.GachaponDraw();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;

                    case ConsoleKey.D5:

                        Console.Clear();
                        player.BuildDeck();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.D6:

                        Console.Clear();
                        player.ViewDeck();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;

                    case ConsoleKey.D7:

                        Console.Clear();
                        player.StartBattle();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;

                }
            }
            while (seleccion != ConsoleKey.D8 && seleccion != ConsoleKey.Escape);
        }

        static Configuration LoadConfig(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                Configuration config = JsonSerializer.Deserialize<Configuration>(json);
                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading config: " + ex.Message);
                return null;
            }

        }
    }
}
