# TaskFlow

TaskFlow est une application Windows de gestion de tâches (To-Do List) inspirée de Notion et Todoist.

## Stack technique
- C# / .NET 8
- WPF (XAML)
- Architecture MVVM
- SQLite + Entity Framework Core
- Injection de dépendances via Generic Host

## Structure du projet

```text
/TaskFlow
  /Converters
  /Database
  /Helpers
  /Models
  /Resources
  /Services
  /ViewModels
  /Views
TaskFlow.sln
```

## Fonctionnalités implémentées
- CRUD des tâches
- Statut (En cours / Terminée)
- Priorité (Basse / Moyenne / Haute)
- Date d'échéance et description
- Projets
- Sous-tâches (modèle de données)
- Recherche texte
- Filtres (Toutes, Aujourd'hui, À venir, Terminées)
- Réorganisation par glisser-déposer dans la liste
- Mode sombre / clair
- Notifications Windows (toast)
- Raccourcis clavier (`Ctrl+N`, `Ctrl+D`, `Suppr`)
- Barre de progression des tâches terminées
- Sauvegarde automatique via persistance EF Core

## Ouvrir dans Visual Studio
1. Installer **Visual Studio 2022** (17.8+) avec la charge de travail **.NET Desktop Development**.
2. Installer le SDK **.NET 8**.
3. Ouvrir `TaskFlow.sln`.
4. Restaurer les packages NuGet si demandé.

## Compiler et lancer
Dans un terminal développeur Windows (PowerShell):

```powershell
dotnet restore
dotnet build TaskFlow.sln -c Debug
dotnet run --project .\TaskFlow\TaskFlow.csproj
```

## Générer un exécutable Windows

```powershell
dotnet publish .\TaskFlow\TaskFlow.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Le binaire sera généré dans:
`TaskFlow\bin\Release\net8.0-windows\win-x64\publish\TaskFlow.exe`
