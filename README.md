# EmployeePerformanceApp (упрощённая версия)

Простое WinForms-приложение для оценки KPI сотрудников по задачам.

## Что изменено

Теперь приложение стало проще и удобнее:

- Ввод только:
  - ФИО сотрудника
  - Выполнено задач
  - Не выполнено задач
- KPI считается автоматически:

`KPI = Выполнено / (Выполнено + Не выполнено) × 100`

- Автоматически формируется анализ:
  - `>= 90` — Отличный результат
  - `>= 75` — Хороший результат
  - `>= 60` — Средний результат
  - `< 60` — Низкий результат, требуется внимание

- В статистике отображается:
  - средний KPI по всем сотрудникам
  - лучший сотрудник
  - сколько сотрудников имеют KPI ниже 60%


## Как решить конфликт в PR (MainForm.cs, Model, Service, README)

Если GitHub показывает конфликт, как на скриншоте, проще всего решить его локально и запушить обратно в ту же ветку PR.

### Шаги

1. Переключитесь на ветку вашего PR:

```bash
git checkout <ВАША_ВЕТКА_PR>
```

2. Подтяните целевую ветку (обычно `main`) и выполните merge:

```bash
git fetch origin
git merge origin/main
```

3. Если появятся конфликты в:
   - `MainForm.cs`
   - `Models/EmployeeEvaluation.cs`
   - `Services/PerformanceCalculator.cs`
   - `README.md`

   оставьте **упрощённую KPI-версию** (с полями: ФИО, выполненные, невыполненные задачи),
   удалите маркеры `<<<<<<<`, `=======`, `>>>>>>>`, затем сохраните файлы.

4. Отметьте файлы как решённые и завершите merge:

```bash
git add MainForm.cs Models/EmployeeEvaluation.cs Services/PerformanceCalculator.cs README.md
git commit -m "Resolve merge conflicts and keep simplified KPI flow"
```

5. Отправьте изменения:

```bash
git push origin <ВАША_ВЕТКА_PR>
```

После push GitHub автоматически обновит PR, и кнопка Merge станет активной.

### Если хотите решить конфликт одной командой (оставить свою версию)

> Используйте этот вариант, только если уверены, что в вашей ветке уже правильная версия файлов.

```bash
git checkout --ours MainForm.cs Models/EmployeeEvaluation.cs Services/PerformanceCalculator.cs README.md
git add MainForm.cs Models/EmployeeEvaluation.cs Services/PerformanceCalculator.cs README.md
git commit -m "Resolve conflicts by keeping PR version of KPI app"
git push origin <ВАША_ВЕТКА_PR>
```

## Как скачать проект

### Вариант A: ZIP с GitHub

1. Откройте страницу репозитория.
2. Нажмите `Code` → `Download ZIP`.
3. Распакуйте архив в папку, например `C:\Projects\deepMO`.

### Вариант B: через Git

```bash
git clone <URL_РЕПОЗИТОРИЯ>
cd deepMO
```

## Как открыть и запустить в Visual Studio

1. Установите **Visual Studio 2022+**.
2. При установке выберите workload: **Desktop development with .NET**.
3. Откройте проект:
   - `File` → `Open` → `Project/Solution` → `EmployeePerformanceApp.csproj`
4. Запустите:
   - `F5` (с отладкой) или `Ctrl+F5` (без отладки)

## Файлы проекта

- `Program.cs` — запуск приложения
- `MainForm.cs` — форма и логика UI
- `Models/EmployeeEvaluation.cs` — модель данных сотрудника
- `Services/PerformanceCalculator.cs` — расчёт KPI и текстового анализа
- `EmployeePerformanceApp.csproj` — настройки WinForms
