# BioWare Aurora Merchant

Merchant resources use the `UTM` GFF layout. In OdyTools.NET, open them with the
Merchant editor to edit the store identity, price markups, scripting hooks, and
inventory list.

## Common Fields

- `Tag` identifies the merchant in scripts and module data.
- `TemplateResRef` is the resource reference used by the module.
- `ID` is the merchant identifier stored in the resource.
- `MarkUp` and `MarkDown` control buy and sell price adjustment.
- `OnOpenStore` references the script fired when the store opens.
- `Inventory` contains store items and item availability flags.

## Editing Notes

Keep ResRefs at 16 characters or fewer and avoid characters that are invalid in
Windows filenames. OdyTools trims valid ResRefs and clears invalid values before
building the saved `UTM`.

