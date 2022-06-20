using Avalonia.Controls;
using AxisAvaloniaApp.Helpers;
using AxisAvaloniaApp.Services.Payment;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class CashRegisterViewModel : OperationViewModelBase
    {
        #region Fields
        private readonly IPaymentService paymentService;
        private Control content;
        private decimal cashAmount;
        private string description;
            
        #endregion

        #region Properties
        public Control Content { get => content; set => this.RaiseAndSetIfChanged(ref content, value); }
        public decimal CashAmount
        { 
            get => cashAmount;
            set 
            { 
                this.RaiseAndSetIfChanged(ref cashAmount, value);
            } 
        }       
        public string Description { get => description; set => this.RaiseAndSetIfChanged(ref description, value); }

      
        #endregion

        #region Commands
        public ReactiveCommand<Unit, Unit> Z_ReportCommand { get; }
        public ReactiveCommand<Unit, Unit> X_ReportCommand { get; }
        public ReactiveCommand<Unit, Unit> DuplicateChequeCommand { get; }
        public ReactiveCommand<Unit, Unit> DepositeCashCommand { get; }
        public ReactiveCommand<Unit, Unit> WithdrawCashCommand { get; }
        public ReactiveCommand<Unit, Unit> CurrentMonthReportCommand { get; }
        public ReactiveCommand<Unit, Unit> LastMonthReportCommand { get; }
        public ReactiveCommand<Unit, Unit> ResetPOSterminalCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }
        #endregion


        public CashRegisterViewModel()
        {
            paymentService = Splat.Locator.Current.GetRequiredService<IPaymentService>();

            Z_ReportCommand = ReactiveCommand.Create(Z_Report, paymentService.ObservableFiscalDeviceInitializedState);
            X_ReportCommand = ReactiveCommand.Create(X_Report, paymentService.ObservableFiscalDeviceInitializedState);
            DuplicateChequeCommand = ReactiveCommand.Create(DuplicateCheque, paymentService.ObservableFiscalDeviceInitializedState);
            DepositeCashCommand = ReactiveCommand.Create(DepositeCash, paymentService.ObservableFiscalDeviceInitializedState);
            WithdrawCashCommand = ReactiveCommand.Create(WithdrawCash, paymentService.ObservableFiscalDeviceInitializedState);
            CurrentMonthReportCommand = ReactiveCommand.Create(CurrentMonthReport, paymentService.ObservableFiscalDeviceInitializedState);
            LastMonthReportCommand = ReactiveCommand.Create(LastMonthReport, paymentService.ObservableFiscalDeviceInitializedState);
            ResetPOSterminalCommand = ReactiveCommand.Create(ResetPOSterminal, paymentService.ObservableFiscalDeviceInitializedState);
            ClearCommand = ReactiveCommand.Create(Clear, paymentService.ObservableFiscalDeviceInitializedState);

            //OnNext(paymentService.FiscalDeviceInitialized);
            paymentService.ObservableFiscalDeviceInitializedState.Subscribe(OnFiscalDeviceInitializedChanged);
        }

        private void OnFiscalDeviceInitializedChanged(bool flag)
        {
            Description = flag ? String.Empty : "Настройте меня!!!";
        }
        
        private void Z_Report()
        {
            Description += "\n" + paymentService.FiscalDevice.PrintDailyReportZ().Result.ToString();
        }

        private void X_Report()
        {
            Description += "\n" + paymentService.FiscalDevice.PrintDailyReportX().Result.ToString();
        }

        private void DuplicateCheque()
        {
            Description += "\n" + paymentService.FiscalDevice.PrintDuplicateCheque().Result.ToString();
        }
        private void DepositeCash()
        {
            Description += "\n" + paymentService.FiscalDevice.DepositeCash(CashAmount).Result.ToString();
        }
        private void WithdrawCash()
        {
            Description += "\n" + paymentService.FiscalDevice.WithdrawCash(CashAmount).Result.ToString();
        }
        private void CurrentMonthReport()
        {
            Description += "\n" + paymentService.FiscalDevice.CurrentMonthReport().Result.ToString();
        }
        private void LastMonthReport()
        {
            Description += "\n" + paymentService.FiscalDevice.LastMonthReport().Result.ToString();
        }
        private void ResetPOSterminal()
        {
            Description += "\n" + paymentService.FiscalDevice.ResetPOSterminal().Result.ToString();
        }
        private void Clear()
        {
            Description = String.Empty;
        }
    }

}
