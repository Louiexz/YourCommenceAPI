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
       "DatabaseName": "sua_db"
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
- `POST /api/Auth/login`: Autentica um usuário e retorna um token JWT.
- `POST /api/Auth/user`: Registra um novo usuário.
- `POST /api/Auth/logout`: Desloga o usuário.

### Usuários
- `GET /api/User/user{id}`: Obtém detalhes de um usuário.
- `PATCH /api/User/user{id}`: Atualiza um usuário.
- `DELETE /api/User/user{id}`: Remove um usuário (admin).

### Categorias
- `GET /api/Categoty/categories`: Lista todas as categorias.
- `GET /api/Category/category{id}`: Obtém detalhes de uma categoria.
- `POST /api/Category`: Cria uma nova categoria (admin).
- `PATCH /api/Category/{id}`: Atualiza uma categoria (admin).
- `DELETE /api/DeleteCategory/{id}`: Remove uma categoria (admin).

### Produtos
- `GET /api/Product/products`: Lista todos os produtos.
- `GET /api/Product/categoryproducts{id}`: Lista produtos por categoria.
- `GET /api/Product/search{product}`: Procura por produtos.
- `GET /api/Product/product{id}`: Obtém detalhes de um produto.
- `POST /api/Product/product`: Cria um novo produto (admin).
- `PATCH /api/Product/product{id}`: Atualiza um produto (admin).
- `DELETE /api/Product/product{id}`: Remove um produto (admin).

### Imagens
- `GET /api/Image/image{id}`: Obtém uma imagem pelo ID.

## Testando a API

- Utilize o Swagger para testar os endpoints diretamente:
  ```sh
  http://localhost:5000/swagger/index.html
  ```
- Para autenticação, utilize o token JWT obtido no endpoint de login.
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

Contribuições são bem-vindas! Sinta-se à vontade para abrir um Pull Request ou relatar problemas.