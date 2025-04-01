# WebAPI

## Descrição
Esta é a API backend de um sistema de e-commerce desenvolvido em C# com .NET. O sistema permite gestão de categorias, produtos e usuários, utilizando MongoDB como banco de dados e autenticação via Bearer Token.

## Tecnologias Utilizadas
- .NET 8 (ou versão utilizada)
- MongoDB
- Swagger para documentação da API
- Autenticação via Bearer Token (JWT)

## Instalação e Configuração
1. Clone o repositório:
   ```sh
   git clone https://github.com/Louiexz/WebAPI.git
   ```
2. Navegue até o diretório do projeto:
   ```sh
   cd WebAPI
   ```
3. Configure as variáveis de ambiente no arquivo `appsettings.json`:
   ```json
   {
     "DatabaseSettings": {
       "ConnectionString": "mongodb://localhost:27017",
       "DatabaseName": "ecommerce_db"
     },
     "Jwt": {
       "Key": "sua-chave-secreta",
       "Issuer": "seu-dominio.com",
       "Audience": "seu-dominio.com"
     }
   }
   ```
4. Execute a aplicação:
   ```sh
   dotnet run
   ```

## Endpoints Principais
### Autenticação
- `POST /api/auth/login`: Autentica um usuário e retorna um token JWT.
- `POST /api/auth/register`: Registra um novo usuário.

### Categorias
- `GET /api/GetCategories`: Lista todas as categorias.
- `GET /api/GetCategory/{id}`: Obtém detalhes de uma categoria.
- `POST /api/CreateCategory`: Cria uma nova categoria (admin).
- `PATCH /api/UpdateCategory/{id}`: Atualiza uma categoria (admin).
- `DELETE /api/DeleteCategory/{id}`: Remove uma categoria (admin).

### Produtos
- `GET /api/GetProducts`: Lista todos os produtos.
- `GET /api/GetProduct/{id}`: Obtém detalhes de um produto.
- `POST /api/CreateProduct`: Cria um novo produto (admin).
- `PATCH /api/UpdateProduct/{id}`: Atualiza um produto (admin).
- `DELETE /api/DeleteProduct/{id}`: Remove um produto (admin).

### Usuários
- `POST /api/SignIn`: Autenticar usuário.
- `POST /api/SignUp`: Cadastrar usuário.
- `GET /api/GetUser/{id}`: Obtém detalhes de um usuário.
- `PATCH /api/UpdateUser/{id}`: Atualiza um usuário.
- `DELETE /api/DeleteUser/{id}`: Remove um usuário (admin).

## Testando a API
- Acesse a documentação interativa via Swagger:
  ```sh
  http://localhost:5000/swagger
  ```
- Utilize o Postman para testar os endpoints manualmente.

## Contribuição
1. Fork o repositório.
2. Crie um branch para sua funcionalidade (`git checkout -b minha-feature`).
3. Commit suas alterações (`git commit -m 'Adiciona nova funcionalidade'`).
4. Push para o branch (`git push origin minha-feature`).
5. Abra um Pull Request.
