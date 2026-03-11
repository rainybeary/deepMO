using EmployeePerformanceApp.Models;
using EmployeePerformanceApp.Services;

namespace EmployeePerformanceApp;

public sealed class MainForm : Form
{
    private readonly TextBox _fullNameTextBox = new();
    private readonly NumericUpDown _completedTasksInput = new();
    private readonly NumericUpDown _notCompletedTasksInput = new();
    private readonly Label _statsLabel = new();
    private readonly DataGridView _grid = new();

    private readonly BindingSource _bindingSource = new();
    private readonly List<EmployeeEvaluation> _evaluations = [];

    public MainForm()
    {
        Text = "Простая оценка KPI сотрудников";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 900;
        Height = 600;

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(12)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        root.Controls.Add(BuildInputPanel(), 0, 0);
        root.Controls.Add(BuildGrid(), 0, 1);
        root.Controls.Add(BuildStatsPanel(), 0, 2);

        Controls.Add(root);
        RefreshStats();
    }

    private Control BuildInputPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 6,
            RowCount = 3,
            Padding = new Padding(0, 0, 0, 10)
        };

        panel.Controls.Add(new Label { Text = "ФИО", AutoSize = true }, 0, 0);
        panel.Controls.Add(_fullNameTextBox, 1, 0);
        panel.SetColumnSpan(_fullNameTextBox, 2);

        ConfigureTaskInput(_completedTasksInput, "Выполнено задач", panel, 0, 1);
        ConfigureTaskInput(_notCompletedTasksInput, "Не выполнено задач", panel, 3, 1);

        var addButton = new Button
        {
            Text = "Добавить",
            AutoSize = true,
            Padding = new Padding(8)
        };
        addButton.Click += (_, _) => AddEvaluation();

        var resetButton = new Button
        {
            Text = "Очистить",
            AutoSize = true,
            Padding = new Padding(8)
        };
        resetButton.Click += (_, _) => ClearInputs();

        panel.Controls.Add(addButton, 0, 2);
        panel.Controls.Add(resetButton, 1, 2);

        var hintLabel = new Label
        {
            AutoSize = true,
            Text = "KPI рассчитывается автоматически: Выполнено / (Выполнено + Не выполнено) × 100",
            ForeColor = Color.DimGray,
            Padding = new Padding(10, 10, 0, 0)
        };
        panel.Controls.Add(hintLabel, 2, 2);
        panel.SetColumnSpan(hintLabel, 4);

        return panel;
    }

    private static void ConfigureTaskInput(NumericUpDown input, string caption, TableLayoutPanel panel, int column, int row)
    {
        input.Minimum = 0;
        input.Maximum = 100000;
        input.DecimalPlaces = 0;
        input.Width = 120;

        panel.Controls.Add(new Label { Text = caption, AutoSize = true }, column, row);
        panel.Controls.Add(input, column + 1, row);
    }

    private Control BuildGrid()
    {
        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AutoGenerateColumns = true;
        _grid.DataSource = _bindingSource;

        _bindingSource.DataSource = _evaluations;
        return _grid;
    }

    private Control BuildStatsPanel()
    {
        _statsLabel.AutoSize = true;
        _statsLabel.Font = new Font(Font, FontStyle.Bold);
        _statsLabel.Padding = new Padding(0, 10, 0, 0);
        return _statsLabel;
    }

    private void AddEvaluation()
    {
        if (string.IsNullOrWhiteSpace(_fullNameTextBox.Text))
        {
            MessageBox.Show("Введите ФИО сотрудника.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var completed = (int)_completedTasksInput.Value;
        var notCompleted = (int)_notCompletedTasksInput.Value;

        if (completed + notCompleted == 0)
        {
            MessageBox.Show("Введите количество задач (выполненных или невыполненных).", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var kpi = PerformanceCalculator.CalculateKpi(completed, notCompleted);
        var analysis = PerformanceCalculator.GetAnalysis(kpi);

        _evaluations.Add(new EmployeeEvaluation
        {
            FullName = _fullNameTextBox.Text.Trim(),
            CompletedTasks = completed,
            NotCompletedTasks = notCompleted,
            Kpi = kpi,
            Analysis = analysis
        });

        _bindingSource.ResetBindings(false);
        RefreshStats();
        ClearInputs();
    }

    private void ClearInputs()
    {
        _fullNameTextBox.Clear();
        _completedTasksInput.Value = 0;
        _notCompletedTasksInput.Value = 0;
    }

    private void RefreshStats()
    {
        if (_evaluations.Count == 0)
        {
            _statsLabel.Text = "Статистика: данных пока нет.";
            return;
        }

        var avgKpi = _evaluations.Average(e => e.Kpi);
        var top = _evaluations.MaxBy(e => e.Kpi);
        var lowCount = _evaluations.Count(e => e.Kpi < 60m);

        _statsLabel.Text =
            $"Статистика: средний KPI = {avgKpi:0.00}%. Лучший сотрудник: {top?.FullName} ({top?.Kpi:0.00}%). " +
            $"Сотрудников с KPI ниже 60%: {lowCount}.";
    }
}
