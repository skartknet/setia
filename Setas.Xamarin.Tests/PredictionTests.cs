using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Setas.Models;
using Setas.Services;
using Setas.ViewModels;
using System;

namespace Setas.Xamarin.Tests
{
    [TestClass]
    public class PredictionTests
    {
        [TestMethod]
        public void ShouldCreateProperResultsViewModel()
        {
            var result = new PredictionResponse
            {
                Id = "c1e00621-4033-472c-b545-6c2b2c41ab98",
                Project = "2e7aba89-bdde-479f-9b27-be098914db6a",
                Iteration = "3b753b92-5854-4643-b3e7-60c81d2cfd35",
                Created = DateTime.Parse("2018-11-21T21:33:08.9464937Z"),
                Predictions = new Prediction[]
                {
               new Prediction
               {
                   Probability = 0.366f,
                   TagName = "\"1087\" , \"Amanita citrina\""
               },
               new Prediction
               {
                   Probability = 0.249321371f,
                   TagName = "\"1107\" , \"excelsa\""
               },
               new Prediction
               {
                   Probability =  0.16047141f,
                   TagName = "\"1109\" , \"muscaria\""
               },
               new Prediction
               {
                   Probability = 0.130597785f,
                   TagName = "\"1085\" , \"Amanita caesarea\""
               }
                }
            };


            var dataService = new Mock<IDataService>();

            var identificationVm = new IdentificationViewModel(dataService.Object, null, null);
        }
    }
}
