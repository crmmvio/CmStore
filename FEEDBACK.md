# Feedback - Avaliação Geral

## Front End

### Navegação
  * Pontos positivos:
    - Projeto MVC com views e controllers bem definidos para autenticação, produtos e categorias.
    - Funcionalidade de navegação completa.

  * Pontos negativos:
    - Nenhum.

### Design
  - Interface simples, clara e bem adaptada para uso administrativo.

### Funcionalidade
  * Pontos positivos:
    - CRUD completo para produtos e categorias implementado em ambas as camadas (MVC e API).
    - Identity implementado corretamente, com autenticação via JWT na API e Cookie no MVC.
    - A criação do vendedor é feita junto ao registro do usuário no MVC com o compartilhamento do mesmo ID.
    - Seed de dados e migrations automáticas com SQLite estão presentes e funcionais.
    - Modelagem enxuta e compatível com o escopo do projeto.

  * Pontos negativos:
    - Falta verificação se o vendedor logado é o dono do produto durante edição ou exclusão, o que compromete a segurança.

## Back End

### Arquitetura
  * Pontos positivos:
    - Arquitetura bem definida com três camadas: API, Core e MVC (Ui).
    - Uso correto de dependência entre projetos e estrutura de camadas clara.

  * Pontos negativos:
    - A lógica de acesso a dados é repetida entre API e MVC, pois ambas usam diretamente o contexto. Poderiam ser implementados repositórios no `Core` para evitar duplicação de lógica.
    - `JwtSettings` está na mesma pasta que os modelos, idealmente deveria estar em pasta separada de configurações.

### Funcionalidade
  * Pontos positivos:
    - Funcionalidade de autenticação e rotas protegidas bem implementada.
    - CRUDs operam corretamente nas duas camadas.

  * Pontos negativos:
    - Falta segurança no domínio ao não validar o vínculo do produto com o vendedor autenticado.

### Modelagem
  * Pontos positivos:
    - Entidades bem modeladas, simples e de fácil manutenção.
    - Associação correta entre Produto, Categoria e Vendedor.

  * Pontos negativos:
    - Nenhum.

## Projeto

### Organização
  * Pontos positivos:
    - Projeto organizado com `src`, solution `.sln` na raiz, e separação clara entre projetos.
    - README.md e FEEDBACK.md presentes.

  * Pontos negativos:
    - Duplicação do helper de seed (`DbMigrationHelpers`) entre API e MVC.

### Documentação
  * Pontos positivos:
    - Documentação presente e bem estruturada.
    - Swagger configurado na API.

  * Pontos negativos:
    - Nenhum.

### Instalação
  * Pontos positivos:
    - Migrations automáticas e seed de dados estão corretamente configurados.
    - SQLite funcional sem dependências externas.

  * Pontos negativos:
    - Nenhum.

---

# 📊 Matriz de Avaliação de Projetos

| **Critério**                   | **Peso** | **Nota** | **Resultado Ponderado**                  |
|-------------------------------|----------|----------|------------------------------------------|
| **Funcionalidade**            | 30%      | 9        | 2,7                                      |
| **Qualidade do Código**       | 20%      | 9        | 1,8                                      |
| **Eficiência e Desempenho**   | 20%      | 8        | 1,6                                      |
| **Inovação e Diferenciais**   | 10%      | 8        | 0,8                                      |
| **Documentação e Organização**| 10%      | 8        | 0,8                                      |
| **Resolução de Feedbacks**    | 10%      | 8        | 0,8                                      |
| **Total**                     | 100%     | -        | **8,5**                                  |

## 🎯 **Nota Final: 8,5 / 10**
