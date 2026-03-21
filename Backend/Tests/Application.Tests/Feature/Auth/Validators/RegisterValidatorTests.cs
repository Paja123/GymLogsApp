using Application.Feature.Auth.Commands.Register;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Feature.Auth.Validators
{
    public class RegisterValidatorTests
    {
        private readonly RegisterValidator _validator = new();

        [Fact]
        public void Should_Pass_When_All_Fields_Valid()
        {
            var command = new RegisterCommand(
                "Jhon", "Doe", "johndoe", "john@example.com", "Password1!");

            var result = _validator.TestValidate(command);
            
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_Email_Invalid()
        {
            var command = new RegisterCommand(
                                "Jhon", "Doe", "johndoe", "not-an-email", "Password1!");
            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Fail_When_Password_Too_Short()
        {
            var command = new RegisterCommand(
                "John", "Doe", "johndoe",
                "john@example.com", "Ab1!");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
        [Fact]
        public void Should_Fail_When_Password_Missing_Uppercase()
        {
            var command = new RegisterCommand(
                "John", "Doe", "johndoe",
                "john@example.com", "password1!");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
        [Fact]
        public void Should_Fail_When_Password_Missing_Number()
        {
            var command = new RegisterCommand(
                "John", "Doe", "johndoe",
                "john@example.com", "Password!");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
        [Fact]
        public void Should_Fail_When_FirstName_Empty()
        {
            var command = new RegisterCommand(
                "", "Doe", "johndoe",
                "john@example.com", "Password1!");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }
        [Fact]
        public void Should_Fail_When_LastName_Empty()
        {
            var command = new RegisterCommand(
                "John", "", "johndoe",
                "john@example.com", "Password1!");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }
    }
}
