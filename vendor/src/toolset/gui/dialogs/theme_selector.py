from __future__ import annotations

from typing import TYPE_CHECKING

from qtpy.QtCore import Qt, Signal  # pyright: ignore[reportPrivateImportUsage]
from qtpy.QtWidgets import (
    QDialog,
    QDialogButtonBox,
    QListWidgetItem,
    QPushButton,
)

from toolset.gui.common.localization import translate as tr

if TYPE_CHECKING:
    from qtpy.QtWidgets import QWidget


class ThemeSelectorDialog(QDialog):
    """Non-blocking dialog for selecting application themes and styles."""

    theme_changed = Signal(str)  # pyright: ignore[reportPrivateImportUsage]
    style_changed = Signal(str)  # pyright: ignore[reportPrivateImportUsage]

    def __init__(
        self,
        parent: QWidget | None = None,
        available_themes: list[str] | None = None,
        available_styles: list[str] | None = None,
        current_theme: str | None = None,
        current_style: str | None = None,
    ):
        """Initialize the theme selector dialog.

        Args:
        ----
            parent: Parent widget
            available_themes: List of available theme names
            available_styles: List of available style names
            current_theme: Currently selected theme name
            current_style: Currently selected style name
        """
        super().__init__(parent)
        self._available_themes = available_themes or []
        self._available_styles = available_styles or []
        self._current_theme = current_theme
        self._current_style = current_style
        self._init_ui()
        self._populate_lists()

        # Setup event filter to prevent scroll wheel interaction with controls
        from toolset.gui.common.filters import NoScrollEventFilter

        self._no_scroll_filter = NoScrollEventFilter(self)
        self._no_scroll_filter.setup_filter(parent_widget=self)

    def _init_ui(self):
        """Set up the user interface."""
        from toolset.uic.qtpy.dialogs.theme_selector import Ui_Dialog

        self.ui = Ui_Dialog()
        self.ui.setupUi(self)

        self.setWindowTitle(tr("Theme Selection"))
        self.setWindowFlags(Qt.WindowType.Dialog | Qt.WindowType.WindowCloseButtonHint | Qt.WindowType.WindowTitleHint)

        # Set stretch factor for current theme/style displays
        self.ui.themesHeaderLayout.setStretchFactor(self.ui.currentThemeDisplay, 1)
        self.ui.stylesHeaderLayout.setStretchFactor(self.ui.currentStyleDisplay, 1)

        # Connect signals
        self.ui.searchEdit.textChanged.connect(self._filter_items)
        self.ui.themesList.itemClicked.connect(self._on_theme_selected)
        self.ui.themesList.itemDoubleClicked.connect(self._on_theme_double_clicked)
        self.ui.stylesList.itemClicked.connect(self._on_style_selected)
        self.ui.stylesList.itemDoubleClicked.connect(self._on_style_double_clicked)
        self.ui.buttonBox.rejected.connect(self.close)

        # Apply button (for immediate application)
        apply_button = QPushButton(tr("Apply"))
        apply_button.setDefault(True)
        apply_button.clicked.connect(self._apply_selection)
        self.ui.buttonBox.addButton(apply_button, QDialogButtonBox.ButtonRole.AcceptRole)

    def _populate_lists(self):
        """Populate the theme and style lists."""
        # Update current theme display
        if self._current_theme:
            self.ui.currentThemeDisplay.setText(self._current_theme)
        else:
            self.ui.currentThemeDisplay.clear()

        # Populate themes
        self.ui.themesList.clear()
        for theme_name in sorted(self._available_themes):
            item = QListWidgetItem(theme_name)
            item.setCheckState(Qt.CheckState.Unchecked)
            if self._current_theme and theme_name.lower() == self._current_theme.lower():
                item.setCheckState(Qt.CheckState.Checked)
                self.ui.themesList.setCurrentItem(item)
                self.ui.themesList.scrollToItem(item)
            self.ui.themesList.addItem(item)

        # Update current style display
        if self._current_style == "":
            self.ui.currentStyleDisplay.setText(tr("Native (System Default)"))
        elif self._current_style:
            self.ui.currentStyleDisplay.setText(self._current_style)
        else:
            self.ui.currentStyleDisplay.clear()

        # Populate styles
        self.ui.stylesList.clear()
        # Add "Native (System Default)" option
        native_item = QListWidgetItem(tr("Native (System Default)"))
        native_item.setData(Qt.ItemDataRole.UserRole, "")
        native_item.setCheckState(Qt.CheckState.Unchecked)
        if self._current_style == "":
            native_item.setCheckState(Qt.CheckState.Checked)
            self.ui.stylesList.setCurrentItem(native_item)
        self.ui.stylesList.addItem(native_item)

        # Add other styles
        for style_name in sorted(self._available_styles):
            item = QListWidgetItem(style_name)
            item.setData(Qt.ItemDataRole.UserRole, style_name)
            item.setCheckState(Qt.CheckState.Unchecked)
            if self._current_style == style_name:
                item.setCheckState(Qt.CheckState.Checked)
                self.ui.stylesList.setCurrentItem(item)
                self.ui.stylesList.scrollToItem(item)
            self.ui.stylesList.addItem(item)

    def _filter_items(self, text: str):
        """Filter the theme and style lists based on search text."""
        search_text = text.lower()

        # Filter themes
        for i in range(self.ui.themesList.count()):
            item = self.ui.themesList.item(i)
            if item:
                item.setHidden(search_text not in item.text().lower())

        # Filter styles
        for i in range(self.ui.stylesList.count()):
            item = self.ui.stylesList.item(i)
            if item:
                item.setHidden(search_text not in item.text().lower())

    def _on_theme_selected(self, item: QListWidgetItem):
        """Handle theme selection."""
        # Uncheck all themes
        for i in range(self.ui.themesList.count()):
            list_item = self.ui.themesList.item(i)
            if list_item:
                list_item.setCheckState(Qt.CheckState.Unchecked)

        # Check selected theme
        item.setCheckState(Qt.CheckState.Checked)

        # Update current theme display
        self.ui.currentThemeDisplay.setText(item.text())

    def _on_style_selected(self, item: QListWidgetItem):
        """Handle style selection."""
        # Uncheck all styles
        for i in range(self.ui.stylesList.count()):
            list_item = self.ui.stylesList.item(i)
            if list_item:
                list_item.setCheckState(Qt.CheckState.Unchecked)

        # Check selected style
        item.setCheckState(Qt.CheckState.Checked)

        # Update current style display
        style_display_name = item.text()  # Use the display text (e.g., "Native (System Default)" or style name)
        self.ui.currentStyleDisplay.setText(style_display_name)

    def _on_theme_double_clicked(self, item: QListWidgetItem):
        """Handle theme double-click - apply immediately."""
        self._on_theme_selected(item)
        self._apply_selection()

    def _on_style_double_clicked(self, item: QListWidgetItem):
        """Handle style double-click - apply immediately."""
        self._on_style_selected(item)
        self._apply_selection()

    def _apply_selection(self):
        """Apply the selected theme and/or style."""
        # Check for selected theme
        selected_theme = None
        for i in range(self.ui.themesList.count()):
            item = self.ui.themesList.item(i)
            if item and item.checkState() == Qt.CheckState.Checked:
                selected_theme = item.text()
                break

        # Check for selected style
        selected_style = None
        for i in range(self.ui.stylesList.count()):
            item = self.ui.stylesList.item(i)
            if item and item.checkState() == Qt.CheckState.Checked:
                selected_style = item.data(Qt.ItemDataRole.UserRole) or ""
                break

        # Apply theme if selected
        if selected_theme:
            self.theme_changed.emit(selected_theme)

        # Apply style if selected
        if selected_style is not None:
            self.style_changed.emit(selected_style)

    def update_current_selection(self, theme_name: str | None = None, style_name: str | None = None):
        """Update the current selection in the dialog."""
        self._current_theme = theme_name
        self._current_style = style_name
        self._populate_lists()
