using System.Threading.Tasks;

namespace AxisAvaloniaApp.Rules.Common
{
    internal class NomenclatureIsNotNull : AbstractStage
    {
        private object nomanclature;

        /// <summary>
        /// Initializes a new instance of the <see cref="NomenclatureIsNotNull"/> class.
        /// </summary>
        /// <param name="nomanclature">Nomenclature object.</param>
        public NomenclatureIsNotNull(object nomanclature)
        {
            this.nomanclature = nomanclature;
        }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Data to the current method.</param>
        /// <returns>Returns a method to call the next step if the rule is met; otherwise returns "-1".</returns>
        /// <date>07.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (nomanclature == null)
            {
                return await Task.FromResult<object>(-1);
            }

            return await base.Invoke(request);
        }
    }
}
