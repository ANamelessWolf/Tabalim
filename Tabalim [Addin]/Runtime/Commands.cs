using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Controller;
using Tabalim.Addin.Mocking;
using Tabalim.Addin.Model;
using static Tabalim.Addin.Assets.AddinConstants;
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
        [CommandMethod("PEGAR_ALIM")]
        public void InsertAlimentador()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            AlimentadorContent fullContent = AutoCADUtils.GetAlimentadorFromJSON();
            AutoCADUtils.VoidTransaction((Document doc, Transaction tr) =>
            {
                if (fullContent != null && fullContent.Lineas.Length > 0)
                {
                    var saver = new FormatSaver(fullContent.Lineas);
                    saver.Save(doc, tr);
                }
            });
            if (fullContent != null)
            {
                List<AlimentadorContent> contentByPages = AlimentadorContent.FixContentByPages(fullContent);
                foreach (var content in contentByPages)
                {
                    AutoCADUtils.VoidTransaction((Document doc, Transaction tr) =>
                    {
                        var res = ed.GetPoint("Selecciona el punto de inserción de la tabla");
                        if (res.Status == PromptStatus.OK)
                        {
                            AlimTable table = new AlimTable(res.Value, content);
                            table.Init();
                            table.Insert(doc, tr);
                        }
                    });
                    ed.Regen();
                }
            }
        }
        [CommandMethod("PEGAR_LINEAS_HOR")]
        public void InsertHorizontalLines()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            AutoCADUtils.VoidTransaction((Document doc, Transaction tr) =>
            {
                FormatSaver sav = new FormatSaver();
                sav.InsertHorizontalTags(doc, tr);
            });
            ed.Regen();
        }
        [CommandMethod("PEGAR_LINEAS_VER")]
        public void InsertVerticalLines()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            AutoCADUtils.VoidTransaction((Document doc, Transaction tr) =>
            {
                FormatSaver sav = new FormatSaver();
                sav.InsertVerticalTags(doc, tr);
            });
            ed.Regen();
        }
        [CommandMethod("INSERTAR_BLOQUE")]
        public void InsertTabalimBlock()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            String blkDirectory = Assembly.GetAssembly(typeof(Tabalim.Addin.Runtime.Commands)).Location;
            blkDirectory = Path.GetDirectoryName(blkDirectory);
            blkDirectory = Path.Combine(blkDirectory, BLOCK_DIR);
            View.WinBlockInsert win = new View.WinBlockInsert();
            win.ShowDialog();
            AutoCADUtils.VoidTransaction((Document doc, Transaction tr) =>
            {
                if (win.DialogResult.Value)
                {
                    String blockName = win.SelectedBlock.Index.ToString(),
                    file = Path.Combine(blkDirectory, String.Format("{0}.dwg", blockName));
                    tr.LoadBlock(doc.Database, file, blockName);
                };
            });
            PromptStatus status = PromptStatus.OK;
            PromptPointResult res;
            while (status == PromptStatus.OK)
                AutoCADUtils.VoidTransaction((Document doc, Transaction tr) =>
            {
                String blockName = win.SelectedBlock.Index.ToString();
                BlockTable blkTab = (BlockTable)doc.Database.BlockTableId.GetObject(OpenMode.ForRead);
                BlockTableRecord currentSpace = (BlockTableRecord)doc.Database.CurrentSpaceId.GetObject(OpenMode.ForWrite);
                res = ed.GetPoint(new PromptPointOptions("Selecciona el punto de inserción del bloque.") { AllowNone = true });
                status = res.Status;
                if (status == PromptStatus.OK)
                {
                    BlockReference blkRef = new BlockReference(res.Value, blkTab[blockName]);
                    blkRef.Rotation = 0;
                    blkRef.ScaleFactors = new Scale3d(16);
                    currentSpace.AppendEntity(blkRef);
                    tr.AddNewlyCreatedDBObject(blkRef, true);
                }
                ed.Regen();
            });
        }
    }
}
