using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Settings;
using AxisAvaloniaApp.Services.Translation;
using AxisAvaloniaApp.UserControls.MessageBoxes;
using DataBase.Entities.ApplicationLog;
using DataBase.Entities.OperationHeader;
using DataBase.Enums;
using DataBase.Repositories.ApplicationLog;
using DataBase.Repositories.OperationHeader;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        private readonly ITranslationService translationService;
        private readonly ISettingsService settingsService;
        private readonly IApplicationLogRepository applicationLogRepository;
        private readonly IOperationHeaderRepository operationHeaderRepository;

        private MessageBox messageBox;
        private MessageBoxModel dataContext;
        private MessageBoxParams boxParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerService"/> class.
        /// </summary>
        public LoggerService()
        {
            translationService = Splat.Locator.Current.GetRequiredService<ITranslationService>();
            settingsService = Splat.Locator.Current.GetRequiredService<ISettingsService>();
            applicationLogRepository = Splat.Locator.Current.GetRequiredService<IApplicationLogRepository>();
            operationHeaderRepository = Splat.Locator.Current.GetRequiredService<IOperationHeaderRepository>();

            messageBox = new MessageBox(EMessageBoxStyles.Windows);
            messageBox.IsReuseWindow = true;
            messageBox.ControlBox = false;

            boxParams = new MessageBoxParams();
            boxParams.Window = messageBox;

            dataContext = new MessageBoxModel(boxParams);
            messageBox.DataContext = dataContext;
        }

        /// <summary>
        /// Writes description of error to log file.
        /// </summary>
        /// <param name="sender">Class in which error is happened.</param>
        /// <param name="message">Description of error.</param>
        /// <param name="method">Method in which error is happened.</param>
        /// <date>13.06.2022.</date>
        public async void RegisterError(object sender, string message, string method = "")
        {
            try
            {
                using (TextWriter logFile = new StreamWriter(settingsService.LogfilePath, true))
                {
                    logFile.Write("\r\n" + "Application:{0} {1}" + "\r\n" + "Date/Time:  {2:yyyy/MM/dd HH:mm:ss.fff}" + "\r\n" + "OS:         {3}" + "\r\n" + "Object:     {4}" + "\r\n" + "Method:      {5}" + "\r\n" + "Message:    {6}", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion, DateTime.Now, Environment.OSVersion, sender.ToString(), method, message);
                    logFile.WriteLine();
                    logFile.Flush();
                    logFile.Close();
                }
            }
            catch (Exception el)
            {
                var result = await ShowDialog(translationService.Localize("msgErrorDuringWritingToLog")+ "\r\n" + el.ToString(), translationService.Localize("strError"), EButtonIcons.Error);
            }
        }

        /// <summary>
        /// Writes description of error to log file.
        /// </summary>
        /// <param name="sender">Class in which error is happened.</param>
        /// <param name="exception">Error exeption.</param>
        /// <param name="method">Method in which error is happened.</param>
        /// <date>13.06.2022.</date>
        public async void RegisterError(object sender, Exception exception, string method = "")
        {
            try
            {
                using (TextWriter logFile = new StreamWriter(settingsService.LogfilePath, true))
                {
                    logFile.Write("\r\n" + "Application:{0} {1}" + "\r\n" + "Date/Time:  {2:yyyy/MM/dd HH:mm:ss.fff}" + "\r\n" + "OS:         {3}" + "\r\n" + "Object:     {4}" + "\r\n" + "Method:      {5}" + "\r\n" + "Message:    {6}", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion, DateTime.Now, Environment.OSVersion, sender.ToString(), method, exception.Source, exception.Message, exception.TargetSite, exception.StackTrace);
                    logFile.WriteLine();
                    logFile.Flush();
                    logFile.Close();
                }
            }
            catch (Exception el)
            {
                var result = await ShowDialog(translationService.Localize("msgErrorDuringWritingToLog") + "\r\n" + el.ToString(), translationService.Localize("strError"), EButtonIcons.Error);
            }
        }

        /// <summary>
        /// Writes description of error to Database.
        /// </summary>
        /// <param name="eventID">Id of application stage on which error is happened.</param>
        /// <param name="description">Description of error.</param>
        /// <param name="operationHeaderID">The identifier of the operation in which the action took place.</param>
        /// <date>13.06.2022.</date>
        public async void RegisterError(EApplicationLogEvents eventID, string description, int operationHeaderID = 0)
        {
            try
            {
                description = description.Trim();
                if (description.Length > 3000)
                {
                    description = description.Substring(0, 3000 - 1);
                }
                OperationHeader operationHeader = operationHeaderID > 0 ? await operationHeaderRepository.GetOperationHeaderByIdAsync(operationHeaderID) : null;
                var result = await applicationLogRepository.AddApplicationLogAsync(ApplicationLog.Create(eventID,description,operationHeader));
            }
            catch (Exception ex)
            {
                RegisterError(nameof(LoggerService), ex, "WriteErrorToDatabase");
            }
        }

        /// <summary>
        /// Shows info to user.
        /// </summary>
        /// <param name="messageKey">Key to localize message that will be shown to user.</param>
        /// <param name="headerKey">Key to localize header of the message.</param>
        /// <param name="icon">Icon that will visualize message.</param>
        /// <param name="buttons">Buttons to get response from user.</param>
        /// <returns></returns>
        /// <date>14.06.2022.</date>
        public async Task<EButtonResults> ShowDialog(string messageKey, string headerKey = "", EButtonIcons icon = EButtonIcons.None, EButtons buttons = EButtons.Ok)
        {
            TaskCompletionSource<EButtonResults> taskSource = new TaskCompletionSource<EButtonResults>();
            messageBox.Closing += delegate
            {
                taskSource.TrySetResult(messageBox.Result);
            };

            boxParams.ContentHeaderKey = headerKey;
            boxParams.ContentMessageKey = messageKey;
            boxParams.Icon = icon;
            boxParams.ButtonDefinitions = buttons;

            dataContext.UpdateParams(boxParams);
            messageBox.ShowDialog(App.MainWindow);

            return await taskSource.Task;
        }

        /// <summary>
        /// Shows info to user.
        /// </summary>
        /// <param name="message">Message that will be shown to user.</param>
        /// <param name="header">Header of the message.</param>
        /// <param name="icon">Icon that will visualize message.</param>
        /// <param name="buttons">Buttons to get response from user.</param>
        /// <returns></returns>
        /// <date>14.06.2022.</date>
        public async Task<EButtonResults> ShowDialog1(string message, string header = "", EButtonIcons icon = EButtonIcons.None, EButtons buttons = EButtons.Ok)
        {
            TaskCompletionSource<EButtonResults> taskSource = new TaskCompletionSource<EButtonResults>();
            messageBox.Closing += delegate
            {
                taskSource.TrySetResult(messageBox.Result);
            };
            boxParams.ContentHeader = header;
            boxParams.ContentMessage = message;
            boxParams.Icon = icon;
            boxParams.ButtonDefinitions = buttons;

            dataContext.UpdateParams(boxParams);
            messageBox.ShowDialog(App.MainWindow);

            return await taskSource.Task;
        }
    }
}
