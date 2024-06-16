namespace HomeApi.Data.Queries
{
    public class UpdateRoomQuery
    {
        public string NewName { get; }

        public string NewArea { get; set; }
        public string NewGasConnected { get; set; }
        public string NewVoltage { get; set; }

        public UpdateRoomQuery(
            string newName = null,
            string newArea = null,
            string newGasConnected = null,
            string newVoltage = null)
        {
            NewName = newName;
            NewArea = newArea;
            NewGasConnected = newGasConnected;
            NewVoltage = newVoltage;
        }
    }
}
