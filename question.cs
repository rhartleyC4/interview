public class Controller
{
    private readonly ControllerConnection _controllerCommunication;
    private readonly Email _email;
    private readonly List<Device> _devices;

    public Controller()
    {
        _controllerCommunication = new ControllerConnection();
        _controllerCommunication.DeviceStatusChanged += OnDeviceStatusChanged;
        _email = new Email();
        _devices = new List<Device>();
    }

    public void AddDevice(string name)
    {
        var device = new Device(name);
        if (_controllerCommunication.AddDevices(device))
        {
            device.Status = _controllerCommunication.ConnectDevice(device.Id);
            if (device.Status == Status.Error)
            {
                _email.Send($"Device {device.Name}({device.Id}) has an error");
            }
            _devices.Add(device);
        }
        else
        {
            _email.Send($"Failed to add device {device.Name}({device.Id})");
        }
    }

    public Device FindDevice(Guid id)
    {
        return _devices
            .FirstOrDefault(d => d.Id == id);
    }

    private void OnDeviceStatusChanged(Device device, Status previousStatus)
    {
        if (previousStatus == Status.Connected && device.Status == Status.Error)
        {
            _email.Send($"Device {device.Name}({device.Id}) has an error");
        }
    }
}

public delegate void DeviceStatusChanged(Device currentDevice, Status previousStatus);

public class ControllerConnection
{
    public event DeviceStatusChanged DeviceStatusChanged;

    public bool AddDevices(Device device)
    {
        // logic would be here
        return true;
    }

    public Status ConnectDevice(Guid deviceId)
    {
        // logic would be here
        return Status.Connected;
    }

    // logic would be here to fire event
}

public class Email
{
    public void Send(string message)
    {
        // send logic
    }
}

public enum Status
{
    Unknown,
    Disconnected,
    Connected,
    Error
}

public class Device
{
    public Device(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        Status = Status.Unknown;
    }

    public Guid Id { get; }

    public string Name { get; set; }

    public Status Status { get; set; }
}
