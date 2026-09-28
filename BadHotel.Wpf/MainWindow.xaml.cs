using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BadHotel.Core;

namespace BadHotel.Wpf;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<HotelWorker> _workers = new();

    public MainWindow()
    {
        InitializeComponent();
        WorkersListBox.ItemsSource = _workers;
    }

    private void AddWorkerButton_Click(object sender, RoutedEventArgs e)
    {
        string name = NameTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show(
                "Please enter a worker name.",
                "Input error",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (WorkerTypeComboBox.SelectedItem is not ComboBoxItem selectedItem)
        {
            MessageBox.Show(
                "Please select a worker type.",
                "Input error",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        HotelWorker worker;

        switch (selectedItem.Content?.ToString())
        {
            case "Receptionist":
                worker = new Receptionist(name);
                break;

            case "Cleaner":
                worker = new Cleaner(name);
                break;

            case "Cook":
                worker = new Cook(name);
                break;

            default:
                MessageBox.Show("Unknown worker type.");
                return;
        }

        _workers.Add(worker);
        AddToLog($"{worker.Name} joined the hotel as {worker.GetType().Name}.");

        NameTextBox.Clear();
        WorkerTypeComboBox.SelectedIndex = -1;

        WorkersListBox.SelectedItem = worker;
    }

    private void WorkersListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        UpdateSelectedWorker();
    }

    private void WorkButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedWorker(out HotelWorker? worker))
            return;

        AddToLog(worker.Work());
        RefreshWorker();
    }

    private void CrazyActionButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedWorker(out HotelWorker? worker))
            return;

        AddToLog(worker.CrazyAction());
        RefreshWorker();
    }

    private void SpecialAbilityButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedWorker(out HotelWorker? worker))
            return;

        string message;

        if (worker is ICook cook)
        {
            message = cook.CookMeal();
        }
        else if (worker is IClean cleaner)
        {
            message = cleaner.CleanRoom();
        }
        else if (worker is IServe server)
        {
            message = server.ServeGuest();
        }
        else
        {
            message = $"{worker.Name} has no special ability.";
        }

        AddToLog(message);
        RefreshWorker();
    }

    private void RemoveWorkerButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryGetSelectedWorker(out HotelWorker? worker))
            return;

        _workers.Remove(worker);
        AddToLog($"{worker.Name} left the Bad Hotel.");

        UpdateSelectedWorker();
    }

    private bool TryGetSelectedWorker(out HotelWorker? worker)
    {
        worker = WorkersListBox.SelectedItem as HotelWorker;

        if (worker is not null)
            return true;

        MessageBox.Show(
            "Please select a worker first.",
            "No worker selected",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        return false;
    }

    private void RefreshWorker()
    {
        UpdateSelectedWorker();
        WorkersListBox.Items.Refresh();
    }

    private void UpdateSelectedWorker()
    {
        if (WorkersListBox.SelectedItem is HotelWorker worker)
        {
            SelectedNameText.Text = worker.Name;
            SelectedTypeText.Text = worker.GetType().Name;
            SelectedEnergyText.Text = $"{worker.Energy}/100";
            EnergyProgressBar.Value = worker.Energy;
        }
        else
        {
            SelectedNameText.Text = "No worker selected";
            SelectedTypeText.Text = "-";
            SelectedEnergyText.Text = "-";
            EnergyProgressBar.Value = 0;
        }
    }

    private void AddToLog(string message)
    {
        ActivityLogListBox.Items.Add(message);

        if (ActivityLogListBox.Items.Count > 0)
        {
            ActivityLogListBox.ScrollIntoView(
                ActivityLogListBox.Items[ActivityLogListBox.Items.Count - 1]);
        }
    }
}