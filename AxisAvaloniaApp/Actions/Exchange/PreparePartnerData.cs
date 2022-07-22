using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Actions.Exchange
{
    public class PreparePartnerData : AbstractStage
    {
        private Action<string> newPartnerMessage;
        private Dictionary<int, Models.PartnerModel> availablePartners;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreparePartnerData"/> class.
        /// </summary>
        /// <param name="newPartnerMessage">Method to show that new partnwe was created.</param>
        public PreparePartnerData(Action<string> newPartnerMessage)
        {
            this.newPartnerMessage = newPartnerMessage;
            availablePartners = new Dictionary<int, Models.PartnerModel>();
            NewPartner = new Models.PartnerModel();
        }

        /// <summary>
        /// Sets data to search partner.
        /// </summary>
        /// <date>21.07.2022.</date>
        public Models.PartnerModel PartnerData { get; set; }

        /// <summary>
        /// Gets searched or created partner.
        /// </summary>
        /// <date>21.07.2022.</date>
        public Models.PartnerModel NewPartner { get; private set; }

        /// <summary>
        /// Starts invocation of stages.
        /// </summary>
        /// <param name="request">Total amount of the orders list.</param>
        /// <returns>Returns a method to call the next step.</returns>
        /// <date>21.07.2022.</date>
        public async override Task<object> Invoke(object request)
        {
            if (PartnerData == null)
            {
                throw new NullReferenceException();
            }

            if (availablePartners.ContainsKey(PartnerData.Id))
            {
                NewPartner.Clone(availablePartners[PartnerData.Id]);
            }
            else
            {
                NewPartner.Clone(await Models.PartnerModel.FindOrCreatePartnerAsync(
                    PartnerData,
                    true, 
                    newPartnerMessage));
                
                availablePartners.Add(PartnerData.Id, NewPartner);
            }

            return await base.Invoke(request);
        }
    }
}
