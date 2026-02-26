# 🧠 Fokus

Fokus é um aplicativo de lista de tarefas (To-Do List) desenvolvido em WPF com C#, utilizando o padrão de arquitetura MVVM. O projeto está sendo criado como estudo e também como parte do meu portfólio de desenvolvimento desktop.

---

## 📌 Objetivo do projeto

Criar um gerenciador de tarefas que permita:

- Organizar atividades com título, descrição e checklists
- Acompanhar progresso por item
- Trabalhar com estados e prioridades
- Ter uma interface clara e agradável

---

## 🛠️ O que foi feito até agora

### 📁 Estrutura do projeto

Separação em camadas:

- `Views`
- `ViewModels`
- `Models`
- `Dictionaries`
- `DataService`

### 🎨 Interface (UI)

- Janela principal (`MainWindow`)
- Janela de criação de tarefas (`NewTaskWindow`) com:
  - Campo de título com placeholder animado
  - Campo de descrição multilinha com placeholder animado
  - Checklist dinâmico com itens adicionáveis e removíveis
  - ComboBox de importância com menu de contexto
- `ResourceDictionaries` para cores, estilos e ícones

### 📦 Models

- `Task` — entidade principal com título, descrição, datas, estado, importância, categoria e timer
- `TaskCheckList` — item de checklist com descrição e status de conclusão, com suporte a `INotifyPropertyChanged`
- Enums: `TaskState`, `TaskImportance`, `TaskCategory`

### 🧩 MVVM

- `MainWindowViewModel` — controla a janela principal e abertura da `NewTaskWindow`
- `NewTaskWindowViewModel` — controla criação de tarefas com:
  - Lista reativa de checklists via `ObservableCollection`
  - Comandos: adicionar item, remover item, remover selecionados, selecionar acima, selecionar abaixo, selecionar todos
  - Binding do enum `TaskImportance` no ComboBox
- `RelayCommand` e `RelayCommand<T>` — implementações de `ICommand` para comandos com e sem parâmetro

---

## 🚧 Status do projeto

⚠️ **Em desenvolvimento**

Funcionalidades planejadas ainda não implementadas:

- Persistência de dados (arquivo ou banco)
- Atualização de status das tarefas
- Listagem e gerenciamento de tarefas na tela principal

---

## 🧪 Tecnologias utilizadas

- C#
- WPF (.NET)
- MVVM
- XAML

---

## 📅 Próximos passos (planejado)

- Implementar persistência (arquivo ou banco de dados)
- Listar tarefas criadas na tela principal
- Trabalhar estados e filtros das tarefas
- Refinar layout e estilos
