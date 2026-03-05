namespace OdyPatch.UI.Rte
{
    public static class RteDocumentConverter
    {
        public static RteDocument FromFlowDocument(object document)
        {
            return new RteDocument
            {
                Content = document == null ? string.Empty : document.ToString()
            };
        }

        public static void ApplyToRichTextBox(object richTextBox, RteDocument document)
        {
        }
    }
}
