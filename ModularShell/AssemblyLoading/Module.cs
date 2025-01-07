namespace ModularShell.AssemblyLoading;

public class Module
{
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public List<string> Dependencies { get; set;  } = [];
}