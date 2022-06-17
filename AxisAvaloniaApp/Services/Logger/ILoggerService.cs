using AxisAvaloniaApp.UserControls.MessageBox;
using DataBase.Enums;
using System;

namespace AxisAvaloniaApp.Services.Logger
{
    public interface ILoggerService
    {
        /// <summary>
        /// Writes description of error to log file.
        /// </summary>
        /// <param name="sender">Class in which error is happened.</param>
        /// <param name="message">Description of error.</param>
        /// <param name="method">Method in which error is happened.</param>
        /// <date>13.06.2022.</date>
        void RegisterError(object sender, string message, string method = "");

        /// <summary>
        /// Writes description of error to log file.
        /// </summary>
        /// <param name="sender">Class in which error is happened.</param>
        /// <param name="exception">Error exeption.</param>
        /// <param name="method">Method in which error is happened.</param>
        /// <date>13.06.2022.</date>
        void RegisterError(object sender, Exception exception, string method = "");

        /// <summary>
        /// Writes description of error to Database.
        /// </summary>
        /// <param name="eventID">Id of application stage on which error is happened.</param>
        /// <param name="description">Description of error.</param>
        /// <param name="operationHeaderID">The identifier of the operation in which the action took place.</param>
        /// <date>13.06.2022.</date>
        void RegisterError(EApplicationLogEvents eventID, string description, int operationHeaderID = 0);

        /// <summary>
        /// Shows info to user.
        /// </summary>
        /// <param name="messageKey">Key to localize message that will be shown to user.</param>
        /// <param name="headerKey">Key to localize header of the message.</param>
        /// <param name="icon">Icon that will visualize message.</param>
        /// <param name="buttons">Buttons to get response from user.</param>
        /// <returns></returns>
        /// <date>14.06.2022.</date>
        System.Threading.Tasks.Task<EButtonResults> ShowDialog(string messageKey, string headerKey = "", EButtonIcons icon = EButtonIcons.None, EButtons buttons = EButtons.Ok);

        /// <summary>
        /// Shows info to user.
        /// </summary>
        /// <param name="message">Message that will be shown to user.</param>
        /// <param name="header">Header of the message.</param>
        /// <param name="icon">Icon that will visualize message.</param>
        /// <param name="buttons">Buttons to get response from user.</param>
        /// <returns></returns>
        /// <date>14.06.2022.</date>
        System.Threading.Tasks.Task<EButtonResults> ShowDialog1(string message, string header = "", EButtonIcons icon = EButtonIcons.None, EButtons buttons = EButtons.Ok);
    }
}
