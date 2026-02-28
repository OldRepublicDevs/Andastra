"""Shared tooltip helpers for appending reference-search and other hints.

Tooltips should be defined in .ui files with full descriptive text (as rich HTML).
Code-behind must only append hints (e.g. Right-click to find references) and must
use HTML so tooltips remain consistently styled. Never overwrite .ui tooltips.
"""

from __future__ import annotations

from typing import TYPE_CHECKING

from toolset.gui.common.localization import translate as tr

if TYPE_CHECKING:
    from qtpy.QtWidgets import QWidget


REFERENCE_TYPES = ("script", "tag", "template_resref", "conversation")


def append_reference_search_tooltip(
    widget: QWidget,
    reference_type: str = "script",
) -> None:
    """Append the 'Right-click to find references' hint to the widget's existing tooltip.

    The existing tooltip (from .ui or set earlier) is preserved. The hint is appended
    as italic HTML so it is styled consistently. Use this instead of setToolTip() when
    enabling reference search on a field that already has a descriptive tooltip in .ui.

    Args:
        widget: The widget whose tooltip to append to (e.g. tagEdit, resrefEdit).
        reference_type: One of 'script', 'tag', 'template_resref', 'conversation'.
    """
    base = widget.toolTip() or ""
    if reference_type == "script":
        hint = tr("Right-click to find references to this script in the installation.")
    elif reference_type == "tag":
        hint = tr("Right-click to find references to this tag in the installation.")
    elif reference_type == "template_resref":
        hint = tr("Right-click to find references to this template resref in the installation.")
    elif reference_type == "conversation":
        hint = tr("Right-click to find references to this conversation in the installation.")
    else:
        hint = tr("Right-click to find references in the installation.")
    # Append as italic so tooltip remains rich text
    widget.setToolTip(base + " <i>(" + hint + ")</i>")
