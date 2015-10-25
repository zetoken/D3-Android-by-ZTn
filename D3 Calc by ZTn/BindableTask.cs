using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ZTn.Pcl.D3Calculator
{
    /// <summary>
    /// Inspired by http://msdn.microsoft.com/en-us/magazine/dn605875.aspx
    /// </summary>
    /// <typeparam name="TResult">Type of data returned by the task.</typeparam>
    public class BindableTask<TResult> : INotifyPropertyChanged
    {
        public Task<TResult> Task { get; }

        public TResult Result => (Task.Status == TaskStatus.RanToCompletion ? Task.Result : default(TResult));

        public TaskStatus Status => Task.Status;

        public bool IsCompleted => Task.IsCompleted;

        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;

        public bool IsCanceled => Task.Status == TaskStatus.Canceled;

        public bool IsFaulted => Task.Status == TaskStatus.Faulted;

        public BindableTask(Task<TResult> task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        private async Task WatchTaskAsync(Task<TResult> task)
        {
            try
            {
                await task;
            }
            catch (Exception)
            {
                // ignored
            }

            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
            {
                return;
            }

            propertyChanged(this, new PropertyChangedEventArgs(nameof(Status)));
            propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));

            if (Task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsCanceled)));
            }
            else if (Task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(IsFaulted)));
            }
            else
            {
                propertyChanged(this, new PropertyChangedEventArgs(nameof(Result)));
            }
        }

        #region >> INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
