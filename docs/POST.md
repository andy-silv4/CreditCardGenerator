# DESENVOLVENDO UMA API REST COM .NET CORE

Existem vários tutoriais na internet de como fazer isso, eu sei. O desenvolvimento de uma API não é lá um bicho de sete cabeças, principalmente quando você entende bem os conceitos de uma API REST e entende bem o que sua API precisa fazer.

Para criar uma API só é preciso ter conhecimentos intermediários sobre o protocolo HTTP, servidores web e uma linguagem de programação para web. Utilizando frameworks como o .NET Core essa tarefa parece ainda mais "simples". Será?

Nesse post vou tentar dar o meu ponto de vista sobre a criação de APIs REST, explicar um pouco alguns conceitos básicos e por fim desenvolver uma API de exemplo para geração de números de cartão de crédito.

## REST

REST tem como significado *Representational State Transfer* que traduzido, significa transferência representacional de estado. O significado soa estranho, mas segundo o Wikipédia, "é um estilo de arquitetura de software que define um conjunto de restrições a serem usadas para a criação de web services (serviços Web)."

Nada mais é que um meio de comunicação entre aplicações utilizando requisições HTTP. Através de uma API, por exemplo, contendo regras bem definidas e interfaces devidamente declaradas permitem a integração dessa API com outras aplicações. Isso permite, no nosso caso, a criação das APIs RESTful, que são aplicações que utilizam e respeitam o REST.
	
Para nosso estudo de caso, o gerador de números de cartão de crédito, precisamos entender então como são realizadas essa comunicação utilizando o HTTP.

## Métodos HTTP

O HTTP é um protocolo utilizado para transferência de dados e que disponibiliza diversas ações para comunicação na web. Os métodos HTTP (ou verbos HTTP) são utilizados para essa função, muito importante para a criação de APIs REST. Os verbos mais comuns são o GET e o POST, que resumidamente possibilita receber informações e enviar informações.

Uma API necessita desses e de outros métodos para realizar a comunicação, uma vez que cada ação tem uma função especifica sobre as informações ou dados que estão sendo trafegadas. Nas APIs, esses dados são geralmente no formato JSON, que na arquitetura REST transfere representações do estado dessas informações. Isso vai ficar mais claro na desenvolvimento da aplicação.

Veja a lista com os principais métodos:

| Método | Descrição                                                    |
| ------ | ------------------------------------------------------------ |
| GET    | O método GET solicita a representação de um recurso específico. Requisições utilizando o método GET devem retornar apenas dados. |
| POST   | O método POST é utilizado para submeter uma entidade a um recurso específico, frequentemente causando uma mudança no estado do recurso ou efeitos colaterais no servidor. |
| PUT    | O método PUT substitui todas as atuais representações do recurso de destino pela carga de dados da requisição. |
| DELETE | O método DELETE remove um recurso específico.                |



## API REST: Gerador de Números de cartão de crédito

Como eu bem disse no começo desse post, criar uma API não é difícil. Talvez possa ser complicado. Felizmente na internet existem diversos tutoriais para nos ajudar (rs). 

Como nossa API irá gerar números de cartão de crédito, precisamos primeiramente saber o que iremos construir. Gerar números aleatórios parece ser simples, uma vez que somente precisaríamos solicitar a geração através de um método POST, utilizar alguma função que gere números aleatórios do C#, (ou alguma biblioteca que gere números de cartão válidos, com [essa](https://github.com/gustavofrizzo/CreditCardValidator)) e disponibilizar na tela através de uma chamada GET.

Para essa API resolvi pesquisar como funciona a geração de números para um cartão de crédito. Para isso basta entender o [algoritmo de Luhn](https://en.wikipedia.org/wiki/Luhn_algorithm) e realizar a implementação. Para desenvolver a API, vou utilizar o .NET Core 3.1 e irei criar o projeto utilizando a [CLI do .NET](https://docs.microsoft.com/pt-br/dotnet/core/tools/). Estarei utilizando o VSCode para o desenvolvimento, mas o Visual Studio é mais completo para uma melhor experiência no desenvolvimento.

Vamos criar 2 endpoints. Um receberá o e-mail da pessoa e retornará um objeto de resposta com o número do cartão de crédito. E o outro endpoint deverá listar, em ordem de criação, todos os cartões de crédito virtuais de um solicitante (passando seu e-mail como parâmetro).

### Criando o projeto

Nossa API REST contém basicamente uma classe que representa uma informação e um controlador para tratar as requisições e respostas realizadas pela API, além de possuir classes para a inicialização e execução do projeto. Ao executar o comando abaixo no terminal, é criado uma aplicação mínima utilizando uma estrutura pronta para APIs :

```powershell
dotnet new webAPI -o CreditCardGenerator.Api
```

Já na pasta do projeto, o comando a seguir executa a aplicação que pode ser acessada na rota https://localhost:5001/WeatherForecast :

```powershell
dotnet run
```

Observe que o endpoint acessado retorna um json de resposta, mostrando uma lista de previsão do tempo:

```json
[
   {
      "date":"2021-05-29T14:03:02.5971866-03:00",
      "temperatureC":45,
      "temperatureF":112,
      "summary":"Scorching"
   },
   {
      "date":"2021-05-30T14:03:02.5976139-03:00",
      "temperatureC":34,
      "temperatureF":93,
      "summary":"Mild"
   },
    ...
]
```

*Obs.: Para finalizar a execução basta digitar Ctrl + C no terminal.*

Eu estaria somente sendo repetitivo se continuasse com o tutorial nesse post, então peço que siga o tutorial da própria Microsoft [aqui](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-web-API?view=aspnetcore-3.1&tabs=visual-studio-code). Seguindo esse tutorial corretamente já temos a base necessária para criar nossa API. Lembre-se de remover as classes *WeatherForecast* e a *WeatherForecastController*, pois não vamos precisar delas.

Após o tutorial, você deverá ter:

- Criado um projeto de API Web.
- Adicionado uma classe de modelo e um contexto de banco de dados.
- Realizado um scaffold de um controlador com métodos CRUD.
- Configurado o roteamento, os caminhos de URL e os valores retornados.
- Realizado testes da API Web com o Postman.

Utilizaremos também o Entity Framework. É ele que permite o acesso ao banco de dados usando um modelo. Um modelo é criado a partir das classes de entidade e um objeto de contexto que representa uma sessão com o banco de dados. Assim podemos consultar e salvar dados. É possível ainda utilizar de migrações para criar tabelas com base no modelo, possibilitando a evolução do banco de dados conforme o modelo muda. Para nosso caso estou usando um único modelo, apenas representativo e para atender a necessidade da API. Em casos mais elaborados, o correto é definir bem as classes de modelo, mantendo nelas apenas as informações pertinentes à ela. Quanto ao EF então, vou usar o banco de dados em mémoria para facilitar o desenvolvimento.

Com a aplicação criada vamos dar continuidade ao nosso projeto. Como dito antes, nossa API é um serviço que irá gerar números aleatórios de cartão de crédito. Vamos aproveitar então e editar a classe *TodoItem* do último tutorial substituindo pela nossa classe de cartão de crédito:

```c#
using System;

namespace CreditCardGenerator.Api.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
```

*Obs.: Lembre-se de renomear também os arquivos de classes daqui pra frente, mantendo os namespaces para evitar erros.* 

A model acima representa as informações que precisamos para gerar um número de cartão de crédito, após ser enviado uma solicitação através do e-mail. Um Id do tipo *Guid* é gerado para que possamos garantir que cada registro é único e para que nossa rota não informe mais do que necessário, como por exemplo a quantidade de registros que o banco provavelmente possui se considerado o id passado para a requisição. Altere também a classe *TodoItemDTO* com as mesmas informações da *CreditCard*. O construtor da *CreditCardDTO* não é necessário.

É necessário agora atualizar também nossa classe de contexto, *TodoContext* para *ApiDbContext*. Com isso poderemos acessar o banco de dados corretamente. Utilizando o Visual Studio ou o VSCode, é possível renomear as classes e atualizar todas as suas referências. Caso não seja possível devido ao editor de texto, é preciso atualizar a classe Startup no seguinte ponto do código:

```c#
		// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(opt =>
               opt.UseInMemoryDatabase("CreditCardGenerator"));

            services.AddControllers();
        }
```

Como dito anteriormente, para gerar números de cartão de crédito válidos, utilizamos o algoritmo de Luhn. Entendendo como o algoritmo funciona, podemos realizar a implementação da lógica para geração dos números. Como deve ter notado, vou me abster de implementar a geração de um CVV (*Card Verification Value*) e de uma data de validade. Vamos acrescentar nossa implementação em um método junto à model *CreditCard*:

```c#
    public void Generate()
    {
        Random random = new Random();

        //MasterCard
        var iin = $"{random.Next(510000, 559999):000000}";
        var account = $"{random.Next(0, 999999999):000000000}";

        string sequence = new string(($"{iin}{account}").Reverse().ToArray());
        var length = sequence.Length;

        int soma = 0;
        for (int i = 0; i < length; i++)
        {
            var digito = Int32.Parse(sequence[i].ToString());
            if ((i + 1) % 2 > 0)
            {
                if ((digito *= 2) > 9)
                {
                    digito -= 9;
                }
            }

            soma = soma + digito;
        }

        var verfication = (10 - (soma % 10 != 0 ? soma % 10 : 10)).ToString();

        this.CardNumber = $"{iin}{account}{verfication}";
    }
```

*Obs.: À titulo de exemplo, utilizei IIN (Issuer Identification Number) do MasterCard. Mais detalhes [aqui](https://cleilsontechinfo.netlify.app/jekyll/update/2019/12/08/um-guia-completo-para-validar-e-formatar-cartoes-de-crédito.html).*

Nesse momento precisamos atualizar nosso controlador para que realize a chamada do contexto de banco de dados corretamente e para que possa recuperar/gravar nossas informações. O fluxo para acessarmos nossa funcionalidade de geração dos números do cartão de crédito é o seguinte:

- O usuário informa apenas o e-mail no corpo da solicitação para gerar os dados;
- As informações são submetidas através de um POST para o recurso /api/creditcards;
- A API gera o número do cartão de crédito, grava as informações no banco e retorna o objeto criado;
- O usuário pode agora visualizar todos os cartões gerados realizando uma solicitação GET para o recurso /api/credicards, informando também seu e-mail.

Aproveitando *TodoItemsController*, vamos criar nossas ações baseado nos métodos *GetTodoItem*s, *CreateTodoItem* e o *ItemToDTO*. Vamos renomear o arquivo e classe para *CreditCardController*: 

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CreditCardGenerator.Api.Models;

namespace CreditCardGenerator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public CreditCardsController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<IEnumerable<CreditCardDTO>>> GetCreditCards(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }
            
            var creditCard = _context.CreditCards
                .Where(x => x.Email == email)
                .OrderByDescending(x => x.CreatedAt);

            return await creditCard.Select(x => CreditCardToDTO(x)).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<CreditCardDTO>> CreateCreditCard(CreditCardDTO todoItemDTO)
        {
            var creditCard = new CreditCard
            {
                Email = todoItemDTO.Email,
                CreatedAt = DateTime.Now
            };

            _context.CreditCards.Add(creditCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCreditCards),
                new { email = creditCard.Email },
                CreditCardToDTO(creditCard));
        }
        
        private static CreditCardDTO CreditCardToDTO(CreditCard creditCard) =>
            new CreditCardDTO
            {
                Id = creditCard.Id,
                CardNumber = creditCard.CardNumber,
                CreatedAt = creditCard.CreatedAt
            };  
    }
}
```

Note que estamos filtrando os resultados para obter os registros por e-mail, com ordenação decrescente, para listar primeiramente os últimos registros criados e obtermos um número de cartão mais recente. Ao rodar a aplicação novamente, você poderá realizar um POST, utilizando o Postman por exemplo, informando o e-mail no corpo da requisição:

```http
POST https://localhost:5001/api/creditcards HTTP/1.1
content-type: application/json

{
    "email" : "example@teste.com"
}
```

E recuperar as informações acessando:

```http
GET https://localhost:5001/api/creditcards/example@teste.com
```

Agora só precisamos chamar o nosso gerador para criar o número de cartão:

```c#
    [HttpPost]
    public async Task<ActionResult<CreditCardDTO>> CreateCreditCard(CreditCardDTO todoItemDTO)
    {
        var creditCard = new CreditCard
        {
            Email = todoItemDTO.Email,
            CreatedAt = DateTime.Now
        };

        creditCard.Generate();

        _context.CreditCards.Add(creditCard);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCreditCards),
            new { email = creditCard.Email },
            CreditCardToDTO(creditCard));
    }
```

Acessando o método POST novamente, conseguimos recuperar um número de cartão válido:

```http
HTTP/1.1 201 Created
Connection: close
Date: Fri, 28 May 2021 20:21:59 GMT
Content-Type: application/json; charset=utf-8
Server: Kestrel
Transfer-Encoding: chunked
Location: https://localhost:5001/api/CreditCards/example@test.com

{
  "id": "74d3fb35-7fa0-48a8-8b07-e795329d1868",
  "email": null,
  "cardNumber": "5396462764596006",
  "createdAt": "2021-05-28T17:21:59.8341558-03:00"
}
```



## Conclusão

Já vou começar a conclusão dizendo que o tutorial é muito simples, se comparado a aplicações que possuem diversos endpoints e uma maior complexidade. Porém, cumpre seu papel e sua funcionalidade. A API de busca por CEP, por exemplo, aparentemente não precisa ser muito elaborada e utilizamos ela sem muito esforço até hoje. Lembrando que o passo a passo aqui é somente para tratarmos da construção de uma API, e que existem diversos cuidados que devem ser tomas durante o desenvolvimento. Seguindo o fluxo de requisição e resposta, vou fazer umas observações sobre APIs REST:

- O usuário informa apenas o e-mail no corpo da solicitação para gerar os dados;

  A aplicação deve receber apenas entradas válidas. A API deve fazer a verificação antes mesmo de acessar qualquer camada mais interna, retornando o mais rápido possível e informando o tipo de entrada correta;

- As informações são submetidas através de um POST para o recurso /api/creditcards;

  Os métodos HTTP devem ser utilizados corretamente. Não precisamos sempre criar CRUDs para acessar os recursos, devendo apenas haver os endpoints necessários para o funcionamento da aplicação;

- A API gera o número do cartão de crédito, grava as informações no banco e retorna o objeto criado;

  Mais internamente as regras de negócio devem estar bem definidas. Utilizando de recursos já consolidados como criar um camada *Service* para definir as regras de  negócio é mais apropriado. Já se tratando dos dados, pode se criar uma camada *Repository* para isolar o acesso aos dados e manter a aplicação mais organizada;

- O usuário pode agora visualizar todos os cartões gerados realizando uma solicitação GET para o recurso /api/credicards, informando também seu e-mail.

  Uma API REST deve possuir uma documentação bem definida. Utilizando de ferramentas como o *Swagger*, podemos criar documentações que irão ajudar as equipes de desenvolvimento a se entenderem, onde equipes de frontend podem acessar os endpoints facilmente e consumir os recursos disponíveis corretamente. Lembrando também que devemos nos atentar bem aos [códigos de status de respostas HTTP](https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Status).

Complementando a aplicação desenvolvida nesse post, tentei seguir as recomendações descritas acima para melhorar a aplicação. Os código da aplicação completa estão disponíveis no [github](https://github.com/andy-silv4/CreditCardGenerator).

Agora sim concluindo, podemos ver que é "simples" criar uma API REST, mas também observamos que o desenvolvimento de uma API tem muitos pontos a serem analisados. As APIs REST hoje são comumente utilizadas em arquiteturas de microsserviços, sendo essencial que a aplicação seja desenvolvida pensando sempre em qualidade, que seja escalável, passível de manutenção e de testes.


Então, é isso! Até a próxima! 
