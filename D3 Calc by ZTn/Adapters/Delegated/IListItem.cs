using System;

using Android.Views;

namespace ZTnDroid.D3Calculator.Adapters
{
    /// <summary>
    /// Interface to be respected when using <see cref="ListAdapter"/>
    /// </summary>
    public interface IListItem
    {
        /// <summary>
        /// Return the layout selectedResource to be used to create the view for this item
        /// </summary>
        /// <returns></returns>
        int getLayoutResource();

        /// <summary>
        /// Return true is the item at the specified position is not a separator
        /// </summary>
        /// <returns></returns>
        bool isEnabled();

        /// <summary>
        /// Called when the view needs to be updated with new data
        /// </summary>
        /// <param name="view"><see cref="View"/> object to be updated</param>
        /// <param name="recycled">Should be <c>true</c> if the view is recycled and not a newly created one</param>
        void updateView(View view, Boolean recycled);

        /// <summary>
        /// Called when the view is to be removed or recycled
        /// </summary>
        /// <param name="view"><see cref="View"/> object to be removed</param>
        void removeView(View view);
    }
}