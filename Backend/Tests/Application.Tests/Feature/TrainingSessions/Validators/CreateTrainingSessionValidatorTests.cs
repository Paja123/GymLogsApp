using Application.Feature.TrainingSessions.Commands.Create;
using Domain.Enums;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Feature.TrainingSessions.Validators
{
    public class CreateTrainingSessionValidatorTests
    {
        private readonly CreateTrainingSessionValidator _validator = new();

        [Fact]
        public void Should_Have_Errors_For_All_Invalid_Properties() {
            
            var command = new CreateTrainingSessionCommand
            (
                (TrainingType)999,
                0, 
                -22, 
                11,  
                0,
                DateTime.Now.AddDays(1),
                new string('a', 301)
            );

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TrainingType);
            result.ShouldHaveValidationErrorFor(x => x.Duration);
            result.ShouldHaveValidationErrorFor(x => x.IntensityLevel);
            result.ShouldHaveValidationErrorFor(x => x.TirednessLevel);
            result.ShouldHaveValidationErrorFor(x => x.Date);
            result.ShouldHaveValidationErrorFor(x => x.Notes);
        }

        [Fact]
        public void Should_Not_Have_Errors_When_All_Properties_Valid()
        {
            var command = new CreateTrainingSessionCommand
            (
                TrainingType.Cardio,
                60,
                250,
                5,
                5,
                DateTime.Now.AddMilliseconds(-1),
               "All good"
            );

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

    }
}
