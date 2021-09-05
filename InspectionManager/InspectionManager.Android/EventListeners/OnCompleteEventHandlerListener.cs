using System;
using Android.Gms.Tasks;

namespace InspectionManager.Droid.EventListeners
{
    public class OnCompleteEventHandlerListener: Java.Lang.Object, IOnCompleteListener
    {
        private readonly Action<Task> _completeAction;

        public OnCompleteEventHandlerListener(Action<Task> completeAction)
        {
            _completeAction = completeAction;
        }

        public void OnComplete(Task task) => _completeAction(task);
    }
}
