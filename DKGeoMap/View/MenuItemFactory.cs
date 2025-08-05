using System;
using System.Windows.Forms;

namespace DKGeoMap.View
{
    public static class MenuItemFactory
    {
        /// <summary>
        /// Creates a ToolStripMenuItem for an overlay toggle.
        /// </summary>
        /// <param name="overlayName">Display name for the overlay.</param>
        /// <param name="isChecked">Initial checked state.</param>
        /// <param name="onClick">Click event handler (async supported).</param>
        /// <returns>Configured ToolStripMenuItem.</returns>
        public static ToolStripMenuItem CreateOverlayMenuItem(string overlayName, bool isChecked, Func<ToolStripMenuItem, EventArgs, System.Threading.Tasks.Task> onClick)
        {
            var menuItem = new ToolStripMenuItem(overlayName)
            {
                Checked = isChecked,
                CheckOnClick = false // Manual control
            };

            menuItem.Click += async (s, e) =>
            {
                await onClick(menuItem, e);
            };

            return menuItem;
        }
    }
}