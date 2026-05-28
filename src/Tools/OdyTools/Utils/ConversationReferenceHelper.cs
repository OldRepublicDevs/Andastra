using System;
using Avalonia.Controls;
using OdyTools.Data;

namespace OdyTools.Utils
{
    /// <summary>
    /// Shared conversation (DLG) reference search wiring for editor conversation combo context menus.
    /// </summary>
    public static class ConversationReferenceHelper
    {
        public static void FindAndShowConversationReferences(
            Window parent,
            ComboBox comboBox,
            OdyInstallation installation)
        {
            if (comboBox == null)
            {
                return;
            }

            string conversationName = comboBox.Text?.Trim();
            if (string.IsNullOrEmpty(conversationName))
            {
                conversationName = comboBox.SelectedItem?.ToString()?.Trim();
            }

            ReferenceSearchHelper.FindAndShowConversationReferences(
                parent,
                conversationName,
                installation,
                showOptionsDialog: true);
        }
    }
}
