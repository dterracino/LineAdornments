using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Classification;
using System.Windows.Media;

namespace Winterdom.VisualStudio.Extensions.Text {

   static class CurrentLineClassificationDefinition {
      [Export(typeof(ClassificationTypeDefinition))]
      [Name(LineHighlight.NAME)]
      internal static ClassificationTypeDefinition CurrentLineClassificationType = null;
   }
   [Export(typeof(EditorFormatDefinition))]
   [ClassificationType(ClassificationTypeNames = LineHighlight.NAME)]
   [Name(LineHighlight.NAME)]
   [DisplayName("Current Line")]
   [UserVisible(true)]
   [Order(Before = Priority.Default)]
   sealed class CurrentLineFormat : ClassificationFormatDefinition {
      public CurrentLineFormat() {
         this.BackgroundColor = Color.FromArgb(0xFF, 0xEB, 0xEB, 0xEB);
         this.ForegroundColor = Colors.Silver;
         this.BackgroundOpacity = 0.4;
      }
   }

   [Export(typeof(IWpfTextViewCreationListener))]
   [ContentType("text")]
   [TextViewRole(PredefinedTextViewRoles.Document)]
   internal sealed class EditorAdornmentFactory : IWpfTextViewCreationListener {
      [Import]
      public IClassificationTypeRegistryService ClassificationRegistry = null;
      [Import]
      public IClassificationFormatMapService FormatMapService = null;

      [Export(typeof(AdornmentLayerDefinition))]
      [Name(LineHighlight.NAME)]
      [Order(Before = "Selection")]
      [TextViewRole(PredefinedTextViewRoles.Document)]
      public AdornmentLayerDefinition editorAdornmentLayer = null;

      public void TextViewCreated(IWpfTextView textView) {
         IClassificationType classification =
            ClassificationRegistry.GetClassificationType(LineHighlight.NAME);
         IClassificationFormatMap map =
            FormatMapService.GetClassificationFormatMap(textView);
         new LineHighlight(textView, map, classification);
      }
   }
}
