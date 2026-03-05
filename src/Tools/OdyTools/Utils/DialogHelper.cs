using System;
using OdyTools.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace OdyTools.Utils
{
    /// <summary>
    /// Centralized utility for creating and displaying message boxes in OdyTools dialogs and windows.
    /// Provides fire-and-forget asynchronous message display with consistent styling and patterns.
    /// Reduces code duplication across multiple editor and dialog components.
    /// </summary>
    public static class DialogHelper
    {
        /// <summary>
        /// Display a modal message box with custom buttons and icon asynchronously.
        /// Fire-and-forget: returns task but does not block execution.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message text</param>
        /// <param name="buttons">Button configuration (Ok, YesNo, etc.)</param>
        /// <param name="icon">Icon type (Info, Warning, Error, Success, Question)</param>
        /// <returns>Task that completes when dialog closes; returns button result</returns>
        public static Task<ButtonResult> ShowAsync(string title, string message, ButtonEnum buttons, Icon icon)
        {
            return MessageBoxManager
                .GetMessageBoxStandard(title, message, buttons, icon)
                .ShowAsync();
        }

        /// <summary>
        /// Display a modal message box with custom buttons and icon, centered over a parent window.
        /// Blocks the parent window and waits for user response.
        /// </summary>
        /// <param name="owner">Parent window to center dialog over</param>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message text</param>
        /// <param name="buttons">Button configuration (Ok, YesNo, etc.)</param>
        /// <param name="icon">Icon type (Info, Warning, Error, Success, Question)</param>
        /// <returns>Task awaitable for dialog completion; returns button result</returns>
        public static Task<ButtonResult> ShowWindowAsync(Window owner, string title, string message, ButtonEnum buttons, Icon icon)
        {
            return MessageBoxManager
                .GetMessageBoxStandard(title, message, buttons, icon)
                .ShowWindowDialogAsync(owner);
        }

        /// <summary>
        /// Display a simple message box over a parent window with "OK" button.
        /// Fire-and-forget: does not wait for user response; dialog closes on OK.
        /// </summary>
        /// <param name="owner">Parent window to center dialog over</param>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message text</param>
        /// <param name="icon">Icon type (Info, Warning, Error, Success)</param>
        public static void ShowWindow(Window owner, string title, string message, Icon icon)
        {
            _ = ShowWindowAsync(owner, title, message, ButtonEnum.Ok, icon);
        }

        /// <summary>
        /// Display a warning message with "OK" button. Fire-and-forget async convenience method.
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Warning message text</param>
        /// <returns>Task that completes when dialog closes</returns>
        public static Task ShowWarningAsync(string title, string message)
        {
            return ShowAsync(title, message, ButtonEnum.Ok, Icon.Warning);
        }

        /// <summary>
        /// Show a message asking user to select an item before proceeding.
        /// Common pattern for validation errors when required selection is missing.
        /// </summary>
        /// <param name="owner">Parent window</param>
        /// <param name="itemType">Type of item to select (e.g., "a character", "a module")</param>
        /// <returns>Task that completes when user closes dialog</returns>
        public static Task ShowSelectFirstAsync(Window owner, string itemType)
        {
            return ShowWindowAsync(owner, "Selection Required", $"Please select {itemType} first.", ButtonEnum.Ok, Icon.Info);
        }

        /// <summary>
        /// Build a truncated list message with "...and X more" suffix if items exceed limit.
        /// Common pattern for error/validation result lists shown in dialogs.
        /// </summary>
        /// <param name="items">List of items to display</param>
        /// <param name="maxDisplay">Maximum number of items to show (default 10)</param>
        /// <param name="prefix">Optional prefix before each item (default empty)</param>
        /// <returns>Formatted string with items and overflow indicator if applicable</returns>
        public static string BuildTruncatedList(IEnumerable<string> items, int maxDisplay = 10, string prefix = "")
        {
            var itemList = items.ToList();
            if (itemList.Count == 0)
                return string.Empty;

            var displayed = itemList.Take(maxDisplay).Select(i => prefix + i).ToList();
            string message = string.Join("\n", displayed);
            if (itemList.Count > maxDisplay)
            {
                message += $"\n... and {itemList.Count - maxDisplay} more.";
            }
            return message;
        }

        /// <summary>
        /// Show validation or error results with automatic truncation for readability.
        /// Common pattern across multiple editors for showing validation and error messages.
        /// </summary>
        /// <param name="owner">Parent window</param>
        /// <param name="title">Dialog title</param>
        /// <param name="errors">List of error/validation messages</param>
        /// <param name="maxDisplay">Maximum number of items to show before truncating</param>
        /// <param name="icon">Icon type (default Error)</param>
        /// <returns>Task that completes when user closes dialog</returns>
        public static async Task ShowErrorListAsync(Window owner, string title, IEnumerable<string> errors, int maxDisplay = 10, Icon icon = Icon.Error)
        {
            string message = BuildTruncatedList(errors, maxDisplay, "• ");
            await ShowWindowAsync(owner, title, message, ButtonEnum.Ok, icon);
    }

    /// <summary>
    /// Show a simple error message from an exception.
    /// Convenience method for displaying exception details in a consistent manner.
    /// </summary>
    /// <param name="owner">Parent window</param>
    /// <param name="exception">Exception to display</param>
    public static void ShowErrorFromException(Window owner, Exception exception)
    {
        ShowWindow(owner, Localization.Tr("Error"), exception?.Message ?? "An error occurred.", Icon.Error);
        }
    }
}
