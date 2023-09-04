namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал на добавление нового игрока в список лидеров
    /// </summary>
    public class RemoveLeaderSignal {
        public string Key { get; private set; }

        public RemoveLeaderSignal(string key) {
            Key = key;
        }
    }
}