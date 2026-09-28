# Bad Hotel

Bad Hotel is a humorous WPF application where different hotel workers try to do their jobs while strange things happen in the hotel.

## How to run

1. Clone the repository.
2. Open the project in Visual Studio Code or Visual Studio.
3. Build the solution:
   dotnet build BadHotel.slnx
4. Run the WPF application:
   dotnet run --project BadHotel.Wpf

## Class hierarchy

The abstract base class is `HotelWorker`.

Current subclasses:
- `Receptionist`
- `Cleaner`
- `Cook`

The project uses the interfaces:
- `IServe`
- `IClean`
- `ICook`

`Cook` implements both `ICook` and `IServe`.

All workers are stored in an `ObservableCollection<HotelWorker>`.

## Crazy Action and state

Every worker overrides `CrazyAction()` with different behavior.

Workers have an `Energy` value from 0 to 100. Actions use energy. The `TryUseEnergy()` method prevents an action from reducing energy below 0, so invalid state changes are rejected.

The WPF application calls `Work()` and `CrazyAction()` polymorphically through `HotelWorker`.

Special abilities are detected using interfaces and the `is` operator.

## Tested use cases

1. Add a Receptionist with a valid name and perform Work.
2. Add a Cleaner and use Special ability.
3. Add a Cook and use Crazy Action.
4. Try to add a worker without a name and verify that an error is shown.
5. Use actions until there is not enough energy and verify that Energy never becomes negative.

## WPF design

The application uses a `DataTemplate` to display every worker's name and energy in the worker list.

A `ProgressBar` displays the selected worker's current energy. This makes changes in the worker's state easy to see.

## Collaboration

A classmate will add another `HotelWorker` subclass with its own themed `CrazyAction()` through a separate Git branch and Pull Request.

Classmate: TO BE ADDED

Issue: TO BE ADDED

Pull Request: TO BE ADDED

## AI use

ChatGPT was used to help understand the assignment requirements, plan the project structure, explain C# and WPF concepts, and help with debugging and Git commands. The generated code was built and tested by the project author, who checked the application behavior and made the final project decisions.
