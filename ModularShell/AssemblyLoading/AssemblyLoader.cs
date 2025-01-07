using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.Loader;

namespace ModularShell.AssemblyLoading;

public class AssemblyLoader
{
    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, Assembly> _loadedAssemblies = new();
    
    public AssemblyLoader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Assembly>> LoadAdditionalAssemblies()
    {
        List<Assembly> newlyLoadedAssemblies = new();
        try
        {
            // Get manifest of modules to load
            var moduleManifest = await _httpClient.GetFromJsonAsync<List<Module>>("/Modules/modules.json");
            if (moduleManifest is null) throw new Exception("moduleManifest is null");
            
            foreach (var module in moduleManifest)
            {
                // Load dependencies first
                foreach (var dependencyPath in module.Dependencies)
                {
                    if (!_loadedAssemblies.ContainsKey(dependencyPath))
                    {
                        // Dependency not loaded yet, load it
                        var depResponse = await _httpClient.GetAsync(dependencyPath);
                        depResponse.EnsureSuccessStatusCode();
                        await using var depStream = await depResponse.Content.ReadAsStreamAsync();
                        var depAssembly = AssemblyLoadContext.Default.LoadFromStream(depStream);
                        _loadedAssemblies[dependencyPath] = depAssembly;
                        newlyLoadedAssemblies.Add(depAssembly);
                        Console.WriteLine($"Loaded dependency: {depAssembly.FullName} (for module: {module.Name})");
                    }
                    else
                    {
                        Console.WriteLine($"Dependency already loaded: {_loadedAssemblies[dependencyPath].FullName} (for module: {module.Name})");
                    }
                }

                // Load the module assembly
                if (!_loadedAssemblies.ContainsKey(module.Path))
                {
                    var moduleResponse = await _httpClient.GetAsync(module.Path);
                    moduleResponse.EnsureSuccessStatusCode();
                    await using var moduleStream = await moduleResponse.Content.ReadAsStreamAsync();
                    var moduleAssembly = AssemblyLoadContext.Default.LoadFromStream(moduleStream);
                    _loadedAssemblies[module.Path] = moduleAssembly;
                    newlyLoadedAssemblies.Add(moduleAssembly);
                    Console.WriteLine($"Loaded module: {moduleAssembly.FullName}");
                }
                else
                {
                    Console.WriteLine($"Module already loaded: {_loadedAssemblies[module.Path].FullName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading modules: {ex}");
        }

        return newlyLoadedAssemblies;
    }
}