using System;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.StartUp
{
    public interface IStartUpService
    {
        event Action<int,string>? ProgressChanged;

        /// <summary>
        /// Runs activation handlers to perform the program activation.
        /// </summary>
        /// <returns>Task with process of programm activation.</returns>
        Task ActivateAsync();



    }
}
