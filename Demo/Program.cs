using Microsoft.Extensions.DependencyInjection;
using Demo.GraphQL;
using StrawberryShake;

var serviceCollection = new ServiceCollection();

serviceCollection
    .AddPokemonClient()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://graphqlpokemon.favware.tech/v7"));

IServiceProvider services = serviceCollection.BuildServiceProvider();

IPokemonClient client = services.GetRequiredService<IPokemonClient>();

// See https://aka.ms/new-console-template for more information
while (true)
{
    Console.WriteLine("Insert Pokemon Name...");
    var name = Console.ReadLine();

    if (int.TryParse(name, out var pokemonNumber))
    {
        name = ((PokemonEnum)pokemonNumber).ToString();
    }

    if (Enum.TryParse(typeof(PokemonEnum), name, out var pokemon))
    {
        var result = await client.GetPokemon.ExecuteAsync((PokemonEnum)pokemon);
        result.EnsureNoErrors();
        var pok = result.Data!.GetPokemon;
        Console.WriteLine(new string('_', 50));
        Console.WriteLine($"{pok.Key} ({pok.Num}) HP: {pok.BaseStats.Hp} ATK: {pok.BaseStats.Attack} DEF: {pok.BaseStats.Defense}");
        Console.WriteLine($"Sprite: {pok.Sprite}");
        Console.WriteLine($"ShinySprite: {pok.ShinySprite}");
        Console.WriteLine($"Bulbapedia: {pok.BulbapediaPage}");
        Console.WriteLine($"SmogonPage: {pok.SmogonPage}");
        Console.WriteLine($"SerebiiPage: {pok.SerebiiPage}");
    }
    else
    {
        Console.WriteLine("Could not find this name, please try again.");
    }
}