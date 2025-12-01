using Common.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Reflection.Metadata;

namespace Common.Utilities;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddAllImplementationsInAssemblies<T>(IEnumerable<Assembly> assemblies, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var interfaceType = typeof(T);

            if (!interfaceType.IsGenericType)
                throw new ArgumentException("Type must be an interface", nameof(T));

            foreach (var assembly in assemblies)
            {
                if (assembly is null)
                    continue;

                var implementations = assembly.GetTypes()
                    .Where(t => !t.IsAbstract &&
                               !t.IsInterface &&
                               interfaceType.IsAssignableFrom(t));

                foreach (var implementation in implementations)
                {
                    services.Add(new ServiceDescriptor(interfaceType, implementation, lifetime));
                }
            }
        }

        public void AddAllImplementationsInAssemblies(Type interfaceType, IEnumerable<Assembly> assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (!interfaceType.IsInterface)
                throw new ArgumentException("Type must be an interface", nameof(interfaceType));

            foreach (var assembly in assemblies)
            {
                if (assembly is null)
                    continue;

                var implementations = assembly.GetTypes()
                    .Where(t => !t.IsAbstract &&
                                !t.IsInterface &&
                                !t.IsGenericTypeDefinition &&
                                t.GetInterfaces()
                                .Any(i => interfaceType.IsGenericTypeDefinition ?
                                            i.IsGenericType && interfaceType.IsAssignableFrom(i.GetGenericTypeDefinition()) :
                                            interfaceType.IsAssignableFrom(i)))
                                .ToList();

                foreach (var implementation in implementations)
                {
                    // Находим закрытый generic интерфейс, который реализует этот тип
                    var closedInterfaces = implementation.GetInterfaces()
                        .Where(i => !i.IsGenericTypeDefinition &&
                                    i.IsGenericType ?
                                        i.GetGenericTypeDefinition() == interfaceType :
                                        i.GetInterfaces().Any(i => i.IsGenericType && interfaceType.IsAssignableFrom(i.GetGenericTypeDefinition())))
                        .ToList();
                    
                    foreach (var closedInterface in closedInterfaces)
                    {
                        services.Add(new ServiceDescriptor(closedInterface, implementation, lifetime));
                    }
                }
            }
        }

        public void AddAllImplementations<T>(ServiceLifetime lifetime = ServiceLifetime.Scoped,
            Func<Assembly, bool>? assemblyFilter = null)
        {
            // Получаем все загруженные сборки
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Применяем фильтр если предоставлен
            assemblies = assemblyFilter is null ? assemblies : assemblies.Where(assemblyFilter);

            AddAllImplementationsInAssemblies<T>(services, assemblies, lifetime);
        }

        public void AddAllImplementations(Type openGenericInterface, ServiceLifetime lifetime = ServiceLifetime.Scoped,
            Func<Assembly, bool>? assemblyFilter = null)
        {
            // Получаем все загруженные сборки
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Применяем фильтр если предоставлен
            assemblies = assemblyFilter is null ? assemblies : assemblies.Where(assemblyFilter);

            AddAllImplementationsInAssemblies(services, openGenericInterface, assemblies, lifetime);
        }

        public void AddAllImplementationsFromApplication<T>(ServiceLifetime lifetime = ServiceLifetime.Scoped,
            Func<Assembly, bool>? assemblyFilter = null)
        {
            var assemblies = GetAllAssembliesFromApplication();

            var filteredAssemblies = assemblyFilter is null ? assemblies : assemblies.Where(assemblyFilter);

            AddAllImplementationsInAssemblies<T>(services, filteredAssemblies, lifetime);
        }

        public void AddAllImplementationsFromApplication(Type openGenericInterface, ServiceLifetime lifetime = ServiceLifetime.Scoped,
                Func<Assembly, bool>? assemblyFilter = null)
        {
            var assemblies = GetAllAssembliesFromApplication();

            var filteredAssemblies = assemblyFilter is null ? assemblies : assemblies.Where(assemblyFilter);

            AddAllImplementationsInAssemblies(services, openGenericInterface, filteredAssemblies, lifetime);
        }

        public void AddAllTypesWithDependencies<T>(ServiceLifetime lifetime = ServiceLifetime.Scoped,
            Func<Assembly, bool>? assemblyFilter = null)
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly == null)
                return;

            // Получаем все связанные сборки
            var assemblies = GetReferencedAssembliesRecursive(entryAssembly);
            assemblies.Add(entryAssembly);

            // Применяем фильтр если предоставлен
            IEnumerable<Assembly> filteredAssemblies = assemblyFilter is null ? assemblies : assemblies.Where(assemblyFilter);

            AddAllImplementationsInAssemblies<T>(services, filteredAssemblies, lifetime);
        }

        public void AddAllImplementationsWithDependencies(Type openGenericInterface, ServiceLifetime lifetime = ServiceLifetime.Scoped,
            Func<Assembly, bool>? assemblyFilter = null)
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly == null)
                return;

            // Получаем все связанные сборки
            var assemblies = GetReferencedAssembliesRecursive(entryAssembly);
            assemblies.Add(entryAssembly);

            // Применяем фильтр если предоставлен
            IEnumerable<Assembly> filteredAssemblies = assemblyFilter is null ? assemblies : assemblies.Where(assemblyFilter);

            AddAllImplementationsInAssemblies(services, openGenericInterface, filteredAssemblies, lifetime);
        }
    }

    private static HashSet<Assembly> GetReferencedAssembliesRecursive(Assembly assembly)
    {
        var result = new HashSet<Assembly>();
        var stack = new Stack<Assembly>();

        stack.Push(assembly);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            foreach (var reference in current.GetReferencedAssemblies())
            {
                try
                {
                    var referencedAssembly = Assembly.Load(reference);
                    if (!result.Contains(referencedAssembly))
                    {
                        result.Add(referencedAssembly);
                        stack.Push(referencedAssembly);
                    }
                }
                catch
                {
                    // Игнорируем сборки, которые не могут быть загружены
                }
            }
        }

        return result;
    }

    private static IEnumerable<Assembly> GetAllAssembliesFromApplication()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        var location = Path.GetDirectoryName(entryAssembly?.Location);

        if (string.IsNullOrEmpty(location))
            yield break;

        // Получаем все DLL файлы в папке приложения
        var dllFiles = Directory.GetFiles(location, "*.dll");

        foreach ( var dllFile in dllFiles)
        {
            Assembly assembly;
            try
            {
                // Загружаем сборку
                assembly = Assembly.LoadFrom(dllFile);
            }
            catch (Exception ex) when (ex is BadImageFormatException || ex is ReflectionTypeLoadException)
            {
                // Игнорируем некорректные сборки
                continue;
            }

            yield return assembly;
        }
    }
}
