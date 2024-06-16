using FluentValidation;
using HomeApi.Contracts.Models.Devices;

namespace HomeApi.Contracts.Validation
{
    public class EditRoomRequestValidator : AbstractValidator<EditRoomRequest>
    {
        public EditRoomRequestValidator()
        {
            RuleFor(x => x.NewArea).NotEmpty();
            RuleFor(x => x.NewName).NotEmpty();
            RuleFor(x => x.NewVoltage).NotEmpty().Must(BeSupported).WithMessage($"Значение Voltage должно быть от 110 до 220");
            RuleFor(x => x.NewGasConnected).NotEmpty();
        }

        private bool BeSupported(string voltage)
        {
            int.TryParse(voltage, out int available);

            return (available >= 110 && available <= 220);
        }

    }
}
