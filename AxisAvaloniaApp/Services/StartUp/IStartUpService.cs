using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.StartUp
{
    public interface IStartUpService
    {
        /// <summary>
        /// Runs activation handlers to perform the program activation.
        /// </summary>
        /// <param name="isFirstRun">Value indicating whether this is the first start of the application.</param>
        /// <returns>Task with process of programm activation.</returns>
        Task ActivateAsync(bool isFirstRun);
    }
}
