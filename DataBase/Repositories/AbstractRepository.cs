using System.Timers;

namespace DataBase.Repositories
{
    public abstract class AbstractRepository
    {
        private bool canExecute;
        private Timer timer;

        public AbstractRepository()
        {
            canExecute = true;
            timer = new Timer(30000);
            timer.Elapsed += EnabledCanExecuteProperty;
        }

        protected bool CanExecute
        { 
            get => canExecute;
            set
            {
                canExecute = value;

                if (!canExecute)
                {
                    timer.Start();
                }
            }
        }

        internal void WaitPreviousExecution()
        {
            while (!CanExecute)
            {

            }
        }

        private void EnabledCanExecuteProperty(object sender, ElapsedEventArgs e)
        {
            CanExecute = true;
            timer.Stop();
        }
    }
}
