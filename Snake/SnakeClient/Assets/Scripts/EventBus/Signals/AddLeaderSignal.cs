namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал на добавление нового игрока в список лидеров
    /// </summary>
    public class AddLeaderSignal {
        public string Key { get; private set; }
        public Player Player { get; private set; }

        public AddLeaderSignal(string key, Player player) {
            Key = key;
            Player = player;
        }
    }
}
