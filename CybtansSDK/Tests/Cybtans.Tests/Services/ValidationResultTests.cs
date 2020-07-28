using AutoMapper.Configuration.Annotations;
using Cybtans.Serialization;
using Cybtans.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cybtans.Tests.Services
{
   public class ValidationResultTests
    {
        [Fact]
        public void AddErrors()
        {
            ValidationResult result = new ValidationResult();
            result.AddError("Name", "Name is required");
            result.AddError("Name", "Name is not valid");
            result.AddError("Lastname", "LastName is required");

            Assert.True(result.HasErrors);
            Assert.Equal(2, result.Errors.Count);
            Assert.Equal(2, result.Errors["Name"].Count);
            Assert.Single(result.Errors["Lastname"]);
        }

        [Fact]
        public void AddErrorMessage()
        {
            ValidationResult result = new ValidationResult("Not Valid");            
            Assert.True(result.HasErrors);
            Assert.Equal("Not Valid", result.ErrorMessage);          
        }

        [Fact]
        public void SerializeTest()
        {
            ValidationResult result = new ValidationResult();
            result.AddError("Name", "Name is required");
            result.AddError("Name", "Name is not valid");
            result.AddError("Lastname", "LastName is required");

            var bytes = BinaryConvert.Serialize(result);

            result = BinaryConvert.Deserialize<ValidationResult>(bytes);
            Assert.True(result.HasErrors);
            Assert.Equal(2, result.Errors.Count);
            Assert.Equal(2, result.Errors["Name"].Count);
            Assert.Single(result.Errors["Lastname"]);

            Assert.Equal("Name is required", result.Errors["Name"][0]);
            Assert.Equal("Name is not valid", result.Errors["Name"][1]);

            Assert.Equal("LastName is required", result.Errors["Lastname"][0]);

        }
    }
}
