using Microsoft.Extensions.Configuration;
using RedisJsonExample.Models;
using RedisJsonExample.Services;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisJsonExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 1. Configurar o carregamento do appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration config = builder.Build();

            var redisConnectionString = config["RedisCache:ConnectionString"];

            if (string.IsNullOrEmpty(redisConnectionString) || redisConnectionString == "<your_redis_connection_string>")
            {
                Console.WriteLine("Por favor, atualize a string de conexão do Redis em appsettings.json.");
                Console.WriteLine("Você pode obter uma string de conexão criando um Azure Cache for Redis no portal do Azure.");
                Console.WriteLine("Pressione qualquer tecla para sair...");
                Console.ReadKey();
                return;
            }

            // 2. Inicializar o serviço de cache Redis
            using var redisService = new RedisCacheService(redisConnectionString);

            Console.WriteLine("\n--- Testando operações do Azure Cache for Redis ---");

            // 3. Criar um objeto de exemplo
            var product = new Product
            {
                Id = 1,
                Name = "Smartphone X",
                Price = 999.99m,
                Category = "Electronics",
                Description = "Latest model with advanced features.",
                InStock = true,
                CreatedAt = DateTime.UtcNow
            };

            var productKey = $"product:{product.Id}";

            Console.WriteLine($"\nTentando armazenar o produto '{product.Name}' no cache...");
            var setSuccess = await redisService.SetJsonAsync(productKey, product, TimeSpan.FromMinutes(5));

            if (setSuccess)
            {
                Console.WriteLine($"Produto '{product.Name}' armazenado com sucesso no cache com a chave '{productKey}'.");
            }
            else
            {
                Console.WriteLine($"Falha ao armazenar o produto '{product.Name}' no cache.");
            }

            // 4. Recuperar o objeto do cache
            Console.WriteLine($"\nTentando recuperar o produto com a chave '{productKey}' do cache...");
            var retrievedProduct = await redisService.GetJsonAsync<Product>(productKey);

            if (retrievedProduct != null)
            {
                Console.WriteLine($"Produto recuperado do cache: {retrievedProduct.Name} (Preço: {retrievedProduct.Price:C})");
                Console.WriteLine("JSON completo do produto recuperado:");
                Console.WriteLine(JsonSerializer.Serialize(retrievedProduct, new JsonSerializerOptions { WriteIndented = true }));
            }
            else
            {
                Console.WriteLine($"Produto com a chave '{productKey}' não encontrado no cache ou falha na recuperação.");
            }

            // 5. Testar a existência da chave
            Console.WriteLine($"\nVerificando se a chave '{productKey}' existe no cache...");
            var exists = await redisService.ExistsAsync(productKey);
            Console.WriteLine($"Chave '{productKey}' existe: {exists}");

            // 6. Testar remoção do cache
            Console.WriteLine($"\nTentando remover o produto com a chave '{productKey}' do cache...");
            var deleteSuccess = await redisService.DeleteAsync(productKey);

            if (deleteSuccess)
            {
                Console.WriteLine($"Produto com a chave '{productKey}' removido com sucesso do cache.");
            }
            else
            {
                Console.WriteLine($"Falha ao remover o produto com a chave '{productKey}' do cache.");
            }

            // 7. Verificar novamente após a remoção
            Console.WriteLine($"\nVerificando se a chave '{productKey}' existe no cache após a remoção...");
            exists = await redisService.ExistsAsync(productKey);
            Console.WriteLine($"Chave '{productKey}' existe: {exists}");

            Console.WriteLine("\n--- Demonstração concluída. ---");
            Console.WriteLine("Lembre-se de substituir '<your_redis_connection_string>' em appsettings.json pela sua string de conexão real do Azure Cache for Redis.");
            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}

