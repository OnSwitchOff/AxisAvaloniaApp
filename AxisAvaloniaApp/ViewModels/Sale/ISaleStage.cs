using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public interface ISaleStage
    {
        /// <summary>
        /// Sets next stage to finalize operation of sale.
        /// </summary>
        /// <param name="nextStage">Next stage.</param>
        /// <returns>Returns next stage to finalize operation of sale.</returns>
        /// <date>23.06.2022.</date>
        ISaleStage SetNext(ISaleStage nextStage);

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns invocation method of next stage.</returns>
        /// <date>23.06.2022.</date>
        Task<object> Invoke(object request);
    }
}
