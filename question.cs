public class FooManager
{
    private readonly Store _store;
    private readonly Logger _logger;

    public FooManager()
    {
        _store = new Store();
        _logger = new Logger();
    }

    public Foo Add(string name)
    {
        try
        {
            string fooName = name.ToUpper();
            var foo = new Foo
            {
                Name = fooName
            };
            foo.Id = _store.Create(foo);
            return foo;
        }
        catch (Exception exception)
        {
            _logger.Write($"Failed to add '{name}': {exception}");
            return null;
        }
    }
}

public class Store
{
    public Guid Create(Foo foo)
    {
        // save to file
        return Guid.NewGuid();
    }
}

public class Logger
{
    public void Write(string message)
    {
        Console.WriteLine(message);
    }
}

public class Foo
{
    public Guid? Id { get; set; }

    public string Name { get; set; }
}
