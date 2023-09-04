namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал об изменении состояния приложения
    /// </summary>
    public class ApplicationStateChangedSignal {
        public readonly ApplicationState CurrentState;

        public ApplicationStateChangedSignal(ApplicationState state) {
            CurrentState = state;
        }
    }
}
