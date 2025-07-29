# Exemplo de Uso do Azure Cache for Redis com C#

Este projeto demonstra como utilizar o Azure Cache for Redis para armazenar e recuperar objetos JSON em uma aplicação C# .NET.

## Pré-requisitos

Antes de executar este projeto, certifique-se de ter o seguinte instalado:

*   [.NET SDK 6.0](https://dotnet.microsoft.com/download/dotnet/6.0) ou superior.
*   Uma instância do [Azure Cache for Redis](https://azure.microsoft.com/en-us/services/cache/) configurada no Azure.
*   [Visual Studio](https://visualstudio.microsoft.com/downloads/) (recomendado) ou Visual Studio Code com as extensões C#.

## Configuração

1.  **Obtenha sua String de Conexão do Redis:**
    *   No portal do Azure, navegue até sua instância do Azure Cache for Redis.
    *   No menu à esquerda, em "Configurações", clique em "Chaves de acesso".
    *   Copie a "String de conexão primária" (ou secundária).

2.  **Atualize `appsettings.json`:**
    *   Abra o arquivo `appsettings.json` localizado em `RedisJsonExample/appsettings.json` (dentro da pasta do projeto).
    *   Substitua `<your_redis_connection_string>` pela string de conexão que você obteve no passo anterior.

    ```json
    {
      "RedisCache": {
        "ConnectionString": "sua_string_de_conexao_do_redis_aqui"
      }
    }
    ```

## Como Abrir e Executar o Projeto

1.  **Abra a Solução no Visual Studio:**
    *   Navegue até a pasta `redis-json-solution`.
    *   Abra o arquivo `RedisJsonExampleSolution.sln` com o Visual Studio.

2.  **Restaure as Dependências (se necessário):**
    *   O Visual Studio geralmente restaura os pacotes NuGet automaticamente ao abrir a solução. Se não, clique com o botão direito na solução no Gerenciador de Soluções e selecione "Restaurar Pacotes NuGet".

3.  **Execute a Aplicação:**
    *   No Visual Studio, pressione `F5` ou clique no botão "Iniciar" (geralmente um triângulo verde) para compilar e executar a aplicação.

Alternativamente, via linha de comando:

1.  **Navegue até o diretório da solução:**

    ```bash
    cd redis-json-solution
    ```

2.  **Execute a aplicação:**

    ```bash
    dotnet run --project RedisJsonExample/RedisJsonExample.csproj
    ```

O console exibirá as operações de cache (armazenamento, recuperação, verificação de existência e remoção) e os resultados.

## Estrutura do Projeto

*   `RedisJsonExampleSolution.sln`: Arquivo de solução do Visual Studio.
*   `RedisJsonExample/` (pasta do projeto):
    *   `Program.cs`: Ponto de entrada da aplicação, demonstra as operações do Redis.
    *   `Models/Product.cs`: Classe modelo que representa o objeto que será armazenado como JSON.
    *   `Services/RedisCacheService.cs`: Classe de serviço que encapsula a lógica de interação com o Azure Cache for Redis, incluindo métodos para `SetJsonAsync`, `GetJsonAsync`, `DeleteAsync`, etc.
    *   `appsettings.json`: Arquivo de configuração para a string de conexão do Redis.

## Pacotes NuGet Utilizados

*   `StackExchange.Redis`: Cliente Redis de alto desempenho para .NET.
*   `System.Text.Json`: Biblioteca para serialização e deserialização JSON (integrada ao .NET).
*   `Microsoft.Extensions.Configuration.Json`: Para carregar configurações de arquivos JSON.
*   `Microsoft.Extensions.Configuration.Binder`: Para vincular configurações a objetos C#.

---

