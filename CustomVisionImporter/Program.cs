using CustomVisionImporter.Services;
using CustomVisionImporter.Services.Extensions;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Setas.Common.Models.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVisionImporter
{
    class Program
    {

        private static UmbracoService umbracoService;
        private static CustomVisionService customVisionService;


        static async Task Main(string[] args)
        {
            //---------- Configuration -------------------
            const string filesPath = @"D:\Mushrooms";
            const string trainingApiKey = "fb972b87bc4b45e5b80c396c2f36fc1d";
            Guid projectId = new Guid("7fd23595-1444-41c6-a91e-4c2ab67c96a3");
            //Uri apiBase = new Uri("http://localhost:22481/umbraco/api/");
            Uri apiBase = new Uri("http://setia-dev.azurewebsites.net/umbraco/api/");

            const string trainingEndpoint = "https://southcentralus.api.cognitive.microsoft.com";
            const string readyFolderId = "clean";
            //----------------------------------------------


            //init services
            umbracoService = new UmbracoService();
            umbracoService.ApiBase = apiBase;

            customVisionService = new CustomVisionService(trainingApiKey, trainingEndpoint, projectId);
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
                    try
                    {
                        await ProcessFolderAsync(mushroomFolder);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            Console.WriteLine();
            ConsoleSuccess($"Import finished!");
            Console.ReadLine();

        }

        private static void ConsoleError(string message, bool insertLineBreak = true)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            if (insertLineBreak) Console.WriteLine();

            WriteToLog(message);
        }

        private static void ConsoleInfo(string message, bool insertLineBreak = true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            if (insertLineBreak) Console.WriteLine();

            WriteToLog(message);
        }

        private static void ConsoleSuccess(string message, bool insertLineBreak = true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            if (insertLineBreak) Console.WriteLine();

            WriteToLog(message);
        }

        private static void WriteToLog(string message)
        {
            string logPath = @"C:\Users\koben\Documents\Setia\ImporterLogs";
            string fileName = "\\" + DateTime.Now.ToString("yyyyMMdd-hh:mm") + ".txt";
            using (StreamWriter sw = File.AppendText(logPath + fileName))
            {
                sw.WriteLine(message);
            }
        }

        private static async Task ProcessFolderAsync(string mushroomFolder)
        {
            var name = FirstCharToUpper(new DirectoryInfo(mushroomFolder).Name);
            var cleanName = name.Replace(" - clean", "", StringComparison.OrdinalIgnoreCase)
                                .Trim();

            ConsoleInfo($"==== Processing folder {cleanName} ====");            

            #region Create node in Umbraco

            Mushroom umbracoContent = null;
            try
            {
                umbracoContent = await UmbracoNodeFromContent(cleanName);
            }
            catch (Exception ex)
            {
                ConsoleError($"Error mapping content. Error: {ex.Message}");

                throw ex;
            }

            int? nodeId = null;

            try
            {
                ConsoleInfo("--- Importing info into Umbraco ---");

                nodeId = await umbracoService.GetMushroomIdAsync(cleanName);

                if (!nodeId.HasValue)
                {
                    //Create a node in umbraco with this name
                    nodeId = await umbracoService.CreateNodeAsync(umbracoContent);

                    //parsing of returned id failed.
                    if (nodeId == null)
                    {
                        ConsoleError("ERROR Importing info into Umbraco!");
                    }
                }
                else
                {
                    ConsoleInfo($"Node {name} already exists in Umbraco.");
                }

            }
            catch (Exception ex)
            {
                ConsoleError($"ERROR creating node in Umbraco with name {name}. Reason: {ex.Message}");

                throw ex;

            }

            ConsoleSuccess($"SUCCESS importing info into Umbraco. Node ID: {nodeId}");

            ConsoleInfo("--- Uploading images into custom vision ---");

            var images = Directory.EnumerateFiles(mushroomFolder);

            ExtractImagesToControlImagesFolder(images, mushroomFolder);

            ConsoleInfo($"There's a total of {images.Count()} images.");

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

            ConsoleSuccess($"==== SUCCESS: Folder {name} processed succesfully. ====", true);            
        }

        private static void ExtractImagesToControlImagesFolder(IEnumerable<string> images, string path, int percentageImagesToTake = 20)
        {
            try
            {
                var controlDirPath = path + "\\control";
                Directory.CreateDirectory(controlDirPath);
                var imagesToTake = (int)Math.Floor(images.Count() * percentageImagesToTake / 100d);
                var controlImages = images.Take(imagesToTake);

                
                foreach (var image in controlImages)
                {
                    var filename = Path.GetFileName(image);
                    var destFile = Path.Combine(controlDirPath, filename);
                    File.Move(image, destFile);
                }
            }
            catch (Exception ex)
            {
                ConsoleError($"Error creating control directory. Error: {ex.Message}");
            }
        }

        private static async Task<Mushroom> UmbracoNodeFromContent(string cleanName)
        {
            var contentPage = await SiteScrapperService.GetContentPage(cleanName);

            if(contentPage == null)
            {
                throw new Exception("Site scrapper: the content page could't be found.");
            }

            TextInfo textInfo = new CultureInfo("es-ES", false).TextInfo;
            var cookingInterest = contentPage.GetMushroomDetailsValue("Importancia Práctica/Interés Gastronómico");


            var umbracoContent = new Mushroom
            {
                Name = cleanName,
                Class = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Clase").ToLowerInvariant()),
                Subclass = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Subclase").ToLowerInvariant()),
                Order = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Orden").ToLowerInvariant()),
                Family = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Familia").ToLowerInvariant()),
                Synonyms = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Sinónimos").ToLowerInvariant().Replace("\n", "").Replace("\r", "")),
                Description = contentPage.GetMushroomDetailsValue("Descripción macroscópica").Replace("\n", "").Replace("\r", ""),
                Confusion = contentPage.GetMushroomDetailsValue("Confusiones").Replace("\n", "").Replace("\r", ""),
                CookingInterest = SiteScrapperService.EdibleStringToEnum(cookingInterest),
                PopularNames = textInfo.ToTitleCase(contentPage.GetMushroomDetailsValue("Nombres Populares").ToLowerInvariant()),
                CookingInstructions = contentPage.GetMushroomDetailsValue("Cocina").Replace("\n", "").Replace("\r", ""),
                Habitat = contentPage.GetMushroomDetailsValue("Hábitat").Replace("\n", "").Replace("\r", ""),
                Season = contentPage.GetMushroomDetailsValue("Temporada").Replace("\n", "").Replace("\r", ""),

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
