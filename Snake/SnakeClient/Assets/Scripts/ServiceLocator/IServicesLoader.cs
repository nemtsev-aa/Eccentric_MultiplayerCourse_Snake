public interface IServicesLoader {

    void Init();

    void RegisterServices();

    void AddDisposables();

    void OnDestroy();
}
