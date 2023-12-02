using System;
using System.IO;

namespace CodeGenerator.Script
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentWorkingDirectory = Directory.GetCurrentDirectory();
            string projectName = "Iter.Core";
            string targetFolderDBEntities = @"C:\Projects\Iter\Iter.Api\Iter.Core\Models\";
            string absolutePathDBEntities = Path.GetFullPath(targetFolderDBEntities);
            string targetFolderController = @"C:\Projects\Iter\Iter.Api\Iter.Api\Controllers\";
            string absolutePathTargetFolder = Path.GetFullPath(targetFolderController);
            string targetFolderService = @"C:\Projects\Iter\Iter.Api\Iter.Service";
            string absolutePathTargetFolderService = Path.GetFullPath(targetFolderService);
            string targetFolderServiceInterface = @"C:\Projects\Iter\Iter.Api\Iter.Service.Interface";
            string absolutePathTargetFolderServiceInterface = Path.GetFullPath(targetFolderServiceInterface);
            string targetFolderRepository = @"C:\Projects\Iter\Iter.Api\Iter.Repository";
            string absolutePathTargetFolderRepository = Path.GetFullPath(targetFolderRepository);
            string targetFolderRepositoryInterface = @"C:\Projects\Iter\Iter.Api\Iter.Repository.Interface";
            string absolutePathTargetFolderRepositoryInterface = Path.GetFullPath(targetFolderRepositoryInterface);
            string targetFolderUpsertModels = @"C:\Projects\Iter\Iter.Api\Iter.Core\Requests";
            string absolutePathTargetFolderUpsertModels = Path.GetFullPath(targetFolderUpsertModels);
            string targetFolderResponseModels = @"C:\Projects\Iter\Iter.Api\Iter.Core\Responses\";
            string absolutePathTargetFolderResponseModels = Path.GetFullPath(targetFolderResponseModels);

            string targetFolderMapper = @"C:\Projects\Iter\Iter.Api\Iter.Api\Mapping";
            string absolutePathTargetFolderMapper = Path.GetFullPath(targetFolderMapper);
            string[] mapperContent = File.ReadAllLines(Path.Combine(absolutePathTargetFolderMapper, $"IterAutoMapperProfile.cs"));

            string targetStartupHelper = @"C:\Projects\Iter\Iter.Api\Iter.Api\Infrastructure";
            string absolutePathTargetStartupHelper = Path.GetFullPath(targetStartupHelper);
            string[] startupHelperContent = File.ReadAllLines(Path.Combine(absolutePathTargetStartupHelper, $"StartupHelper.cs"));

            string[] entityPaths = Directory.GetFiles(absolutePathDBEntities, "*.cs", SearchOption.TopDirectoryOnly);

            string[] entityNames = entityPaths.Select(path => Path.GetFileNameWithoutExtension(path)).ToArray();
            entityNames = Array.FindAll(entityNames, name => !name.Contains("Context") && !name.Contains("Address") && !name.Contains("Agency") && !name.Contains("Migrations"));
            
            string[] entityContents = entityPaths.Select(path => File.ReadAllText(path)).ToArray();
            entityContents = Array.FindAll(entityContents, content => !content.Contains("Context") && !content.Contains("Address") && !content.Contains("Agency") && !content.Contains("Migrations"));

            // string[][] entityContentLines = entityPaths.Select(path => File.ReadAllLines(path)).ToArray();
            // entityContentLines = Array.FindAll(entityContentLines, content => !content.ToString().Contains("Context") && !content.ToString().Contains("Migrations"));

            foreach (string entityName in entityNames)
            {
                GenerateAndWriteTemplate($"{entityName}Controller.cs", absolutePathTargetFolder, () => new ControllerTemplate(entityName));
                GenerateAndWriteTemplate($"{entityName}Service.cs", absolutePathTargetFolderService, () => new ServiceTemplate(entityName));
                GenerateAndWriteTemplate($"I{entityName}Service.cs", absolutePathTargetFolderServiceInterface, () => new IServiceTemplate(entityName));
                GenerateAndWriteTemplate($"{entityName}Repository.cs", absolutePathTargetFolderRepository, () => new RepositoryTemplate(entityName));
                GenerateAndWriteTemplate($"I{entityName}Repository.cs", absolutePathTargetFolderRepositoryInterface, () => new IRepositoryTemplate(entityName));

                GenerateAndWriteUpsertModel(absolutePathTargetFolderUpsertModels, entityNames, entityContents);
                GenerateAndWriteResponseModel(absolutePathTargetFolderResponseModels, entityNames, entityContents);
            }

            // mapper
            GenerateAndWriteMapping(absolutePathTargetFolderMapper, mapperContent, entityNames);

            // startup helper
            GenerateAndWriteConfigServices(absolutePathTargetStartupHelper, startupHelperContent, entityNames);
        }
        public static string ModifyEntityContent(string entityContent, string entityName, string className)
        {
            string[] entityContentLines = entityContent.Split("\n");
            string modifiedEntityContent = "";
            foreach (string line in entityContentLines)
            {
                if (line.Contains("namespace Iter.Core.Models"))
                {
                    modifiedEntityContent += "namespace Iter.Core.Requests" + Environment.NewLine;
                }
                else if (line.Contains("public partial class"))
                {
                    modifiedEntityContent += "    public class " + entityName + className + Environment.NewLine;
                } else {
                    modifiedEntityContent += line;
                }
            }
            return modifiedEntityContent;
        }


        public static void GenerateAndWriteTemplate<TTemplate>(
        string templatePath, 
        string absolutePathTargetFolder,
        Func<TTemplate> templateFactory)
        {
            if (!File.Exists(Path.Combine(absolutePathTargetFolder, templatePath)))
            {
                TTemplate template = templateFactory.Invoke();
                dynamic dynamicTemplate = template;

                string generatedTemplate = dynamicTemplate.Generate();
                File.WriteAllText(Path.Combine(absolutePathTargetFolder, templatePath), generatedTemplate);
            }
            else
            {
                Console.WriteLine("File already exists");
            }
        }

        public static void GenerateAndWriteUpsertModel(string absolutePathTargetFolderUpsertModels, string[] entityNames, string[] entityContents)
        {
            foreach (string entityName in entityNames)
            {
                if (!File.Exists(Path.Combine(absolutePathTargetFolderUpsertModels, $"{entityName}UpsertRequest.cs")))
                {
                    string entityContent = entityContents[Array.IndexOf(entityNames, entityName)];
                    string modifiedEntityContent = ModifyEntityContent(entityContent, entityName, "UpsertRequest");
                    File.WriteAllText(Path.Combine(absolutePathTargetFolderUpsertModels, $"{entityName}UpsertRequest.cs"), modifiedEntityContent);
                }
                else
                {
                    Console.WriteLine("File already exists");
                }
            }
        }

        public static void GenerateAndWriteConfigServices(string absolutePathTargetStartupHelper, string[] startupHelperContent, string[] entityNames)
        {
            if (File.Exists(Path.Combine(absolutePathTargetStartupHelper, "StartupHelper.cs")))
            {
                bool insideConfigureServices = false;
            
                string[] startupHelperContentLinesTrimed = startupHelperContent.Select(line => line.TrimEnd()).Select(line => line.TrimStart()).ToArray();

                List<string> modifiedStartupHelperContentLines = new List<string>();

                foreach (string line in startupHelperContent)
                {
                    if (line.Contains("public static void ConfigureServices(this IServiceCollection services)"))
                    {
                        insideConfigureServices = true;
                        modifiedStartupHelperContentLines.Add(line);
                    }
                    else if (insideConfigureServices)
                    {
                        if (line.Contains("{"))
                        {
                            modifiedStartupHelperContentLines.Add(line);

                            // Add mappings for each entity
                            foreach (string entityName in entityNames)
                            {
                                if (!startupHelperContentLinesTrimed.Contains($"services.AddScoped<I{entityName}Service, {entityName}Service>();"))
                                {
                                    modifiedStartupHelperContentLines.Add($"            services.AddScoped<I{entityName}Service, {entityName}Service>();");
                                }
                                if (!startupHelperContentLinesTrimed.Contains($"services.AddScoped<I{entityName}Repository, {entityName}Repository>();"))
                                {
                                    modifiedStartupHelperContentLines.Add($"            services.AddScoped<I{entityName}Repository, {entityName}Repository>();");
                                    modifiedStartupHelperContentLines.Add(Environment.NewLine);
                                }
                            }

                            insideConfigureServices = false; // Set the flag to false after adding mappings
                        }
                        else
                        {
                            modifiedStartupHelperContentLines.Add(line);
                        }
                    }
                    else
                    {
                        modifiedStartupHelperContentLines.Add(line);
                    }
                }
                File.WriteAllLines(Path.Combine(absolutePathTargetStartupHelper, "StartupHelper.cs"), modifiedStartupHelperContentLines);
            }
        }

        public static void GenerateAndWriteMapping(string absolutePathTargetFolderMapper, string[] mapperContentLines, string[] entityNames)
        {
            if (File.Exists(Path.Combine(absolutePathTargetFolderMapper, "IterAutoMapperProfile.cs")))
            {
                bool insideCreateMap = false;

                string[] mapperContentLinesTrimed = mapperContentLines.Select(line => line.TrimEnd()).Select(line => line.TrimStart()).ToArray();

                List<string> modifiedMapperContentLines = new List<string>();

                foreach (string line in mapperContentLines)
                {
                    if (line.Contains("private void CreateMap()"))
                    {
                        insideCreateMap = true;
                        modifiedMapperContentLines.Add(line);
                    }
                    else if (insideCreateMap)
                    {
                        if (line.Contains("{"))
                        {
                            modifiedMapperContentLines.Add(line);

                            foreach (string entityName in entityNames)
                            {
                                if (!mapperContentLinesTrimed.Contains($"this.CreateMap<{entityName}?, {entityName}Response?>();"))
                                {
                                    modifiedMapperContentLines.Add($"            this.CreateMap<{entityName}?, {entityName}Response?>();");
                                    modifiedMapperContentLines.Add($"            this.CreateMap<{entityName}UpsertRequest?, {entityName}?>();");
                                    modifiedMapperContentLines.Add(Environment.NewLine);
                                }
                            }

                            insideCreateMap = false; // Set the flag to false after adding mappings
                        }
                        else
                        {
                            modifiedMapperContentLines.Add(line);
                        }
                    }
                    else
                    {
                        modifiedMapperContentLines.Add(line);
                    }
                }

                File.WriteAllLines(Path.Combine(absolutePathTargetFolderMapper, "IterAutoMapperProfile.cs"), modifiedMapperContentLines);
            }
        }
    
        public static void GenerateAndWriteResponseModel(string absolutePathTargetFolderResponseModels, string[] entityNames, string[] entityContents)
        {
            foreach (string entityName in entityNames)
            {
                if (!File.Exists(Path.Combine(absolutePathTargetFolderResponseModels, $"{entityName}Response.cs")))
                {
                    string entityContent = entityContents[Array.IndexOf(entityNames, entityName)];
                    string modifiedEntityContent = ModifyEntityContent(entityContent, entityName, "Response");
                    // string modifiedEntityContent = ModifyEntityContentLines(entityContents, entityName, "Response");
                    File.WriteAllText(Path.Combine(absolutePathTargetFolderResponseModels, $"{entityName}Response.cs"), modifiedEntityContent);
                }
                else
                {
                    Console.WriteLine("File already exists");
                }
            }
        }
    }

}