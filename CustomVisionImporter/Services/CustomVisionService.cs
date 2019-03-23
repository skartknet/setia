using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace CustomVisionImporter.Services
{
    class CustomVisionService
    {
        private readonly CustomVisionTrainingClient trainingApi;
        private readonly Guid projectId;

        public CustomVisionService(string trainingApiKey, string trainingEndpoint, Guid projectId)
        {
            trainingApi = new CustomVisionTrainingClient
            {
                ApiKey = trainingApiKey,
                Endpoint = trainingEndpoint
            };
            this.projectId = projectId;
        }

        /// <summary>
        /// It creates a tag with format `1234:name` or gets it if exists and returns it
        /// </summary>
        internal Tag CreateTag(string nodeId, string name)
        {
            //avoid duplicate tags
            var currentTags = trainingApi.GetTags(projectId);

            string tagValue = $"{nodeId}:{name}";

            Tag tag = currentTags.FirstOrDefault(t => t.Name == tagValue);
            if (tag == null)
            {
                tag = trainingApi.CreateTag(projectId, tagValue);
            }

            return tag;
        }

        /// <summary>
        /// updloads images to custom service and assigned to a tag.
        /// </summary>
        /// <param name="images"></param>
        internal void UploadImages(IEnumerable<string> images, Tag tag, int page = 0, int retry = 0, int imagesPerBatch = 10)
        {
            IEnumerable<ImageFileCreateEntry> imageFiles;            

            imageFiles = TakeImagesBatch(images, page, imagesPerBatch);
            

            if (!imageFiles.Any()) return;

            var batch = new ImageFileCreateBatch(imageFiles.ToList(), new List<Guid>() { tag.Id });

            Console.WriteLine($"Importing {imageFiles.Count() + (imagesPerBatch * page)} out of {images.Count()}...");

            try
            {
                var summary = trainingApi.CreateImagesFromFiles(projectId, batch);

                if (!summary.IsBatchSuccessful)
                {
                    foreach (var img in summary.Images.Where(img => img.Status != "OK"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"- Error uploading image {Path.GetFileName(img.SourceUrl)}. Status: {img.Status}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

            }
            catch (Exception ex)
            {

                if (retry < 3)
                {
                    Console.WriteLine($"Upload failed ({ex.Message}). Retrying {retry + 1} out of 3");
                    Task.Delay(1000 * retry).ContinueWith(t => UploadImages(images, tag, page, ++retry));
                }
                else
                {
                    throw ex;
                }
            }

            page++;
            UploadImages(images, tag, page);
        }

        private static IEnumerable<ImageFileCreateEntry> TakeImagesBatch(IEnumerable<string> images, int page, int imgsPerBatch = 10)
        {
            return images.Skip(page * imgsPerBatch).Take(imgsPerBatch).Select(fileName => new ImageFileCreateEntry(fileName, ImagesManager.ResizeImage(File.ReadAllBytes(fileName))));
        }
    }
}
