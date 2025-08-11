# Testes Unitários - Resumo

---

## ApiErrorResponseTests

| Teste                                  | O que o teste faz                          | Resultado esperado                      | Código HTTP (se aplicável) |
|---------------------------------------|-------------------------------------------|---------------------------------------|----------------------------|
| `Constructor_WithSingleMessage_SetsStatusAndMessage` | Constrói ApiErrorResponse com uma mensagem | Status correto e uma única mensagem    | -                          |
| `Constructor_WithMultipleMessages_SetsStatusAndMessages` | Constrói ApiErrorResponse com múltiplas mensagens | Status correto e lista com mensagens  | -                          |

---

## EnumExtensionsTests

| Teste                             | O que o teste faz                              | Resultado esperado                    | Código HTTP (se aplicável) |
|----------------------------------|-----------------------------------------------|-------------------------------------|----------------------------|
| `GetDescription_WithDescription_ReturnsDescription` | Retorna descrição do atributo Description      | Retorna a descrição correta          | -                          |
| `GetDescription_WithoutDescription_ReturnsEnumName` | Retorna nome do enum quando não há descrição   | Retorna o nome do enum               | -                          |

---

## TaskControllerTests

### GetAll Tests

| Teste                            | O que o teste faz                                | Resultado esperado              | Código HTTP |
|---------------------------------|-------------------------------------------------|-------------------------------|-------------|
| `GetAll_ShouldReturnAllTasks`   | Retorna todas as tarefas quando existem          | Lista com todas as tarefas     | 200 (OK)    |
| `GetAll_WhenEmpty_ShouldReturnEmptyList` | Retorna lista vazia se não houver tarefas       | Lista vazia                    | 200 (OK)    |
| `GetAll_WhenServiceThrows_ShouldReturnInternalServerError` | Retorna erro em caso de exceção do serviço        | Status 500 com mensagem de erro| 500         |

### GetById Tests

| Teste                              | O que o teste faz                               | Resultado esperado                  | Código HTTP |
|-----------------------------------|------------------------------------------------|-----------------------------------|-------------|
| `GetById_WhenTaskExists_ShouldReturnTask` | Retorna tarefa pelo ID quando existe           | Retorna tarefa                    | 200 (OK)    |
| `GetById_WhenTaskDoesNotExist_ShouldReturnNotFound` | Retorna NotFound se a tarefa não existir       | Status 404 com mensagem de erro   | 404         |
| `GetById_WhenServiceThrows_ShouldReturnInternalServerError` | Retorna erro em caso de exceção do serviço        | Status 500 com mensagem de erro   | 500         |

### Create Tests

| Teste                              | O que o teste faz                               | Resultado esperado                    | Código HTTP |
|-----------------------------------|------------------------------------------------|-------------------------------------|-------------|
| `Create_WithValidTask_ShouldCreateAndReturnTask` | Cria uma nova tarefa com dados válidos         | Status 201 com tarefa criada         | 201 (Created)|
| `Create_WhenValidationFails_ShouldReturnBadRequest` | Retorna BadRequest quando dados são inválidos | Status 400 com erros de validação    | 400 (Bad Request) |
| `Create_WhenServiceThrows_ShouldReturnInternalServerError` | Retorna erro em caso de exceção do serviço        | Status 500 com mensagem de erro      | 500         |

### Update Tests

| Teste                              | O que o teste faz                               | Resultado esperado                    | Código HTTP |
|-----------------------------------|------------------------------------------------|-------------------------------------|-------------|
| `Update_WhenTaskExists_ShouldUpdateAndReturnTask` | Atualiza tarefa existente                       | Status 200 com tarefa atualizada     | 200 (OK)    |
| `Update_WhenTaskDoesNotExist_ShouldReturnNotFound` | Retorna NotFound se a tarefa não existir       | Status 404 com mensagem de erro      | 404         |
| `Update_WhenServiceThrows_ShouldReturnInternalServerError` | Retorna erro em caso de exceção do serviço        | Status 500 com mensagem de erro      | 500         |

### UpdateStatus Tests

| Teste                              | O que o teste faz                               | Resultado esperado                    | Código HTTP |
|-----------------------------------|------------------------------------------------|-------------------------------------|-------------|
| `UpdateStatus_WhenTaskExists_ShouldUpdateStatus` | Atualiza status de tarefa existente             | Status 200 com tarefa atualizada     | 200 (OK)    |
| `UpdateStatus_WhenTaskDoesNotExist_ShouldReturnNotFound` | Retorna NotFound se a tarefa não existir       | Status 404 com mensagem de erro      | 404         |
| `UpdateStatus_WhenServiceThrows_ShouldReturnInternalServerError` | Retorna erro em caso de exceção do serviço        | Status 500 com mensagem de erro      | 500         |

### Delete Tests

| Teste                              | O que o teste faz                               | Resultado esperado                    | Código HTTP |
|-----------------------------------|------------------------------------------------|-------------------------------------|-------------|
| `Delete_WhenTaskExists_ShouldRemoveTask` | Remove tarefa existente                          | Status 204 NoContent                 | 204 (No Content) |
| `Delete_WhenTaskDoesNotExist_ShouldReturnNotFound` | Retorna NotFound se a tarefa não existir       | Status 404 com mensagem de erro      | 404         |
| `Delete_WhenServiceThrows_ShouldReturnInternalServerError` | Retorna erro em caso de exceção do serviço        | Status 500 com mensagem de erro      | 500         |

---

## TaskRepositoryTests

### GetAll Tests

| Teste                            | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|---------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `GetAll_WhenEmpty_ReturnsEmptyList` | Retorna lista vazia se não houver tarefas       | Lista vazia                    | -                          |
| `GetAll_WithTasks_ReturnsAllTasks` | Retorna todas as tarefas criadas                 | Lista com todas as tarefas     | -                          |
| `GetAll_AfterDelete_ReturnsRemainingTasks` | Retorna lista após exclusão de uma tarefa         | Lista com tarefas restantes    | -                          |

### GetById Tests

| Teste                              | O que o teste faz                               | Resultado esperado              | Código HTTP (se aplicável) |
|-----------------------------------|------------------------------------------------|-------------------------------|----------------------------|
| `GetById_WhenTaskExists_ReturnsTaskItem` | Retorna tarefa pelo Id                           | Retorna tarefa                 | -                          |
| `GetById_WhenTaskDoesNotExist_ReturnsNull` | Retorna null se tarefa não existe                | Retorna null                  | -                          |
| `GetById_AfterUpdate_ReturnsUpdatedTask` | Retorna tarefa atualizada após update            | Retorna tarefa atualizada     | -                          |

### Create Tests

| Teste                             | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|----------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `Create_WithEmptyTitle_ThrowsArgumentException` | Título vazio gera exceção                         | `ArgumentException`             | -                          |
| `Create_WithNullTitle_ThrowsArgumentException` | Título nulo gera exceção                           | `ArgumentException`             | -                          |
| `Create_WithWhitespaceTitle_ThrowsArgumentException` | Título com espaço gera exceção                    | `ArgumentException`             | -                          |
| `Create_AssignsIdAndSetsPendingStatus` | Cria tarefa com Id e status pendente             | Id > 0 e status pendente        | -                          |
| `Create_MultipleTasks_AssignsUniqueIds` | Cria múltiplas tarefas com Ids únicos             | Ids diferentes e crescentes     | -                          |
| `Create_WithNullDescription_SetsEmptyString` | Descrição nula salva como string vazia            | Descrição vazia                | -                          |
| `Create_PreservesInputData`        | Preserva título e descrição mesmo com status modificado | Status sempre pendente          | -                          |

### Update Tests

| Teste                              | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|-----------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `Update_WithEmptyTitle_ThrowsArgumentException` | Título vazio no update gera exceção               | `ArgumentException`             | -                          |
| `Update_WithNullTitle_ThrowsArgumentException` | Título nulo no update gera exceção                 | `ArgumentException`             | -                          |
| `Update_WithWhitespaceTitle_ThrowsArgumentException` | Título espaço no update gera exceção              | `ArgumentException`             | -                          |
| `Update_WhenTaskExists_UpdatesAndReturnsTaskItem` | Atualiza tarefa e retorna tarefa atualizada       | Tarefa atualizada              | -                          |
| `Update_WhenTaskDoesNotExist_ThrowsKeyNotFoundException` | Update de tarefa inexistente gera exceção          | `KeyNotFoundException`          | -                          |
| `Update_PreservesId`               | Confirma que Id da tarefa não é modificado       | Id mantido                    | -                          |

### UpdateStatus Tests

| Teste                             | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|----------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `UpdateStatus_WhenTaskExists_UpdatesStatus` | Atualiza status de tarefa existente              | Status atualizado              | -                          |
| `UpdateStatus_WhenTaskDoesNotExist_DoesNotThrow` | Não lança exceção para tarefa inexistente         | Nenhuma exceção               | -                          |
| `UpdateStatus_OnlyChangesStatus` | Atualiza somente o status da tarefa               | Apenas status alterado        | -                          |
| `UpdateStatus_CanChangeMultipleTimes` | Atualiza status diversas vezes                     | Status atualizado várias vezes | -                          |

### Delete Tests

| Teste                            | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|---------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `Delete_WhenTaskExists_RemovesTask` | Remove tarefa existente                           | Tarefa removida               | -                          |
| `Delete_WhenTaskDoesNotExist_DoesNothing` | Deletar tarefa inexistente não gera exceção       | Nenhuma exceção               | -                          |
| `Delete_RemovesOnlySpecifiedTask` | Remove apenas a tarefa especificada               | Apenas tarefa alvo removida   | -                          |

### Integration Test

| Teste                               | O que o teste faz                               | Resultado esperado              | Código HTTP (se aplicável) |
|------------------------------------|------------------------------------------------|-------------------------------|----------------------------|
| `FullLifecycle_CreateUpdateStatusDelete_WorksAsExpected` | Cria, atualiza status, atualiza conteúdo e deleta tarefa | Todas operações funcionam corretamente | -                          |

---

## AuthServiceTests

| Teste                                | O que o teste faz                                | Resultado esperado             | Código HTTP (se aplicável) |
|-------------------------------------|-------------------------------------------------|------------------------------|----------------------------|
| `ValidateUser_WithValidCredentials_ReturnsUser` | Valida usuário com credenciais corretas          | Retorna usuário               | -                          |
| `ValidateUser_WithInvalidEmail_ReturnsNull` | Valida usuário com email inválido                  | Retorna null                 | -                          |
| `ValidateUser_WithInvalidPassword_ReturnsNull` | Valida usuário com senha inválida                  | Retorna null                 | -                          |

---

## TaskServiceTests

### GetAll Tests

| Teste                              | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|-----------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `GetAll_ShouldReturnAllTasks`       | Retorna todas tarefas                            | Lista com tarefas             | -                          |
| `GetAll_WhenEmpty_ShouldReturnEmptyList` | Retorna lista vazia se não houver tarefas       | Lista vazia                  | -                          |
| `GetAll_WhenRepositoryThrows_ShouldPropagateException` | Propaga exceção do repositório                   | Lança exceção                | -                          |

### GetById Tests

| Teste                              | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|-----------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `GetById_WhenTaskExists_ShouldReturnTask` | Retorna tarefa existente                         | Retorna tarefa               | -                          |
| `GetById_WhenTaskDoesNotExist_ShouldReturnNull` | Retorna null para tarefa inexistente              | Retorna null                | -                          |
| `GetById_WhenRepositoryThrows_ShouldPropagateException` | Propaga exceção do repositório                   | Lança exceção                | -                          |

### Create Tests

| Teste                              | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|-----------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `Create_WithValidTask_ShouldCreateAndReturnTask` | Cria tarefa válida                              | Retorna tarefa criada        | -                          |
| `Create_WithNullDescription_ShouldCreateWithEmptyDescription` | Cria tarefa com descrição nula                  | Descrição salva vazia        | -                          |
| `Create_WithInvalidTitle_ShouldThrowArgumentException` | Cria tarefa com título inválido                 | Lança `ArgumentException`    | -                          |

### Update Tests

| Teste                              | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|-----------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `Update_WithValidTask_ShouldUpdateAndReturnTask` | Atualiza tarefa válida                          | Retorna tarefa atualizada     | -                          |
| `Update_WithInvalidId_ShouldThrowKeyNotFoundException` | Atualiza tarefa inexistente                      | Lança `KeyNotFoundException`  | -                          |
| `Update_WithInvalidTitle_ShouldThrowArgumentException` | Atualiza tarefa com título inválido             | Lança `ArgumentException`     | -                          |
| `Update_WithNullDescription_ShouldUpdateWithEmptyDescription` | Atualiza tarefa com descrição nula              | Descrição salva vazia         | -                          |

### UpdateStatus Tests

| Teste                              | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|-----------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `UpdateStatus_WhenTaskExists_ShouldUpdateStatus` | Atualiza status de tarefa existente             | Chamada confirmada            | -                          |
| `UpdateStatus_WhenTaskDoesNotExist_ShouldNotThrow` | Não lança exceção para tarefa inexistente        | Nenhuma exceção               | -                          |
| `UpdateStatus_WithSameStatus_ShouldUpdateSuccessfully` | Atualiza status para mesmo valor                 | Chamada confirmada            | -                          |

### Delete Tests

| Teste                              | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|-----------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `Delete_WhenTaskExists_ShouldDeleteTask` | Deleta tarefa existente                          | Chamada confirmada            | -                          |
| `Delete_WhenTaskDoesNotExist_ShouldNotThrow` | Não lança exceção para tarefa inexistente        | Nenhuma exceção               | -                          |
| `Delete_WhenRepositoryThrows_ShouldPropagateException` | Propaga exceção ao deletar tarefa                | Lança exceção                | -                          |

---

## UserServiceTests

| Teste                                  | O que o teste faz                                | Resultado esperado              | Código HTTP (se aplicável) |
|---------------------------------------|-------------------------------------------------|-------------------------------|----------------------------|
| `CreateUser_WithValidData_CreatesUser` | Cria usuário com dados válidos                    | Usuário criado com dados corretos | -                          |
| `CreateUser_WithDuplicateEmail_ThrowsException` | Cria usuário com email duplicado                   | Lança exceção                  | -                          |
| `GetByEmail_WithExistingEmail_ReturnsUser` | Retorna usuário pelo email                         | Usuário correspondente         | -                          |
| `GetByEmail_WithNonExistentEmail_ReturnsNull` | Retorna null para email não cadastrado              | Retorna null                  | -                          |
| `GetAll_ReturnsAllUsers`                | Retorna todos os usuários cadastrados             | Lista de usuários             | -                          |

