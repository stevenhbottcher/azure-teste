using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisJsonExample.Services
{
    public class RedisCacheService : IDisposable
    {
        private readonly IDatabase _database;
        private readonly ConnectionMultiplexer _redis;

        public RedisCacheService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _database = _redis.GetDatabase();
        }

        /// <summary>
        /// Armazena um objeto como JSON no cache Redis
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser armazenado</typeparam>
        /// <param name="key">Chave para identificar o objeto no cache</param>
        /// <param name="value">Objeto a ser armazenado</param>
        /// <param name="expiry">Tempo de expiração (opcional)</param>
        /// <returns>True se o objeto foi armazenado com sucesso</returns>
        public async Task<bool> SetJsonAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                return await _database.StringSetAsync(key, json, expiry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao armazenar no cache: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Recupera um objeto JSON do cache Redis
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser recuperado</typeparam>
        /// <param name="key">Chave do objeto no cache</param>
        /// <returns>O objeto deserializado ou null se não encontrado</returns>
        public async Task<T?> GetJsonAsync<T>(string key) where T : class
        {
            try
            {
                var json = await _database.StringGetAsync(key);
                
                if (!json.HasValue)
                    return null;

                return JsonSerializer.Deserialize<T>(json!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao recuperar do cache: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Remove um item do cache
        /// </summary>
        /// <param name="key">Chave do item a ser removido</param>
        /// <returns>True se o item foi removido com sucesso</returns>
        public async Task<bool> DeleteAsync(string key)
        {
            try
            {
                return await _database.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar do cache: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifica se uma chave existe no cache
        /// </summary>
        /// <param name="key">Chave a ser verificada</param>
        /// <returns>True se a chave existe</returns>
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                return await _database.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar existência no cache: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Define um tempo de expiração para uma chave existente
        /// </summary>
        /// <param name="key">Chave do item</param>
        /// <param name="expiry">Tempo de expiração</param>
        /// <returns>True se a expiração foi definida com sucesso</returns>
        public async Task<bool> SetExpiryAsync(string key, TimeSpan expiry)
        {
            try
            {
                return await _database.KeyExpireAsync(key, expiry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao definir expiração: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Obtém o tempo restante até a expiração de uma chave
        /// </summary>
        /// <param name="key">Chave do item</param>
        /// <returns>Tempo restante ou null se não há expiração definida</returns>
        public async Task<TimeSpan?> GetTimeToLiveAsync(string key)
        {
            try
            {
                return await _database.KeyTimeToLiveAsync(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter TTL: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Libera os recursos do Redis
        /// </summary>
        public void Dispose()
        {
            _redis?.Dispose();
        }
    }
}

