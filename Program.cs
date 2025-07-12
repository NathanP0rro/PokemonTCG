using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EstructurasDeDatos1
    {
    // -------------------- CLASES POKEMON Y CARTA --------------------

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

    public class Card
    {
        public int id { get; set; }
        public string rarity { get; set; }
        public int attack {  get; set; }
        public int life { get; set; }
        public Pokemon pokemonData { get; set; }
        public bool isPlayable { get; set; }

        public Card (int i, string r, int at, int l, Pokemon pd)
        {
            this.id = i;
            this.rarity = r;
            this.attack = at;
            this.life = l;
            this.pokemonData = pd;
            this.isPlayable = pokemonData.isBase;
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
            for(int i= 0; i < catalog.Length; i++)
            {
                if(catalog[i] != null)
                Console.WriteLine(i + " " + catalog[i].pokemonData.name);
            }
        }
         public void ViewCollection() //Muestra la colección (inventario) del jugador.
        {
            for (int i = 0; i < collection.Length; i++)
            {
                if (collection[i]!= null)
                Console.WriteLine(i + " " + collection[i].pokemonData.name);
            }
        }
        public void GachaponDraw() //selecciona aleatoriamente una carta del catálogo y la añade a la colección.
        {
            int indice;
            Random random = new Random();
            indice = random.Next(0,catalog.Length);
            bool flag = false;
            for(int i = 0; i < collection.Length; i++)
            {
                if (collection[i]==null)
                {
                    collection[i] = catalog[indice];
                    Console.WriteLine($"Your Pokemon is {collection[i].pokemonData.name}");
                    flag = true;
                    break;
                }             
            } 
            if(!flag)
            {
                Console.WriteLine("Collection is full");
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
                for (int i = 0; i < deck.Length; i++)
                {
                    
                    if (deck[i] == null)
                    {
                        deck[i] = new Card(collection[indice].id, collection[indice].rarity, collection[indice].attack, collection[indice].life, collection[indice].pokemonData);
                        Console.WriteLine($"Added {collection[indice].pokemonData.name} to your deck.");
                        flag = true;
                        break;
                    }                  
                }
                if (!flag)
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
            for ( i = deck.Length-1; i >= 0; i--)
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

            Console.WriteLine("Your Pokemon is " + jugador.ToString() );
            Console.WriteLine("Rival's Pokemon is " +  enemigo.ToString());
            Console.WriteLine();
            Console.WriteLine("Press Key to start battle");
            Console.ReadKey();
           
             while(jugador.life > 0 && enemigo.life > 0)
               {
                jugador.life = jugador.life - enemigo.attack;
                enemigo.life = enemigo.life - jugador.attack;
               }
           
            if (jugador.life <= 0 )
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
                Console.WriteLine("3-Realizar Gachapón Draw");
                Console.WriteLine("4-Construir Deck");
                Console.WriteLine("5-Ver Deck");
                Console.WriteLine("6-Iniciar Batalla");
                Console.WriteLine("7-Salir");
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
                        player.GachaponDraw();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;

                case ConsoleKey.D4:

                        Console.Clear();
                        player.BuildDeck();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;
                case ConsoleKey.D5:
                        
                        Console.Clear();
                        player.ViewDeck();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;

                case ConsoleKey.D6:

                        Console.Clear();
                        player.StartBattle();
                        Console.WriteLine();
                        Console.WriteLine("Pulse cualquier tecla para volver al menú");
                        Console.ReadKey();
                        break;

                }
            }             
            while (seleccion != ConsoleKey.D7 && seleccion != ConsoleKey.Escape);
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
