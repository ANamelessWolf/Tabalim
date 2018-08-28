using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabalim.Addin.Mocking;
using Tabalim.Addin.Model;

namespace Tabalim.Addin.Controller
{
    /// <summary>
    /// Define un conjunto de herramientas auxiliares que utiliza AutoCAD
    /// </summary>
    public static class AutoCADUtils
    {
        /// <summary>
        /// Define una transacción que no regresa ningun valor.
        /// </summary>
        /// <param name="TransactionTask">La transacción a realizar.</param>
        public static void VoidTransaction(Action<Document, Transaction> TransactionTask)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            using (Transaction tr = doc.Database.TransactionManager.StartTransaction())
            {
                try
                {
                    TransactionTask(doc, tr);
                    tr.Commit();
                }
                catch (Exception exc)
                {
                    doc.Editor.WriteMessage(exc.Message);
                    tr.Abort();
                }
            }
        }


        /// <summary>
        /// Dibuja una entidad en el bloque especificado
        /// </summary>
        /// <param name="ent">La entidad a dibujar.</param>
        /// <param name="tr">La transacción activa</param>
        /// <param name="block">El bloque a utilzar, debe estar en modo escritura.</param>
        /// <returns>El Object id del objeto dibujado</returns>
        public static ObjectId DrawEntity(this Entity ent, Transaction tr, BlockTableRecord block = null)
        {
            if (block == null)
            {
                var blockId = Application.DocumentManager.MdiActiveDocument.Database.CurrentSpaceId;
                block = blockId.GetObject(OpenMode.ForWrite) as BlockTableRecord;
            }
            block.AppendEntity(ent);
            tr.AddNewlyCreatedDBObject(ent, true);
            return ent.Id;
        }
        /// <summary>
        /// Devuelve la lista de los bloques cargados
        /// </summary>
        /// <param name="tr">La transacción.</param>
        /// <param name="db">La base de datos activa.</param>
        /// <returns>La relación que existe entre nombres y ids</returns>
        public static Dictionary<String, ObjectId> GetBlockTableRecordIds(this Transaction tr, Database db)
        {
            Dictionary<String, ObjectId> blocks = new Dictionary<string, ObjectId>();
            BlockTable bTab = (BlockTable)db.BlockTableId.GetObject(OpenMode.ForRead);
            DBObject obj;
            foreach (var bId in bTab)
            {
                obj = bId.GetObject(OpenMode.ForRead);
                if (obj is BlockTableRecord)
                    blocks.Add((obj as BlockTableRecord).Name, obj.Id);
            }
            return blocks;
        }
        /// <summary>
        /// Realizá la carga del bloque
        /// </summary>
        /// <param name="tr">La transacción activa.</param>
        /// <param name="localDwg">La base de datos local.</param>
        /// <param name="file">La ruta del bloque a cargar.</param>
        /// <param name="blockName">El nombre del bloque.</param>
        public static void LoadBlock(this Transaction tr, Database localDwg, string file, string blockName)
        {
            BlockTable blkTab = (BlockTable)localDwg.BlockTableId.GetObject(OpenMode.ForRead);
            if (!blkTab.Has(blockName) && File.Exists(file))
            {
                blkTab.UpgradeOpen();
                BlockTableRecord newRecord = new BlockTableRecord();
                newRecord.Name = blockName;
                blkTab.Add(newRecord);
                tr.AddNewlyCreatedDBObject(newRecord, true);
                //3: Se abre la base de datos externa
                Database externalDB = new Database();
                externalDB.ReadDwgFile(file, FileShare.Read, true, null);
                //4: Con una subtransacción se clonán los elementos que esten contenidos en el espcio de modelo de la
                //base de datos externa
                ObjectIdCollection ids;
                using (Transaction subTr = externalDB.TransactionManager.StartTransaction())
                {
                    //4.1: Abrir el espacio de modelo de la base de datos externa
                    ObjectId modelId = SymbolUtilityServices.GetBlockModelSpaceId(externalDB);
                    BlockTableRecord model = subTr.GetObject(modelId, OpenMode.ForRead) as BlockTableRecord;
                    //4.2: Se extraen y clonan los elementos mediante la clase IdMapping
                    ids = new ObjectIdCollection(model.OfType<ObjectId>().ToArray());
                    using (IdMapping iMap = new IdMapping())
                        localDwg.WblockCloneObjects(ids, newRecord.Id, iMap, DuplicateRecordCloning.Replace, false);
                }
            }
        }
        /// <summary>
        /// Actualizamos la ruta de las imagenes del tablero
        /// </summary>
        /// <param name="tr">La transacción activa.</param>
        /// <param name="doc">El documento a cargar.</param>
        /// <param name="tablero">El nombre del bloque.</param>
        public static void UpdatePath(this Transaction tr, Document doc, string tablero)
        {
            BlockTable blk = doc.Database.BlockTableId.GetObject(OpenMode.ForWrite) as BlockTable;
            if (blk.Has(tablero))
            {
                BlockTableRecord tabRec = blk[tablero].GetObject(OpenMode.ForRead) as BlockTableRecord;
                DBObject obj;
                foreach (ObjectId objId in tabRec)
                {
                    obj = objId.GetObject(OpenMode.ForRead);
                    if (obj is RasterImage)
                    {
                        obj.UpgradeOpen();
                        RasterImage img = obj as RasterImage;
                        String appData = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        String imgFile = Path.Combine(appData, "Autodesk", "ApplicationPlugins", "Tabalim.bundle", "contents", "img", "tableros", String.Format("{0}.BMP", tablero));
                        DBDictionary imgDic = RasterImageDef.GetImageDictionary(doc.Database).GetObject(OpenMode.ForRead) as DBDictionary;
                        if (File.Exists(imgFile) && imgDic.Contains(tablero))
                        {
                            RasterImageDef imgDef = imgDic.GetAt(tablero).GetObject( OpenMode.ForWrite) as RasterImageDef;
                            if (imgDef.SourceFileName != imgFile)
                            {
                                imgDef.SourceFileName = imgFile;
                                imgDef.Load();
                                //img.UpgradeOpen();
                                //img.ImageDefId = imgDef.Id;
                                //img.AssociateRasterDef(imgDef);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene la información que crea un tablero desde los datos en JSON
        /// que obtiene del porta papeles
        /// </summary>
        /// <returns>La información del tablero a insertar</returns>
        public static TableroContent GetTableroFromJSON()
        {
            TableroContent cont = null;
            try
            {
                string cp = System.Windows.Clipboard.GetText();
                if (cp != null)
                    cont = Newtonsoft.Json.JsonConvert.DeserializeObject<TableroContent>(cp);
                if (cont == null)
                    throw new Exception("Error al cargar la información del tablero. JSON no válido.");
            }
            catch (Exception exc)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(exc.Message);
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("La información del portapapeles actual no coincide con la información de un tablero.");
            }
            return cont;
        }
        /// <summary>
        /// Obtiene la información que crea una tabla de alimentadores desde los datos en JSON
        /// que obtiene del porta papeles
        /// </summary>
        /// <returns>La información del tablero a insertar</returns>
        public static AlimentadorContent GetAlimentadorFromJSON()
        {
            AlimentadorContent cont = null;
            try
            {
                string cp = System.Windows.Clipboard.GetText();
                if (cp != null)
                    cont = Newtonsoft.Json.JsonConvert.DeserializeObject<AlimentadorContent>(cp);
                if (cont == null)
                    throw new Exception("Error al cargar la información del proyecto de alimentador. JSON no válido.");
            }
            catch (Exception exc)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(exc.Message);
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("La información del portapapeles actual no coincide con la información de un tablero.");
            }
            return cont;
        }
    }
}
