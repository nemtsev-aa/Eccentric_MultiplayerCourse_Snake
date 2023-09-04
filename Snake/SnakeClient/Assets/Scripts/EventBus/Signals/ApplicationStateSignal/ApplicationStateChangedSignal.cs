namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ �� ��������� ��������� ����������
    /// </summary>
    public class ApplicationStateChangedSignal {
        public readonly ApplicationState CurrentState;

        public ApplicationStateChangedSignal(ApplicationState state) {
            CurrentState = state;
        }
    }
}
