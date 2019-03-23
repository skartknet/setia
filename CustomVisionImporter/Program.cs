using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomVisionImporter.Services;
using CustomVisionImporter.Services.Extensions;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Setas.Common.Models.Api;

namespace CustomVisionImporter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //---------- Configuration -------------------
            const string filesPath = @"D:\Mushrooms";
            const string trainingApiKey = "fb972b87bc4b45e5b80c396c2f36fc1d";
            Guid projectId = new Guid("aaedaff8-49db-48e9-aa8d-6cf0262a29d1");
            //Uri apiBase = new Uri("http://localhost:22481/umbraco/api/");
            Uri apiBase = new Uri("http://setia-dev.azurewebsites.net/umbraco/api/");

            const string trainingEndpoint = "https://southcentralus.api.cognitive.microsoft.com";
            const string readyFolderId = "clean";
            //----------------------------------------------


            //init services
            var umbracoService = new UmbracoService();
            umbracoService.ApiBase = apiBase;

            var customVisionService = new CustomVisionService(trainingApiKey, trainingEndpoint, projectId);
            //----------------

            string[] lettersFolders = default;
            try
            {
                lettersFolders = Directory.GetDirectories(filesPath);
            }
            catch (Exception ex)
            {

                ConsoleError("Error opening path." + ex.Message);
                Environment.Exit(0);
            }

            foreach (var letterFolder in lettersFolders)
            {

                var mushroomsFolders = Directory.GetDirectories(letterFolder).Where(folder => folder.Contains(readyFolderId, StringComparison.OrdinalIgnoreCase));

                foreach (var mushroomFolder in mushroomsFolders)
                {
                    var name = FirstCharToUpper(new DirectoryInfo(mushroomFolder).Name);
                    var cleanName = name.Replace(" - clean", "", StringComparison.OrdinalIgnoreCase)
                                        .Trim();

                    Console.WriteLine($"==== Processing folder {cleanName} ====");

                    #region Create node in Umbraco

                    Mushroom umbracoContent = null;
                    try
                    {
                        umbracoContent = await UmbracoNodeFromContent(cleanName);
                    }
                    catch (Exception ex)
                    {
                        ConsoleError($"Error mapping content. Error: {ex.Message}");

                        continue;
                    }

                    int? nodeId = null;

                    try
                    {
                        Console.WriteLine("--- Importing info into Umbraco ---");

                        nodeId = await umbracoService.GetMushroomIdAsync(cleanName);

                        if (!nodeId.HasValue)
                        {
                            //Create a node in umbraco with this name
                            nodeId = await umbracoService.CreateNodeAsync(umbracoContent);

                            //parsing of returned id failed.
                            if (nodeId == null)
                            {
                                ConsoleError("ERROR Importing info into Umbraco!");
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Node {name} already exists in Umbraco.");
                        }

                    }
                    catch (Exception ex)
                    {
                        ConsoleError($"ERROR creating node in Umbraco with name {name}. Reason: {ex.Message}");

                        continue;
                    }



                    Console.WriteLine($"SUCCESS importing info into Umbraco. Node ID: {nodeId}");

                    Console.WriteLine("--- Uploading images into custom vision ---");

                    var images = Directory.EnumerateFiles(mushroomFolder);

                    Console.WriteLine($"There's a total of {images.Count()} images.");

                    Tag tag = customVisionService.CreateTag(nodeId.ToString(), cleanName.Replace(" ", ""));

                    try
                    {
                        customVisionService.UploadImages(images, tag);
                    }
                    catch (Exception ex)
                    {
                        ConsoleError("Error uploading images to Custom Vision", insertLineBreak: false);
                        ConsoleError(ex.Message);
                    }



                    #endregion

                    Console.WriteLine($"==== SUCCESS: Folder {name} processed succesfully. ====");
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Import finished!");
            Console.ReadLine();

        }

        private static void ConsoleError(string message, bool insertLineBreak = true)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            if (insertLineBreak) Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static async Task<Mushroom> UmbracoNodeFromContent(string cleanName)
        {
            var contentPage = await SiteScrapperService.GetContentPage(cleanName);

            TextInfo textInfo = new CultureInfo("es-ES", false).TextInfo;
            var cookingInterest = contentPage.GetMushroomDetailsValue("Importancia Práctica/Interés Gastronómico");


            var umbracoContent = new Mushroom
            {
                Name = cleanName,
                Class = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Clase").ToLowerInvariant()),
                Subclass = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Subclase").ToLowerInvariant()),
                Order = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Orden").ToLowerInvariant()),
                Family = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Familia").ToLowerInvariant()),
                Synonyms = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Sinónimos").ToLowerInvariant().Replace("\n", " ")),
                Description = contentPage.GetMushroomDetailsValue("Descripción macroscópica").Replace("\n", " "),
                Confusion = contentPage.GetMushroomDetailsValue("Confusiones").Replace("\n", " "),
                CookingInterest = SiteScrapperService.EdibleStringToEnum(cookingInterest),
                PopularNames = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Nombres Populares").ToLowerInvariant()),
                CookingInstructions = contentPage.GetMushroomDetailsValue("Cocina").Replace("\n", " "),
                Habitat = contentPage.GetMushroomDetailsValue("Hábitat").Replace("\n", " "),
                Season = contentPage.GetMushroomDetailsValue("Temporada").Replace("\n", " "),

            };

            return umbracoContent;
        }

        public static string FirstCharToUpper(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.  
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.  
            // ... Uppercase the lowercase letters following spaces.  
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

    }
}
