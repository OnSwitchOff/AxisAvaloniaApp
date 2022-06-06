using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AxisAvaloniaApp.ViewModels
{
    public class CashRegisterViewModel : ViewModelBase
    {
        #region Fields
        private Control content;
        private double cashAmount;
        private string description;
        #endregion

        #region Properties
        public Control Content { get => content; set => this.RaiseAndSetIfChanged(ref content, value); }
        public double CashAmount
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
            Z_ReportCommand = ReactiveCommand.Create(Z_Report);
            X_ReportCommand = ReactiveCommand.Create(X_Report);
            DuplicateChequeCommand = ReactiveCommand.Create(DuplicateCheque);
            DepositeCashCommand = ReactiveCommand.Create(DepositeCash);
            WithdrawCashCommand = ReactiveCommand.Create(WithdrawCash);
            CurrentMonthReportCommand = ReactiveCommand.Create(CurrentMonthReport);
            LastMonthReportCommand = ReactiveCommand.Create(LastMonthReport);
            ResetPOSterminalCommand = ReactiveCommand.Create(ResetPOSterminal);
            ClearCommand = ReactiveCommand.Create(Clear);
        }

        
        private void Z_Report()
        {
            CashAmount = 100;
        }

        private void X_Report()
        {

        }

        private void DuplicateCheque()
        {

        }
        private void DepositeCash()
        {

        }
        private void WithdrawCash()
        {

        }
        private void CurrentMonthReport()
        {

        }
        private void LastMonthReport()
        {

        }
        private void ResetPOSterminal()
        {

        }
        private void Clear()
        {

        }
    }
}
