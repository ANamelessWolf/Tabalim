using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Controller;
using Tabalim.Addin.Mocking;
using Tabalim.Addin.Model;

namespace Tabalim.Addin.Runtime
{
    /// <summary>
    /// Define la clase de comandos
    /// </summary>
    public class Commands
    {
        [CommandMethod("PEGAR_TABLERO")]
        public void InsertTablero()
        {
            TableroContent cont = AutoCADUtils.GetTableroFromJSON();
            if (cont == null)
                return;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            var res = ed.GetPoint("Selecciona el punto de inserción de la tabla");
            if (res.Status == PromptStatus.OK)
            {
                cont.LoadBlocks();
                AutoCADUtils.VoidTransaction((Document doc, Transaction tr) =>
                {
                    TableroTable table = new TableroTable(res.Value, cont);
                    table.Blocks = tr.GetBlockTableRecordIds(doc.Database);
                    table.Init();
                    table.Insert(doc, tr);
                });

            }
        }
        [CommandMethod("PEGAR_Alimentador")]
        public void InsertAlimentador()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            AlimentadorContent fullContent = AutoCADUtils.GetAlimentadorFromJSON();
            List<AlimentadorContent> contentByPages = new List<AlimentadorContent>();
            if (fullContent == null)
                return;
            else if (fullContent.Lineas.Length > 20)
            {
                contentByPages.Add(fullContent);
                while (contentByPages.Last().Lineas.Length > 20)
                {
                    var lines = contentByPages.Last().Lineas;
                    var row = lines.Take(20);
                    var skip = lines.Skip(20);
                    lines = row.ToArray();
                    contentByPages.Add(contentByPages.Last().Clone(skip));
                }
            }
            else
                contentByPages.Add(fullContent);
            AutoCADUtils.VoidTransaction((Document doc, Transaction tr) =>
            {
                foreach (var content in contentByPages)
                {
                    var res = ed.GetPoint("Selecciona el punto de inserción de la tabla");
                    if (res.Status == PromptStatus.OK)
                    {
                        fullContent.LoadBlocks();
                        AlimTable table = new AlimTable(res.Value, content);
                        table.Init();
                        table.Insert(doc, tr);
                    }
                }
            });
        }
    }
}
