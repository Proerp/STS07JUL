using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

using Global.Class.Library;

namespace PresentationLayer
{
    interface IMergeToolStrip
    {
        ToolStrip ChildToolStrip { get; set; }
    }


     public enum PrintDestination
        {
            PrintPreview = 0,
            Print = 1,
            Export = 2
        }



     interface ICallToolStrip : INotifyPropertyChanged
     {
         GlobalEnum.TaskID TaskID { get; }

         void Escape();
         void Loading();

         void New();
         void Edit();
         void Save();
         void Delete();

         void Verify();

         void Import();
         void Export();

         void Print(PrintDestination printDestination);

         void SearchText(string searchText);


         bool Closable { get; }
         bool Loadable { get; }

         bool Newable { get; }
         bool Editable { get; }
         bool IsDirty { get; }
         bool IsValid { get; }
         bool Deletable { get; }

         bool Verifiable { get; }
         bool Unverifiable { get; }

         bool Printable { get; }

         bool Importable { get; }
         bool Exportable { get; }


         bool Searchable { get; }


         bool ReadonlyMode { get; }
         bool EditableMode { get; }
     }

}
