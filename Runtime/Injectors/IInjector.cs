namespace LiteNinja.DI
{
    public interface IInjector
    {
        /// <summary>
        /// Inject the services into a object
        /// </summary>
        /// <param name="obj">the object that should be injected</param>
        void Inject(object obj);
    }
}