# Feedback - Avaliação Geral

## Front End
### Navegação
  * Pontos positivos:
    - Nenhum ponto positivo identificado, pois o projeto MVC está apenas com template padrão sem implementações

  * Pontos negativos:
    - Ausência de implementação das views e controllers necessárias
    - Não há rotas definidas para as funcionalidades básicas do sistema
    - Template MVC padrão sem customizações ou implementações dos casos de uso

### Design
 - Será avaliado na entrega final

### Funcionalidade
  * Pontos positivos:
    - Nenhum ponto positivo identificado, pois não há implementação das funcionalidades

  * Pontos negativos:
    - Ausência total de implementação dos casos de uso no front-end
    - Não há implementação das funcionalidades de gestão de produtos e categorias

## Back End
### Arquitetura
  * Pontos positivos:
    - Nenhum ponto positivo identificado

  * Pontos negativos:
    - Arquitetura incoerente e com camadas desnecessárias (IOC, Data e Domain separados)
    - Separar a camada IoC independente não faz sentido
    - Separação desnecessária entre Data e Domain que poderiam estar unificadas em uma camada Core
    - Uso de complexidade arquitetural desnecessária para um CRUD básico

### Funcionalidade

  * Pontos negativos:
    - API sem implementação do Identity
    - Ausência de SQLite configurado
    - Falta de implementação de migrations e seed de dados
    - Controllers vazios sem implementação dos casos de uso

### Modelagem
  * Pontos positivos:
    - Entidades anemicas básicas mas de acordo com a complexidade

  * Pontos negativos:
    - Ausência de implementação das entidades necessárias

## Projeto
### Organização
  * Pontos positivos:
    - Presença do arquivo solution na raiz
    - Uso da pasta src na raiz do projeto

  * Pontos negativos:
    - Estrutura de pastas complexa e desnecessária
    - Separação excessiva em projetos que poderiam ser unificados

### Documentação
  * Pontos positivos:
    - Presença do README.MD com informações do projeto
    - Arquivo FEEDBACK.MD presente no repositório

  * Pontos negativos:
    - Documentação desalinhada com a implementação real
    - Ausência de documentação da API via Swagger
    - Falta de instruções claras para execução do projeto

### Instalação

  * Pontos negativos:
    - Ausência de implementação do SQLite
    - Falta de configuração de migrations automáticas
    - Ausência de seed de dados inicial
