namespace HomeApi.Contracts.Models.Devices
{
    /// <summary>
    /// Запрос для обновления свойств комнаты
    /// </summary>
    public class EditRoomRequest
    {
        public string NewName { get; set; }
        public string NewArea { get; set; }
        public string NewGasConnected { get; set; }
        public string NewVoltage { get; set; }
    }
}
